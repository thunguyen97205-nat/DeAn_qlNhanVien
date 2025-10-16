using System;
using System.Security.Cryptography; // Cần thiết cho SHA256
using System.Text; // Cần thiết cho Encoding

namespace DeAn_qlNhanVien
{
    /// <summary>
    /// Cung cấp các phương thức tĩnh để băm (hash) và xác minh (verify) mật khẩu 
    /// sử dụng thuật toán SHA256, phù hợp với cột matkhauHash trong bảng dangnhap.
    /// </summary>
    public static class PasswordHelper
    {
        // Lưu ý: Đối với ứng dụng thực tế, nên dùng BCrypt hoặc Argon2 thay vì SHA256 để tăng bảo mật.

        /// <summary>
        /// Băm một chuỗi mật khẩu thành chuỗi hex SHA256.
        /// Được dùng khi TẠO TÀI KHOẢN hoặc ĐẶT LẠI MẬT KHẨU.
        /// </summary>
        /// <param name="password">Mật khẩu cần băm (plain text).</param>
        /// <returns>Chuỗi mật khẩu đã băm (hash).</returns>
        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return string.Empty;
            }

            using (SHA256 sha256 = SHA256.Create())
            {
                // Chuyển mật khẩu thành mảng byte UTF8
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Chuyển mảng byte đã băm thành chuỗi hex
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2")); // Định dạng "x2" cho chuỗi hex
                }
                return builder.ToString();
            }
        }

        /// <summary>
        /// Xác minh mật khẩu người dùng nhập vào so với mật khẩu đã băm lưu trong DB.
        /// Được dùng trong chức năng ĐĂNG NHẬP.
        /// </summary>
        /// <param name="password">Mật khẩu người dùng nhập vào (plain text).</param>
        /// <param name="hashedPassword">Mật khẩu đã băm được lấy từ cơ sở dữ liệu.</param>
        /// <returns>True nếu mật khẩu khớp, False nếu không khớp.</returns>
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            if (string.IsNullOrEmpty(hashedPassword))
            {
                // Không có hash trong DB (tài khoản không tồn tại), coi như không khớp.
                return false;
            }

            // Băm mật khẩu người dùng nhập
            string hashOfInput = HashPassword(password);

            // So sánh chuỗi hash. StringComparison.OrdinalIgnoreCase đảm bảo so sánh chuỗi hex chính xác.
            return hashOfInput.Equals(hashedPassword, StringComparison.OrdinalIgnoreCase);
        }
    }
}