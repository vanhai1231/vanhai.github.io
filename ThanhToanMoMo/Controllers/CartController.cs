using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThanhToanMoMo.Models;

namespace ThanhToanMoMo.Controllers
{
    public class CartController : Controller
    {
        MyDataDataContext Data = new MyDataDataContext();
        public ActionResult Index()
        {
            // Đặt mã giỏ hàng mặc định là 0
            int cartId = 0;

            // Lấy danh sách các sản phẩm trong giỏ hàng
            var cartItems = Data.ChiTietGioHangs.Where(c => c.MaGioHang == cartId).ToList();

            // Tạo một danh sách để chứa thông tin chi tiết của các sản phẩm trong giỏ hàng
            var cartDetails = new List<CartItemDetail>();

            foreach (var item in cartItems)
            {
                // Lấy thông tin sản phẩm
                var product = Data.SanPhams.FirstOrDefault(p => p.id == item.id);

                // Lấy thông tin giỏ hàng
                var cart = Data.GioHangs.FirstOrDefault(g => g.MaGioHang == item.MaGioHang);

                if (product != null && cart != null)
                {
                    // Thêm thông tin chi tiết vào danh sách
                    cartDetails.Add(new CartItemDetail
                    {
                        id = item.id,
                        MaGioHang = item.MaGioHang,
                        TenSanPham = product.Tensp,
                        Gia = (decimal)product.Gia,
                        HinhAnh = product.HinhAnh,
                        SoLuong = cart.SoLuong,
                        ThanhTien = (decimal)cart.ThanhTien
                    });
                }
            }

            // Trả về View với danh sách chi tiết giỏ hàng
            return View(cartDetails);
        }

        private static int currentCartId = 0;

        public ActionResult AddToCart(int id)
        {
            // Kiểm tra xem sản phẩm đã có trong giỏ hàng chưa
            var existingCartItem = Data.ChiTietGioHangs.FirstOrDefault(c => c.id == id);

            // Lấy thông tin sản phẩm
            var product = Data.SanPhams.FirstOrDefault(p => p.id == id);

            if (existingCartItem != null && product != null)
            {
                // Nếu sản phẩm đã có trong giỏ hàng, cập nhật số lượng và thành tiền
                var cart = Data.GioHangs.FirstOrDefault(g => g.MaGioHang == existingCartItem.MaGioHang);
                if (cart != null)
                {
                    cart.SoLuong += 1;
                    cart.ThanhTien += product.Gia;
                }
            }
            else if (product != null)
            {
                int maxCartId = 0;

                if (Data.GioHangs.Any())
                {
                    maxCartId = Data.GioHangs.Max(g => g.MaGioHang);
                }

                var cartItem = new ChiTietGioHang
                {
                    id = id,
                    MaGioHang = maxCartId + 1,
                };

                // Tạo một giỏ hàng mới
                var cart = new GioHang
                {
                    MaGioHang = cartItem.MaGioHang,
                    SoLuong = 1,
                    ThanhTien = product.Gia
                };
                Data.GioHangs.InsertOnSubmit(cart);
                Data.ChiTietGioHangs.InsertOnSubmit(cartItem);
            }

            Data.SubmitChanges();
            return RedirectToAction("Index", "Cart");
        }


        public ActionResult RemoveFromCart(int id)
        {
            var cartItem = Data.ChiTietGioHangs.FirstOrDefault(c => c.id == id);

            if (cartItem != null)
            {
                var cart = Data.GioHangs.FirstOrDefault(g => g.MaGioHang == cartItem.MaGioHang);
                if (cart != null)
                {
                    if (cart.SoLuong > 1)
                    {
                        cart.SoLuong -= 1;
                    }
                    else
                    {
                        Data.ChiTietGioHangs.DeleteOnSubmit(cartItem);
                        Data.GioHangs.DeleteOnSubmit(cart);
                    }

                    Data.SubmitChanges();
                }
            }
            return RedirectToAction("Index", "Cart");
        }

    }
}
