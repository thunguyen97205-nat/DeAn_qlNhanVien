using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeAn_qlNhanVien
{
    internal class DatabaseConnection
    {
        private readonly string con;

        // Constructor - bạn truyền chuỗi kết nối vào hoặc gán cố định
        public DatabaseConnection()
        {
            con = "Data Source=LAPTOP-J4N69Q1T\\ANHTHU;Initial Catalog=ql_nhanvien;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
        }

        // Hàm trả về đối tượng SqlConnection đã mở
        public SqlConnection GetConnection()
        {
            SqlConnection conn = new SqlConnection(con);
            return conn;
        }

        // Hàm mở kết nối
        public SqlConnection OpenConnection()
        {
            SqlConnection conn = new SqlConnection(con);
            if (conn.State == System.Data.ConnectionState.Closed)
                conn.Open();
            return conn;
        }

        // Hàm đóng kết nối
        public void CloseConnection(SqlConnection conn)
        {
            if (conn.State == System.Data.ConnectionState.Open)
                conn.Close();
        }
    }
}
