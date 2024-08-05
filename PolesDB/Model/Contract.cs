using DataBase.Common;

namespace Domain.Common
{
    public class Contract : ValueObject
    {
        public string ContractType { get; private set; }
        public Contract() { }
        public Contract(string value)
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(nameof(value));
            this.ContractType = value;
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

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return EmploymentContract;
            yield return MandateContract;
        }

        public static implicit operator string(Contract name) => name.ContractType;
        public static implicit operator Contract(string value) => new(value);
    }
}
