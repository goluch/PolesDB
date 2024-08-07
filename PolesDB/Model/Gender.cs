using DataBase.Common;
using System.Diagnostics.Metrics;

namespace DataBase.Model
{
    public class Gender : ValueObject
    {
        //public int PersonId { get; private set; } //Requred by EF in query
        // EF Core uses it as a foreign key linking the PersonId value object back to the Person entity
        // https://stackoverflow.com/questions/74287111/no-backing-field-could-be-found-for-property-and-the-property-does-not-have-a-ge

        public string Value { get; private set; }

        public Gender(string value)
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(nameof(value));
            this.Value = value;
        }

        public static Gender Male => new Gender ("Male");
        public static Gender Female => new Gender("Female");

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

        public static implicit operator string(Gender name) => name.Value;
        public static implicit operator Gender(string value) => new(value);
    }
}