using Domain.Common;

namespace DataBase.Model
{
    public class Employment : BaseEntity<int>
    {
        Company Company {  get; set; }
        Person Emploee {  get; set; }
        Contract Contract { get; set; }
    }
}
