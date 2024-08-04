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

        public static Contract EmploymentContract => new Contract("Employment Contract") ;
        public static Contract MandateContract => new Contract("Mandate Contract");

        public static IEnumerable<Contract> SupportedContracts
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
