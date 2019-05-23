using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EcomShoes_Webshop.Models;

namespace EcomShoes_Webshop.Models
{
    public class Cart
    {
        K23T3aEntities db = new K23T3aEntities();
        public int iMaSP { get; set; }
        public int itype { get; set; }
        public string iTenSP { get; set; }
        public string iImage { get; set; }
        public double idonGia { get; set; }
        public int isoLuong { get; set; }
        public int iTonKho { get; set; }
        public double thanhTien
        {
            get { return isoLuong * idonGia; }

        }
         // Ham tạo cho giỏ hàng
        public Cart(int MaSP)
        {
            iMaSP = MaSP;
            Product shose = db.Products.Single(n => n.ID == iMaSP);
            iTenSP = shose.ProductName;
            idonGia = double.Parse(shose.SalePrice.ToString());
            isoLuong = 1;
        }
    }
}