using Domain.Common;
using System.Reflection;

namespace DataBase.Model
{
    public class Person : BaseEntity<int>
    {
        string Forename { get; set; }
        string Surname { get; set; }
        string BirthDate { get; set; }
        Gender Gender { get; set; }
        int Earnings { get; set; }
        Person? Mother { get; set; }
        Person? Father { get; set; }
        Person? Partner { get; set; }
    }
}
