using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using MunicipalityTaxes.Core;
using MunicipalityTaxes.Core.Domain.Classifications;
using MunicipalityTaxes.Core.Domain.Taxes;
using MunicipalityTaxes.Core.Logging;
using MunicipalityTaxes.Data.Migrations;

namespace MunicipalityTaxes.Data
{
    public partial class DataAccess : DbContext
    {
        static DataAccess()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataAccess, Configuration>());
        }

        public DataAccess()
            : base("Name=MunicipalityTaxes")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.AutoDetectChangesEnabled = false;
            Configuration.ProxyCreationEnabled = false;
            Configuration.ValidateOnSaveEnabled = false;
            Configuration.EnsureTransactionsForFunctionsAndCommands = false;

            // this.Database.Log = s => LogManager.GetLogger(typeof(DataAccess)).Debug(s);
        }

        public DbSet<Municipality> Municipalities { get; set; }

        public DbSet<Tax> Taxes { get; set; }


        public virtual IQueryable<T> Get<T>(params Expression<Func<T, object>>[] includes) where T : class
        {
            return this.Get<T>(false, includes);
        }

        public virtual IQueryable<T> Get<T>(bool includeDeleted = false, params Expression<Func<T, object>>[] includes) where T : class
        {
            IQueryable<T> query = this.Set<T>();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }


            return query.AsNoTracking();
        }

        public virtual void Insert<T>(T entity) where T : class
        {
            this.Set<T>().Add(entity);
        }

        public virtual void Update<T>(T entity) where T : class
        {
            var entry = this.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                var attachedEntity = typeof(T).GetInterfaces().Contains(typeof(BaseEntity))
                                        ? this.Set<T>().Local.SingleOrDefault(e => ((e as BaseEntity).Id == (entity as BaseEntity).Id))
                                        : null;

                if (attachedEntity != null)
                {
                    var attachedEntry = this.Entry(attachedEntity);
                    attachedEntry.CurrentValues.SetValues(entity);
                    attachedEntry.State = EntityState.Modified;
                }
                else
                {
                    this.Set<T>().Attach(entity);
                    entry.State = EntityState.Modified;
                }
            }
            else
            {
                entry.State = EntityState.Modified;
            }
        }

        public virtual void Update<T, O>(T entity, Expression<Func<T, O>> onlyFields)
            where T : class
            where O : class
        {
            if (onlyFields == null)
            {
                this.Update(entity);
            }
            else
            {
                var entry = this.Entry(entity);

                if (entry.State == EntityState.Detached)
                {
                    var attachedEntity = typeof(T).GetInterfaces().Contains(typeof(BaseEntity))
                                            ? this.Set<T>().Local.SingleOrDefault(e => ((e as BaseEntity).Id == (entity as BaseEntity).Id))
                                            : null;

                    if (attachedEntity != null)
                    {
                        var attachedEntry = this.Entry(attachedEntity);
                        this.MarkPropertiesAsModified(attachedEntry, onlyFields);
                        foreach (var nexm in ((NewExpression)onlyFields.Body).Members)
                        {
                            attachedEntry.Property(nexm.Name).CurrentValue = entry.Property(nexm.Name).CurrentValue;
                            attachedEntry.Property(nexm.Name).IsModified = true;
                        }
                    }
                    else
                    {
                        this.Set<T>().Attach(entity);
                        this.MarkPropertiesAsModified(entry, onlyFields);
                    }
                }
                else
                {
                    this.MarkPropertiesAsModified(entry, onlyFields);
                }
            }
        }


        public virtual void Delete<T>(T entity) where T : class
        {
            var entry = this.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                this.Set<T>().Attach(entity);
            }

            this.Set<T>().Remove(entity);
        }

        public virtual void Save()
        {
            try
            {
                this.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        var msg = string.Format("Property: '{0}' Error: '{1}'", validationError.PropertyName, validationError.ErrorMessage);
                        Logger.Get(((object)this).GetType()).Error(msg);
                    }
                }

                throw;
            }
        }

        public virtual void Rollback()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        {
                            entry.CurrentValues.SetValues(entry.OriginalValues);
                            entry.State = EntityState.Unchanged;
                            break;
                        }

                    case EntityState.Deleted:
                        {
                            entry.State = EntityState.Unchanged;
                            break;
                        }

                    case EntityState.Added:
                        {
                            entry.State = EntityState.Detached;
                            break;
                        }
                }
            }
        }

        public virtual int SqlExecute(string sql, params object[] parameters)
        {
            return this.Database.ExecuteSqlCommand(sql, parameters);
        }

        public virtual IEnumerable<T> SqlQuery<T>(string sql, params object[] parameters)
        {
            return this.Database.SqlQuery<T>(sql, parameters);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Municipality>().HasIndex(p => new { p.Name }).IsUnique();

            // TODO: Not the best one, but will help for performance, no time to search better, with include amount to index could be even better
            modelBuilder.Entity<Tax>().HasIndex(p => new { p.From, p.To, p.MunicipalityId });
        }

        private void MarkPropertiesAsModified<T, O>(DbEntityEntry<T> entry, Expression<Func<T, O>> onlyFields)
            where T : class
            where O : class
        {
            if (onlyFields.NodeType == ExpressionType.Lambda && onlyFields.Body.NodeType == ExpressionType.New)
            {
                foreach (var nexm in ((NewExpression)onlyFields.Body).Members)
                {
                    entry.Property(nexm.Name).IsModified = true;
                }
            }
            else
            {
                throw new Exception("Bad onlyFields expression, must be like: q=> new { q.Id, q.Name} ");
            }
        }
    }
}
