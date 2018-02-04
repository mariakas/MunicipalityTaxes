using System.Collections.Generic;
using System.Text;

namespace MunicipalityTaxes.Core
{
    public class ServiceResult
    {
        private IDictionary<string, IList<string>> errors;

        public ServiceResult()
        {
            this.errors = new Dictionary<string, IList<string>>();
        }

        public ServiceResult(IEnumerable<KeyValuePair<string, string>> errors)
            : this()
        {
            foreach (var item in errors)
            {
                this.AddError(item.Key, item.Value);
            }
        }

        public ServiceResult(ServiceResult other)
            : this()
        {
            this.errors = new Dictionary<string, IList<string>>();
            foreach (var i in other)
            {
                this.errors.Add(i);
            }
        }

        public int Id { get; set; }

        public virtual bool Success
        {
            get { return this.errors.Count == 0; }
        }

        public virtual IEnumerable<KeyValuePair<string, IList<string>>> Errors
        {
            get
            {
                foreach (var item in this.errors)
                {
                    yield return item;
                }
            }
        }

        public virtual ServiceResult AddError(string error)
        {
            return this.AddError(string.Empty, error);
        }

        public virtual ServiceResult AddError(string key, string error)
        {
            if (!this.errors.ContainsKey(key))
            {
                this.errors[key] = new List<string>();
            }

            ((IList<string>)this.errors[key]).Add(error);
            return this;
        }

        public virtual ServiceResult AddErrorFormat(string error, params object[] args)
        {
            return this.AddErrorFormat(string.Empty, error, args);
        }

        public virtual ServiceResult AddErrorFormat(string key, string error, params object[] args)
        {
            if (args == null || args.Length == 0)
            {
                return this.AddError(key, error);
            }

            return this.AddError(key, string.Format(error, args));
        }

        public virtual void AddErrors(ServiceResult other)
        {
            foreach (var i in other)
            {
                foreach (var errorValue in i.Value)
                {
                    this.AddError(i.Key, errorValue);
                }
            }
        }

        public virtual IEnumerator<KeyValuePair<string, IList<string>>> GetEnumerator()
        {
            return this.errors.GetEnumerator();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in this.errors)
            {
                foreach (var subitem in item.Value)
                {
                    sb.AppendLine(subitem);
                }
            }

            return sb.ToString();
        }
    }
}
