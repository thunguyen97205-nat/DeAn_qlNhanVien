using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeAn_qlNhanVien
{
    public class PassDataAccess
    {
        public bool DatlaiMK(string tenDangNhap, string matKhauMoi)
        {
            string conn = "Data Source=LAPTOP-J4N69Q1T\\ANHTHU;Initial Catalog=ql_thoigian;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";

            // Câu lệnh SQL để cập nhật mật khẩu
            // Sử dụng tham số (@param) để ngăn chặn tấn công SQL Injection
            string query = "UPDATE nhanvien SET matkhau = @matkhau WHERE tendangnhap = @tendangnhap";
            using (SqlConnection connection = new SqlConnection(conn))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@matkhau", matKhauMoi);
                command.Parameters.AddWithValue("@tendangnhap", tenDangNhap);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi cập nhật mật khẩu: " + ex.Message);
                    return false;
                }
            }
        }
    }
}
