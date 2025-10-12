using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeAn_qlNhanVien.All_User_Control
{
    public partial class UC_TaskCard : UserControl
    {
        public int MaCV { get; set; }
        public UC_TaskCard()
        {
            InitializeComponent();
        }

        private void UC_TaskCard_Load(object sender, EventArgs e)
        {
            
        }

        public void LoadTaskData (Task task)
        {
            this.MaCV = task.MaCV;
            txtTencv.Text = task.TieuDe;
            txtNguoiht.Text = task.NguoiDuocGiao;
            txtMucdo.Text = task.MucDoUuTien.ToString();
            TrangThai trangThai = (TrangThai)cmbTrangThai.SelectedItem;
        }

        public void CapNhatTrangThai(int maCV, TrangThai trangThaiMoi)
        {
            // Viết code kết nối CSDL và thực thi câu lệnh SQL UPDATE
            // Ví dụ:
            string conn = "Data Source=LAPTOP-J4N69Q1T\\ANHTHU;Initial Catalog=ql_thoigian;Integrated Security=True;Encrypt=True";
            string query = "UPDATE congviec SET trangthai = @trangthai WHERE macv = @macv";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@trangthai", trangThaiMoi.ToString());
                command.Parameters.AddWithValue("@macv", maCV);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi cập nhật CSDL: " + ex.Message);
                }
            }
        }
        private void cmbTrangThai_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
