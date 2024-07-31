using Domain.Common;
using System.IO;

namespace DataBase.Model
{
    public class Gender : BaseValueObject<int>
    {
        public string Value { get; set; }

        public static string Male => "Male";
        public static string Female => "Female";

        public static IEnumerable<string> SupportedGenders
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