using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeAn_qlNhanVien
{
    public class TaskDataAccess
    {
        public List<Task> GetAllTasksFromDatabase()
        {
            List<Task> allTasks = new List<Task>();

            // Chuỗi kết nối đến cơ sở dữ liệu của bạn
            // Thay thế "Your_Connection_String" bằng chuỗi kết nối thực tế
            string conn = "Data Source=LAPTOP-J4N69Q1T\\ANHTHU;Initial Catalog=ql_thoigian;Integrated Security=True";
            string query = "Data Source=LAPTOP-J4N69Q1T\\ANHTHU;Initial Catalog=ql_thoigian;Integrated Security=True"; 
            using (SqlConnection connection = new SqlConnection(conn))
            {
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        // Tạo một đối tượng Task mới cho mỗi dòng dữ liệu
                        Task task = new Task();

                        // Gán dữ liệu từ CSDL vào các thuộc tính của đối tượng Task
                        task.MaCV = reader.GetInt32(reader.GetOrdinal("macv"));
                        task.TieuDe = reader.GetString(reader.GetOrdinal("tieude"));
                        task.MoTa = reader.IsDBNull(reader.GetOrdinal("mota")) ? null : reader.GetString(reader.GetOrdinal("mota"));
                        task.ThoiGian_BD = reader.GetDateTime(reader.GetOrdinal("thoigian_bd"));
                        task.ThoiGian_KT = reader.GetDateTime(reader.GetOrdinal("thoigian_kt"));
                        task.NguoiDuocGiao = reader.GetString(reader.GetOrdinal("NguoiDuocGiao"));

                        // Chuyển đổi chuỗi từ CSDL thành kiểu Enum
                        task.MucDoUuTien = (MucDoUuTien)Enum.Parse(typeof(MucDoUuTien), reader.GetString(reader.GetOrdinal("mucdo_uutien")), true);
                        task.TrangThai = (TrangThai)Enum.Parse(typeof(TrangThai), reader.GetString(reader.GetOrdinal("trangthai")), true);

                        // Thêm đối tượng Task vào danh sách
                        allTasks.Add(task);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi truy vấn dữ liệu: " + ex.Message);
                }
            }

            return allTasks;
        }
    }
}
