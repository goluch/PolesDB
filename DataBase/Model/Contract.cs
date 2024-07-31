namespace Domain.Common
{
    public class Contract : ValueObject
    {
        public string Value { get; set; }

        public static string Employment => "Employment Contract";
        public static string Mandate => "Mandate Contract";

        public static IEnumerable<string> SupportedGenders
        {
            get
            {
                yield return Employment;
                yield return Mandate;
            }
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Employment;
            yield return Mandate;
        }
    }
}
