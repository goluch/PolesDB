using DataBase.Common;
using System.ComponentModel.DataAnnotations.Schema;
namespace DataBase.Model
{
    public class Person : BaseEntity<int>
    {
        public string Forename { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public int Earnings { get; set; }
        public Person? Mother { get; set; }
        public Person? Father { get; set; }
        public Person? Partner { get; set; }
        public ICollection<Person> Doughters { get; set; } = [];
        public ICollection<Person> Sons { get; set; } = [];
        public IEnumerable<Person> Children {
            get {
                return Doughters.Concat(Sons);
            }
            set { } }
        public ICollection<Employment> Employments { get; set; } = [];
    }
}
