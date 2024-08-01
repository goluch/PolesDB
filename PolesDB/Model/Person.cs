using DataBase.Common;
namespace DataBase.Model
{
    public class Person : BaseEntity<int>
    {
        public string Forename { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public int Earnings { get; set; }
        public Person? Parent { get; set; }
        public Person? Partner { get; set; }
        public ICollection<Person> Children { get; set; }
        public ICollection<Employment> Employments { get; set; }
    }
}
