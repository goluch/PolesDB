using DataBase.Common;

namespace Domain.Common
{
    public class Contract : ValueObject
    {
        public Contract(string value)
        {
            this.Value = value;
        }

        public string Value { get; set; }

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
