using Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dto
{
    public class ProductImageDto
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public int? Product_ID { get; set; }
    }
}
