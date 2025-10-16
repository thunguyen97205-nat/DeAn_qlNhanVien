using System;
using System.Collections.Generic;
using System.Data; // Cần cho ConnectionState
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeAn_qlNhanVien
{
    public class PassDataAccess
    {
        // 1. Khởi tạo đối tượng DatabaseConnection (giả định class này tồn tại)
        private DatabaseConnection dbConnection = new DatabaseConnection();

        /// <summary>
        /// Đặt lại mật khẩu cho người dùng bằng cách băm mật khẩu mới và cập nhật vào CSDL.
        /// </summary>
        /// <param name="tenDangNhap">Tên đăng nhập của người dùng.</param>
        /// <param name="matKhauMoi">Mật khẩu mới (dạng văn bản thuần túy).</param>
        /// <returns>True nếu cập nhật thành công, False nếu thất bại.</returns>
        public bool DatlaiMK(string tenDangNhap, string matKhauMoi)
        {
            // 2. Băm mật khẩu mới trước khi lưu (sử dụng PasswordHelper đã sửa lỗi)
            string matKhauHashMoi = PasswordHelper.HashPassword(matKhauMoi);

            // 3. Cập nhật bảng dangnhap, cột matkhauHash
            // Thêm cập nhật ngaysua để trigger hoạt động (nếu có)
            string query = "UPDATE dangnhap SET matkhauHash = @matkhauHash, ngaysua = GETDATE() WHERE tendangnhap = @tendangnhap";

            // Sử dụng khối using với OpenConnection để quản lý kết nối hiệu quả
            using (SqlConnection connection = dbConnection.OpenConnection())
            {
                if (connection == null || connection.State != ConnectionState.Open)
                {
                    MessageBox.Show("Không thể mở kết nối đến Cơ sở dữ liệu.", "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // 4. Thêm tham số đã băm và tham số tên đăng nhập
                    command.Parameters.AddWithValue("@matkhauHash", matKhauHashMoi);
                    command.Parameters.AddWithValue("@tendangnhap", tenDangNhap);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        // Kiểm tra xem có bản ghi nào bị ảnh hưởng không
                        if (rowsAffected > 0)
                        {
                            return true;
                        }
                        else
                        {
                            // Tên đăng nhập không tồn tại
                            MessageBox.Show("Tên đăng nhập không tồn tại trong hệ thống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi cập nhật mật khẩu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            } // Kết nối tự động đóng
        }
    }
}
