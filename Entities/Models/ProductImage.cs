using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class ProductImage : BaseEntity
    {
        public string ImageUrl { get; set; }
        public int? Product_ID { get; set; }
        [ForeignKey("Product_ID")]
        public virtual Product Product { get; set; }
    }
}
