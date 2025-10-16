using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DeAn_qlNhanVien;
using DeAn_qlNhanVien.DataAccess;


namespace DeAn_qlNhanVien
{
    public partial class frmGiaoviec : Form
    {
        private readonly DatabaseConnection dbConnection;
        private readonly TaskDataAccess taskDataAccess;
        public Task AssignedTask { get; private set; }

        public frmGiaoviec()
        {
            InitializeComponent();
            dbConnection = new DatabaseConnection();
            taskDataAccess = new TaskDataAccess();
            LoadInitialData();
        }

        // =================== KHỞI TẠO DỮ LIỆU ===================
        private void LoadInitialData()
        {
            // ComboBox Mức độ ưu tiên
            // Giả định MucDoUuTien là một enum đã được định nghĩa
            cmbMucdo.DataSource = Enum.GetNames(typeof(MucDoUuTien));
            cmbMucdo.SelectedIndex = (int)MucDoUuTien.TrungBinh;

            // === NẠP DANH SÁCH LỊCH LÀM VIỆC (malv - tên ca) ===
            // SỬ DỤNG dbConnection.GetConnection() để lấy đối tượng SqlConnection
            using (SqlConnection conn = dbConnection.GetConnection())
            {
                try
                {
                    // Mở kết nối trước khi dùng SqlDataAdapter
                    conn.Open();

                    // Query: Lấy mã lịch làm việc và tên hiển thị
                    string query = @"SELECT malv, CONCAT(N'Ca ', CAST(malv AS NVARCHAR(10))) AS TenCa 
                             FROM lichlamviec";

                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Xử lý nếu không có dữ liệu
                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("Không tìm thấy ca làm việc nào trong hệ thống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnCapnhat.Enabled = false;
                    }
                    else
                    {
                        btnCapnhat.Enabled = true;
                    }

                    cmbCagiao.DataSource = dt;
                    cmbCagiao.DisplayMember = "TenCa";  // Hiển thị 'Ca X'
                    cmbCagiao.ValueMember = "malv";     // Giá trị thực tế là mã lịch làm việc
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải danh sách ca làm việc: " + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnCapnhat.Enabled = false;
                }
            } // conn.Close() được gọi tự động nhờ khối 'using'

            this.AcceptButton = btnCapnhat;
            this.CancelButton = btnHuy;
        }

        
       
        // =================== PLACEHOLDER ===================
        private void rtbComment_Enter(object sender, EventArgs e)
        {
            if (rtbComment.Text == "Mô tả công việc...")
            {
                rtbComment.Text = "";
                rtbComment.ForeColor = Color.Black;
            }
        }

        private void rtbComment_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(rtbComment.Text))
            {
                rtbComment.Text = "Mô tả công việc...";
                rtbComment.ForeColor = Color.Gray;
            }
        }

        // =================== NÚT HỦY ===================
        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // =================== NÚT CẬP NHẬT ===================
        private void btnCapnhat_Click(object sender, EventArgs e)
        {
            string tieuDe = txtTieude.Text.Trim();
            string moTa = rtbComment.Text == "Mô tả công việc..." ? "" : rtbComment.Text;
            DateTime tgBD = dtpBD.Value;
            DateTime tgKT = dtpKT.Value;
            string mucDoStr = cmbMucdo.SelectedItem?.ToString() ?? "Trung bình";
            int maNguoiDuocGiao = Convert.ToInt32(cmbCagiao.SelectedValue); // ⚠️ Lấy ID, không lấy "Ca 2"
            string tenNguoiDuocGiao = cmbCagiao.Text;

            if (string.IsNullOrWhiteSpace(tieuDe))
            {
                MessageBox.Show("Vui lòng nhập tiêu đề công việc.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (tgKT <= tgBD)
            {
                MessageBox.Show("Thời gian kết thúc phải lớn hơn thời gian bắt đầu.", "Lỗi thời gian", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Parse Enum
            // Chuyển chuỗi sang Enum
            if (!Enum.TryParse(mucDoStr, out MucDoUuTien mucDo))
                mucDo = MucDoUuTien.TrungBinh;

            // Tạo object Task
            Task newTask = new Task
            {
                TieuDe = tieuDe,
                MoTa = moTa,
                ThoiGian_BD = tgBD,
                ThoiGian_KT = tgKT,
                MucDoUuTien = mucDo,
                TrangThai = TrangThai.ChuaLam,
                NguoiDuocGiao = tenNguoiDuocGiao
            };

            try
            {
                // ✅ Lưu vào database qua TaskDataAccess
                taskDataAccess.SaveNewTask(newTask);

                AssignedTask = newTask;
                MessageBox.Show("Thêm công việc mới thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu công việc mới: " + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =================== ĐỊNH DẠNG RICHTEXTBOX ===================
        private void btnB_Click_1(object sender, EventArgs e)
        {
            if (rtbComment.SelectionFont != null)
            {
                var f = rtbComment.SelectionFont;
                rtbComment.SelectionFont = new Font(f, f.Style ^ FontStyle.Bold);
            }
        }

        private void btnI_Click_1(object sender, EventArgs e)
        {
            if (rtbComment.SelectionFont != null)
            {
                var f = rtbComment.SelectionFont;
                rtbComment.SelectionFont = new Font(f, f.Style ^ FontStyle.Italic);
            }
        }

        private void btnU_Click_1(object sender, EventArgs e)
        {
            if (rtbComment.SelectionFont != null)
            {
                var f = rtbComment.SelectionFont;
                rtbComment.SelectionFont = new Font(f, f.Style ^ FontStyle.Underline);
            }
        }
    }
}
