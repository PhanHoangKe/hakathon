using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace hakathon.Utilities
{
    public class Functions
    {
        public static int _UserID = 0;
        public static string _UserName = string.Empty;
        public static string _Email = string.Empty;
        public static string _Password = string.Empty;
        //public static string _RoleId = string.Empty;
        public static string _FullName = string.Empty;
        public static string _Message = string.Empty;

        //biến tĩnh dùng chung cho tất cả đối tượng
        public static string MD5Hash(string text)//Mã hóa một chuỗi đầu vào (text) bằng thuật toán MD5.
        {
            MD5 md5 = new MD5CryptoServiceProvider();//Tạo một đối tượng MD5 để thực hiện băm 
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));//Chuyển đổi chuỗi đầu vào thành mảng byte bằng cách sử dụng mã hóa ASCII và sau đó thực hiện băm. Kết quả băm sẽ được lưu trong thuộc tính md5.Hash.
            byte[] result = md5.Hash;
            StringBuilder strBuilder = new StringBuilder();//Tạo một StringBuilder để xây dựng chuỗi kết quả.
            for (int i = 0; i < result.Length; i++)
                strBuilder.Append(result[i].ToString("x2"));//Vòng lặp for duyệt qua từng byte trong mảng result, chuyển đổi mỗi byte thành chuỗi hex (định dạng "x2" để đảm bảo mỗi byte được biểu diễn bằng hai ký tự) và nối vào StringBuilder.
            return strBuilder.ToString();//trả về chuỗi hex đã được mã hóa.
        }

        public static string MD5Password(string? text)
        {
            string str = MD5Hash(text);//Mã hóa chuỗi đầu tiên
            for (int i = 0; i <= 5; i++)//Lặp thêm 5 lần mã hóa
                str = MD5Hash(str + str);//mỗi lần lặp thì nhân đôi
            return str;
        }
                 public static bool IsLogin()
        {
            if ((Functions._UserID <= 0) || string.IsNullOrEmpty(Functions._UserName) || string.IsNullOrEmpty(Functions._Email))
                return false;
            return true;
        }
       
    }
}