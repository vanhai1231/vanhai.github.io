using System;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Net;

namespace MoMo
{
    class PaymentRequest
    {
        public PaymentRequest()
        {
            
        }
        // Phương thức gửi yêu cầu thanh toán đến endpoint và nhận kết quả trả về
        public static string sendPaymentRequest(string endpoint, string postJsonString)
        {
            try
            {
                // Tạo đối tượng HttpWebRequest để gửi yêu cầu HTTP POST
                HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(endpoint);

                // Dữ liệu JSON cần gửi
                var postData = postJsonString;

                // Chuyển đổi dữ liệu thành dạng byte array với mã hóa UTF-8
                var data = Encoding.UTF8.GetBytes(postData);

                // Thiết lập các thông số cho yêu cầu HTTP
                httpWReq.ProtocolVersion = HttpVersion.Version11;
                httpWReq.Method = "POST";
                httpWReq.ContentType = "application/json";
                httpWReq.ContentLength = data.Length;
                httpWReq.ReadWriteTimeout = 30000;
                httpWReq.Timeout = 15000;

                // Ghi dữ liệu vào luồng của yêu cầu
                Stream stream = httpWReq.GetRequestStream();
                stream.Write(data, 0, data.Length);
                stream.Close();

                // Nhận phản hồi từ máy chủ
                HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();

                // Đọc phản hồi từ máy chủ và chuyển đổi thành chuỗi JSON
                string jsonresponse = "";
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    string temp = null;
                    while ((temp = reader.ReadLine()) != null)
                    {
                        jsonresponse += temp;
                    }
                }

                // Trả về phản hồi dưới dạng chuỗi JSON
                return jsonresponse;
            }
            catch (WebException e)
            {
                return e.Message;
            }
        }
    }
}
