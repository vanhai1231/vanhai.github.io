using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThanhToanMoMo.Models;

namespace ThanhToanMoMo.Controllers
{
    public class SanPhamController : Controller
    {
        MyDataDataContext Data = new MyDataDataContext();

        // GET: SanPham
        public ActionResult Index()
        {
            var all_Sach = from tt in Data.SanPhams select tt;
            return View(all_Sach);
        }

        // GET: Cart
        public ActionResult Cart()
        {
            var gioHangItems = Data.GioHangs.ToList();
            return View(gioHangItems);
        }
        public ActionResult Detail(int id)
        {
            var sanPham = Data.SanPhams.FirstOrDefault(sp => sp.id == id);
            if (sanPham == null)
            {
                // Sản phẩm không tồn tại, trả về một trang lỗi hoặc chuyển hướng đến một trang khác
                return HttpNotFound();
            }
            return View(sanPham);
        }
    }
}
