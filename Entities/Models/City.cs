using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class City : BaseEntity
    {
        public string Name { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
    }
}
