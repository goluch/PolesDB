using DataBase.Common;
using Domain.Common;

namespace DataBase.Model
{
    public class Employment : BaseEntity<int>
    {
        public Company Company { get; set; }
        public Person Emploee {  get; set; }
        public Contract Contract { get; set; }
    }
}
