using DataBase.Common;

namespace Domain.Common
{
    public class Contract : ValueObject
    {
        public string Value { get; private set; }
        public Contract(string value)
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(nameof(value));
            this.Value = value;
        }

        public static string EmploymentContract => "Employment Contract";
        public static string MandateContract => "Mandate Contract";

        public static IEnumerable<string> SupportedGenders
        {
            get
            {
                yield return EmploymentContract;
                yield return MandateContract;
            }
        }

        public string V { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return EmploymentContract;
            yield return MandateContract;
        }
    }
}
