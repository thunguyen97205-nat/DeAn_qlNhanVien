using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeAn_qlNhanVien
{
    public class DangKiDataAccess
    {
        public bool Dangki(string tenDangNhap, string tenNguoiDung, string gioiTinh,
                                 string matKhau, string email, string soDienThoai, string diaChi)
        {
            string conn = "Data Source=LAPTOP-J4N69Q1T\\ANHTHU;Initial Catalog=ql_nhanvien;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";

            // Câu lệnh SQL để chèn dữ liệu vào bảng NhanVien
            // Sử dụng các tham số (@param) để ngăn chặn tấn công SQL Injection
            string query = "INSERT INTO nhanvien (tendangnhap, hoten, gioitinh, matkhau, email, sodienthoai, diachi) " +
                           "VALUES (@tendangnhap, @hoten, @gioitinh, @matkhau, @email, @sodienthoai, @diachi)";
            using (SqlConnection connection = new SqlConnection(conn))
            {
                SqlCommand command = new SqlCommand(query, connection);

                // Thêm các tham số
                command.Parameters.AddWithValue("@tendangnhap", tenDangNhap);
                command.Parameters.AddWithValue("@hoten", tenNguoiDung);
                command.Parameters.AddWithValue("@gioitinh", gioiTinh);
                command.Parameters.AddWithValue("@matkhau", matKhau); // Lưu ý: Cần mã hóa mật khẩu trong thực tế
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@sodienthoai", soDienThoai);
                command.Parameters.AddWithValue("@diachi", diaChi);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery(); // Trả về số dòng bị ảnh hưởng
                    return rowsAffected > 0; // Trả về true nếu chèn thành công
                }
                catch (SqlException ex)
                {
                    // Xử lý lỗi, ví dụ tên đăng nhập đã tồn tại
                    // Lỗi 2627 là vi phạm khóa chính
                    if (ex.Number == 2627)
                    {
                        // Có thể trả về false hoặc xử lý riêng
                    }
                    return false;
                }
            }
        }
    }
}
