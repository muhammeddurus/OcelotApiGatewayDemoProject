using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class OrderDetail : BaseEntity
    {
        public int? Product_ID { get; set; }
        [ForeignKey("Product_ID")]
        public virtual Product Product { get; set; }
        public int? Order_ID { get; set; }
        [ForeignKey("Order_ID")]
        public virtual Order Order { get; set; }
        public int Quantity { get; set; }
    }
}
