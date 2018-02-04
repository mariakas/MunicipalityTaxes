using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MunicipalityTaxes.Core
{
    public abstract class BaseEntity : IEquatable<BaseEntity>
    {
        public BaseEntity() { }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(BaseEntity)) return false;
            return this.Equals((BaseEntity)obj);
        }

        public bool Equals(BaseEntity other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (other.GetType() != GetType()) return false;
            return other.Id.Equals(Id);
        }

        public override int GetHashCode()
        {
            if (Equals(Id, default(int)))
            {
                return base.GetHashCode();
            }
            return Id.GetHashCode();
        }

        public static bool operator ==(BaseEntity x, BaseEntity y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(BaseEntity x, BaseEntity y)
        {
            return !(x == y);
        }
    }
}
