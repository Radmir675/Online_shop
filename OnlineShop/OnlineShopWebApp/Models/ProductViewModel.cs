using Microsoft.AspNetCore.Http;
using OnlineShop.db;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineShopWebApp
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Please enter the product name")]
        [StringLength(70, MinimumLength = 3, ErrorMessage = "The name must contain from 3 to 70 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter the product cost")]
        [Range(1, 50000, ErrorMessage = "This parametr has to be more than zero and less than 50000")]
        public decimal Cost { get; set; }

        [Required(ErrorMessage = "Please enter short product description")]
        [StringLength(300, MinimumLength = 1, ErrorMessage = "The description must contain from 1 to 300 characters")]
        public string ShortDescription { get; set; }
        public List<Image> Images { get; set; }
        public IFormFile UploadedFile { get; set; }
    }
}