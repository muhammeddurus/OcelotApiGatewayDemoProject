using Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dto
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Quantity { get; set; }
        public string Price { get; set; }
        public int Stock { get; set; }
        public string ImageUrlBig { get; set; }
        public string ImageUrlSmall { get; set; }
        public bool Status { get; set; }
        public string SupplierName { get; set; }
        public string SupplierImageUrl { get; set; }
        public string CategoryName { get; set; }
        public int? Category_ID { get; set; }
        public int? Supplier_ID { get; set; }
        public List<string> ProductImages { get; set; }


    }
}
