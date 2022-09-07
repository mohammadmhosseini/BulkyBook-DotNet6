using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Model
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public string ISBN { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        [Range(1, 500000)]
        [Display(Name = "لیست قیمت")]
        public double ListPrice { get; set; }

        [Required]
        [Range(1, 500000)]
        [Display(Name = "قیمت برای 1 تا 50 عدد")]
        public double Price { get; set; }

        [Required]
        [Range(1, 500000)]
        [Display(Name = "قیمت برای 51 تا 100 عدد")]
        public double Price50 { get; set; }

        [Required]
        [Range(1, 500000)]
        [Display(Name = "قیمت برای بیشتر از 100 عدد")]
        public double Price100 { get; set; }

        [ValidateNever]
        public string ImageUrl { get; set; }

        [Required]
        [Display(Name = "دسته بندی")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")] // no need
        [ValidateNever]
        public Category Category { get; set; }

        [Required]
        [Display(Name = "نوع پوشش")]
        public int CoverTypeId { get; set; }
        [ValidateNever]
        public CoverType CoverType { get; set; }
    }
}
