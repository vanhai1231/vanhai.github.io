using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ThanhToanMoMo.Controllers
{
    public class MomoPaymentController : Controller
    {
        public ActionResult InitiatePayment(decimal totalAmount)
        {
            // Tạo request và chuyển hướng đến MoMo
            // Đoạn code tạo request và chuyển hướng đã được giải thích ở câu trả lời trước
            // Bạn có thể sao chép và paste đoạn code từ câu trước vào đây
            // Ví dụ:
            string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
            string partnerCode = "MOMOOJOI20210710";
            string accessKey = "iPXneGmrJH0G8FOP";
            string serectkey = "sFcbSGRSJjwGxwhhcEktCHWYUuTuPNDB";
            string orderInfo = "Thanh toán phí mua hàng.";
            string returnUrl = "https://yourdomain.com/Home/ConfirmPaymentClient";
            string notifyUrl = "https://yourdomain.com/Home/SavePayment";

            string amount = totalAmount.ToString(); 
            string orderId = DateTime.Now.Ticks.ToString(); // Mã đơn hàng duy nhất
            string requestId = DateTime.Now.Ticks.ToString();
            string extraData = "";

            // Tạo chữ ký
            string rawSignature = "partnerCode=" + partnerCode + "&accessKey=" + accessKey +
                "&requestId=" + requestId + "&amount=" + amount +
                "&orderId=" + orderId + "&orderInfo=" + orderInfo + "&returnUrl=" + returnUrl +
                "&notifyUrl=" + notifyUrl + "&extraData=" + extraData;
            string signature = CalculateHMACSHA256Hash(rawSignature, serectkey);

            // Tạo URL redirect đến MoMo
            string redirectUrl = endpoint + "?partnerCode=" + partnerCode +
                "&accessKey=" + accessKey + "&requestId=" + requestId +
                "&amount=" + amount + "&orderId=" + orderId +
                "&orderInfo=" + orderInfo + "&returnUrl=" + HttpUtility.UrlEncode(returnUrl) +
                "&notifyUrl=" + HttpUtility.UrlEncode(notifyUrl) +
                "&extraData=" + HttpUtility.UrlEncode(extraData) + "&signature=" + signature;

            // Chuyển hướng đến trang thanh toán MoMo
            return Redirect(redirectUrl);
        }

        private string CalculateHMACSHA256Hash(string input, string key)
        {
            using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
            {
                byte[] hashValue = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Convert.ToBase64String(hashValue);
            }
        }
    }
}
