using DataBase.Common;
using System.Diagnostics.Metrics;

namespace DataBase.Model
{
    public class Gender : ValueObject
    {
        public string Value { get; private set; }

        public Gender(string value)
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(nameof(value));
            this.Value = value;
        }

        public static Gender Male => new Gender ("Male");
        public static Gender Female => new Gender("Male");

        public static IEnumerable<Gender> SupportedGenders
        {
            get
            {
                yield return Male;
                yield return Female;
            }
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}