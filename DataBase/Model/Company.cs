using Domain.Common;

namespace DataBase.Model
{
    public class Company : BaseEntity<int>
    {
        string Name { get; set; }
        Person Boss { get; set; }
    }
}
