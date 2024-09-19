using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Supplier : BaseEntity
    {
        public string CompanyName { get; set; }
        public string ImageUrl { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
