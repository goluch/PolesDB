using DataBase.Common;

namespace DataBase.Model
{
    public class Company : BaseEntity<int>
    {
        public string Name { get; set; }
        public Person Boss { get; set; }
        public IList<Employment> Employments { get; set; }
    }
}
