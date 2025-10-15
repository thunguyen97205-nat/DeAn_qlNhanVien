using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using DeAn_qlNhanVien; // Để truy cập Task, MucDoUuTien, TrangThai
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeAn_qlNhanVien.DataAccess
{
    public class TaskDataAccess
    {
        // 🔗 Chuỗi kết nối CSDL
        private readonly string connectionString = "Data Source=LAPTOP-J4N69Q1T\\ANHTHU;Initial Catalog=ql_nhanvien;Integrated Security=True;TrustServerCertificate=True";

        // === 1️⃣ LẤY TẤT CẢ CÔNG VIỆC TỪ CSDL ===
        public List<Task> GetAllTasks()
        {
            List<Task> allTasks = new List<Task>();

            // ⚠️ Tên cột phải khớp chính xác với trong SQL Server
            string query = @"SELECT macv, tieude, mota, thoigian_bd, thoigian_kt, nguoigiao, mucdo_uutien, trangthai 
                             FROM congviec";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Task task = new Task
                            {
                                MaCV = reader.GetInt32(reader.GetOrdinal("macv")),
                                TieuDe = reader.GetString(reader.GetOrdinal("tieude")),
                                MoTa = reader.IsDBNull(reader.GetOrdinal("mota"))
                                        ? string.Empty
                                        : reader.GetString(reader.GetOrdinal("mota")),
                                ThoiGian_BD = reader.GetDateTime(reader.GetOrdinal("thoigian_bd")),
                                ThoiGian_KT = reader.GetDateTime(reader.GetOrdinal("thoigian_kt")),
                                NguoiDuocGiao = reader.GetString(reader.GetOrdinal("nguoigiao"))
                            };

                            // Chuyển chuỗi trong DB thành Enum (có xử lý lỗi)
                            string mucDoStr = reader["mucdo_uutien"].ToString();
                            string trangThaiStr = reader["trangthai"].ToString();

                            if (Enum.TryParse(mucDoStr, true, out MucDoUuTien mucDo))
                                task.MucDoUuTien = mucDo;
                            else
                                task.MucDoUuTien = MucDoUuTien.TrungBinh;

                            if (Enum.TryParse(trangThaiStr, true, out TrangThai trangThai))
                                task.TrangThai = trangThai;
                            else
                                task.TrangThai = TrangThai.ChuaLam;

                            allTasks.Add(task);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi truy vấn dữ liệu công việc: " + ex.Message, ex);
                }
            }

            return allTasks;
        }

        // === 2️⃣ THÊM CÔNG VIỆC MỚI ===
        public void SaveNewTask(Task task)
        {
            // ⚠️ Không cần cột macv trong INSERT nếu là IDENTITY
            string query = @"
                INSERT INTO congviec (tieude, mota, thoigian_bd, thoigian_kt, nguoigiao, mucdo_uutien, trangthai) 
                OUTPUT INSERTED.macv
                VALUES (@tieuDe, @moTa, @tgBD, @tgKT, @nguoiGiao, @mucDo, @trangThai);";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@tieuDe", task.TieuDe);
                cmd.Parameters.AddWithValue("@moTa", (object)task.MoTa ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@tgBD", task.ThoiGian_BD);
                cmd.Parameters.AddWithValue("@tgKT", task.ThoiGian_KT);
                cmd.Parameters.AddWithValue("@nguoiGiao", task.NguoiDuocGiao);
                cmd.Parameters.AddWithValue("@mucDo", task.MucDoUuTien.ToString());
                cmd.Parameters.AddWithValue("@trangThai", task.TrangThai.ToString());

                try
                {
                    conn.Open();
                    object newId = cmd.ExecuteScalar();

                    if (newId != null && newId != DBNull.Value)
                        task.MaCV = Convert.ToInt32(newId);
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi lưu công việc mới: " + ex.Message, ex);
                }
            }
        }

        // === 3️⃣ CẬP NHẬT TRẠNG THÁI CÔNG VIỆC ===
        public void UpdateTaskStatus(int maCV, TrangThai newStatus)
        {
            string query = "UPDATE congviec SET trangthai = @trangthai WHERE macv = @macv";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@trangthai", newStatus.ToString());
                command.Parameters.AddWithValue("@macv", maCV);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi cập nhật trạng thái công việc: " + ex.Message, ex);
                }
            }
        }
    }
}
