using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ThanhToanMoMo.Others
{
    class MoMoSecurity
    {
        // Khai báo một đối tượng RNGCryptoServiceProvider để tạo số ngẫu nhiên
        private static RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();

        public MoMoSecurity()
        {
            // Constructor mặc định không có logic cụ thể
            // Được sử dụng để khởi tạo đối tượng MoMoSecurity
        }

        // Phương thức để tạo chuỗi hash từ các thông tin thanh toán
        public string getHash(string partnerCode, string merchantRefId,
            string amount, string paymentCode, string storeId, string storeName, string publicKeyXML)
        {
            // Tạo chuỗi JSON từ các thông tin đầu vào
            string json = "{\"partnerCode\":\"" +
                partnerCode + "\",\"partnerRefId\":\"" +
                merchantRefId + "\",\"amount\":" +
                amount + ",\"paymentCode\":\"" +
                paymentCode + "\",\"storeId\":\"" +
                storeId + "\",\"storeName\":\"" +
                storeName + "\"}";

            byte[] data = Encoding.UTF8.GetBytes(json);
            string result = null;

            // Sử dụng RSACryptoServiceProvider để mã hóa chuỗi JSON với khóa công khai MoMo
            using (var rsa = new RSACryptoServiceProvider(4096)) // KeySize
            {
                try
                {
                    // Chuyển đổi khóa công khai từ dạng XML sang dạng đối tượng
                    rsa.FromXmlString(publicKeyXML);

                    // Mã hóa dữ liệu và chuyển đổi kết quả sang dạng base64
                    var encryptedData = rsa.Encrypt(data, false);
                    var base64Encrypted = Convert.ToBase64String(encryptedData);
                    result = base64Encrypted;
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }

            }

            return result;
        }

        // Phương thức để xây dựng chuỗi hash từ thông tin yêu cầu truy vấn (query request)
        public string buildQueryHash(string partnerCode, string merchantRefId,
            string requestid, string publicKey)
        {
            // Tạo chuỗi JSON từ các thông tin đầu vào
            string json = "{\"partnerCode\":\"" +
                partnerCode + "\",\"partnerRefId\":\"" +
                merchantRefId + "\",\"requestId\":\"" +
                requestid + "\"}";

            byte[] data = Encoding.UTF8.GetBytes(json);
            string result = null;

            // Sử dụng RSACryptoServiceProvider để mã hóa chuỗi JSON với khóa công khai MoMo
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                try
                {
                    // Chuyển đổi khóa công khai từ dạng XML sang dạng đối tượng
                    rsa.FromXmlString(publicKey);

                    // Mã hóa dữ liệu và chuyển đổi kết quả sang dạng base64
                    var encryptedData = rsa.Encrypt(data, false);
                    var base64Encrypted = Convert.ToBase64String(encryptedData);
                    result = base64Encrypted;
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }

            return result;
        }

        // Phương thức để xây dựng chuỗi hash từ thông tin yêu cầu hoàn tiền (refund request)
        public string buildRefundHash(string partnerCode, string merchantRefId,
            string momoTranId, long amount, string description, string publicKey)
        {
            // Tạo chuỗi JSON từ các thông tin đầu vào
            string json = "{\"partnerCode\":\"" +
                partnerCode + "\",\"partnerRefId\":\"" +
                merchantRefId + "\",\"momoTransId\":\"" +
                momoTranId + "\",\"amount\":" +
                amount + ",\"description\":\"" +
                description + "\"}";

            byte[] data = Encoding.UTF8.GetBytes(json);
            string result = null;

            // Sử dụng RSACryptoServiceProvider để mã hóa chuỗi JSON với khóa công khai MoMo
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                try
                {
                    // Chuyển đổi khóa công khai từ dạng XML sang dạng đối tượng
                    rsa.FromXmlString(publicKey);

                    // Mã hóa dữ liệu và chuyển đổi kết quả sang dạng base64
                    var encryptedData = rsa.Encrypt(data, false);
                    var base64Encrypted = Convert.ToBase64String(encryptedData);
                    result = base64Encrypted;
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }

            return result;
        }

        // Phương thức để tạo chữ ký SHA256 từ thông điệp và khóa
        public string signSHA256(string message, string key)
        {
            // Chuyển đổi thông điệp và khóa sang dạng byte array
            byte[] keyByte = Encoding.UTF8.GetBytes(key);
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);

            // Sử dụng HMACSHA256 để tạo chữ ký từ thông điệp và khóa
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);

                // Chuyển đổi kết quả sang dạng hex (hexadecimal)
                string hex = BitConverter.ToString(hashmessage);
                hex = hex.Replace("-", "").ToLower();
                return hex;
            }
        }
    }
}
