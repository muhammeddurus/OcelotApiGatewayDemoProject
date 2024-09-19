using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Quantity { get; set; }
        public string Price { get; set; }
        public int Stock { get; set; }
        public string ImageUrlBig { get; set; }
        public string ImageUrlSmall { get; set; }
        public bool Status { get; set; }
        public int? Supplier_ID { get; set; }
        [ForeignKey("Supplier_ID")]
        public virtual Supplier? Supplier { get; set; }
        public int? Category_ID { get; set; }
        [ForeignKey("Category_ID")]
        public virtual Category? Category { get; set; }
        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
        public virtual ICollection<ProductImage>? ProductImages { get; set; }
    }
}
