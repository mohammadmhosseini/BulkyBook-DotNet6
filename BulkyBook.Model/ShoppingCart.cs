using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Model
{
    public class ShoppingCart
    {
        public Product Product { get; set; }
        [Range(1, 1000, ErrorMessage = "لطفا مقداری بین 1 تا 1000 وارد کنید")]
        public int Count { get; set; }
    }
}
