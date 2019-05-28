﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EcomShoes_Webshop.Models
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq; 
    public partial class Product
    {
        public Product()
        {
            this.OrderDetails = new HashSet<OrderDetail>();
        }


        public int ID { get; set; }
        [RegularExpression(@"^[A-Z]+[0-9]*$",ErrorMessage = "ProductCode phải bắt đầu với 2 kí tự đầu là chữ và in hoa và có độ dài từ 3 - 15 kí tự")]
        [Required]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "ProductCode phải bắt đầu với 2 kí tự đầu là chữ và in hoa và có độ dài từ 3 - 15 kí tự")]
        public string ProductCode { get; set; }
        [StringLength(50, MinimumLength = 3, ErrorMessage = "ProductName chỉ chứa được từ 3 - 50 kí tự")]
        [Required]
        public string ProductName { get; set; }
        public decimal SalePrice { get; set; }
        public decimal OriginalPrice { get; set; }
        public string ImageURL { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public int Quantity { get; set; }
        public int CategoryID { get; set; }
        public string Size { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}