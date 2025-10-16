using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeAn_qlNhanVien
{
    // Giả định DatabaseConnection nằm trong namespace DeAn_qlNhanVien
    public class DangKiDataAccess
    {
        private readonly DatabaseConnection dbConn;

        public DangKiDataAccess()
        {
            dbConn = new DatabaseConnection();
        }

        public bool DangKi(string tenDangNhap, string tenNguoiDung, string gioiTinh,
                            string matKhau, string email, string soDienThoai, string diaChi)
        {
            // 1. Băm mật khẩu (BẢO MẬT BẮT BUỘC)
            string matKhauHash = PasswordHelper.HashPassword(matKhau);
            int maDangNhapMoi = -1;

            // Giả định mã chức vụ mặc định cho nhân viên mới (Cần đảm bảo giá trị này tồn tại trong bảng chucvu)
            // Ví dụ: 1 là "Nhân viên mới"
            int defaultMaChucVu = 1;

            // Sử dụng SqlTransaction để đảm bảo chèn cả 2 bảng thành công
            using (SqlConnection connection = dbConn.OpenConnection())
            {
                if (connection == null || connection.State != ConnectionState.Open)
                {
                    MessageBox.Show("Không thể mở kết nối đến Cơ sở dữ liệu.", "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    // === BƯỚC 1: INSERT vào bảng dangnhap ===
                    string queryDangNhap = @"
                        INSERT INTO dangnhap (tendangnhap, matkhauHash, email, tennguoidung, sdt, diachi)
                        VALUES (@tenDN, @passHash, @email, @tenND, @sdt, @diachi);
                        SELECT SCOPE_IDENTITY(); -- Lấy madangnhap vừa được tạo
                    ";

                    using (SqlCommand cmdDangNhap = new SqlCommand(queryDangNhap, connection, transaction))
                    {
                        cmdDangNhap.Parameters.AddWithValue("@tenDN", tenDangNhap);
                        cmdDangNhap.Parameters.AddWithValue("@passHash", matKhauHash); // Sử dụng mật khẩu đã HASH
                        cmdDangNhap.Parameters.AddWithValue("@email", email);
                        cmdDangNhap.Parameters.AddWithValue("@tenND", tenNguoiDung);
                        // Xử lý giá trị NULL cho các cột optional
                        cmdDangNhap.Parameters.AddWithValue("@sdt", (object)soDienThoai ?? DBNull.Value);
                        cmdDangNhap.Parameters.AddWithValue("@diachi", (object)diaChi ?? DBNull.Value);

                        object result = cmdDangNhap.ExecuteScalar();
                        maDangNhapMoi = Convert.ToInt32(result);
                    }

                    // === BƯỚC 2: INSERT vào bảng nhanvien ===
                    string queryNhanVien = @"
                        INSERT INTO nhanvien (madangnhap, hoten, gioitinh, diachi, sdt, tenchucvu)
                        VALUES (@maDN, @hoTen, @gioiTinh, @diaChi, @sdt, @maCV);
                    ";

                    using (SqlCommand cmdNhanVien = new SqlCommand(queryNhanVien, connection, transaction))
                    {
                        cmdNhanVien.Parameters.AddWithValue("@maDN", maDangNhapMoi);
                        cmdNhanVien.Parameters.AddWithValue("@hoTen", tenNguoiDung);
                        cmdNhanVien.Parameters.AddWithValue("@gioiTinh", gioiTinh);
                        cmdNhanVien.Parameters.AddWithValue("@diachi", (object)diaChi ?? DBNull.Value);
                        cmdNhanVien.Parameters.AddWithValue("@sdt", (object)soDienThoai ?? DBNull.Value);
                        cmdNhanVien.Parameters.AddWithValue("@maCV", defaultMaChucVu); // Khóa ngoại đến bảng chucvu

                        cmdNhanVien.ExecuteNonQuery();
                    }

                    // Nếu cả hai bước đều thành công, thực hiện Commit
                    transaction.Commit();
                    return true;
                }
                catch (SqlException ex)
                {
                    // Thực hiện Rollback nếu có lỗi xảy ra
                    transaction.Rollback();

                    // Lỗi 2627: Unique constraint violation (Tên đăng nhập hoặc Email đã tồn tại)
                    if (ex.Number == 2627)
                    {
                        MessageBox.Show("Đăng ký không thành công. Tên đăng nhập hoặc Email đã tồn tại.", "Lỗi trùng lặp", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (ex.Number == 547)
                    {
                        // Lỗi Foreign Key (ví dụ: defaultMaChucVu không tồn tại)
                        MessageBox.Show("Lỗi: Mã chức vụ mặc định không tồn tại. Vui lòng kiểm tra bảng chucvu.", "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Lỗi SQL: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Lỗi chung khi đăng ký: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }
    }
}
