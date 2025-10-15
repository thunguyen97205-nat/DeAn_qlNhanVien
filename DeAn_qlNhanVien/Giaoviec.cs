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


namespace DeAn_qlNhanVien
{
    public partial class frmGiaoviec : Form
    {
        public Task AssignedTask { get; private set; }

        public frmGiaoviec()
        {
            InitializeComponent();
            LoadInitialData();
        }

        // =================== KHỞI TẠO DỮ LIỆU ===================
        private void LoadInitialData()
        {
            // ComboBox Mức độ ưu tiên
            cmbMucdo.DataSource = Enum.GetNames(typeof(MucDoUuTien));
            cmbMucdo.SelectedIndex = (int)MucDoUuTien.TrungBinh;

            // === NẠP DANH SÁCH LỊCH LÀM VIỆC (malv - tên ca) ===
            using (SqlConnection conn = new SqlConnection("Data Source=LAPTOP-J4N69Q1T\\ANHTHU;Initial Catalog=ql_nhanvien;Integrated Security=True;Encrypt=True;TrustServerCertificate=True"))
            {
                string query = "SELECT malv, CONCAT(N'Ca ', CAST(malv AS NVARCHAR(10))) AS TenCa FROM lichlamviec";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cmbCagiao.DataSource = dt;
                cmbCagiao.DisplayMember = "TenCa";
                cmbCagiao.ValueMember = "malv";
            }

            this.AcceptButton = btnCapnhat;
            this.CancelButton = btnHuy;
        }

        // =================== ĐỊNH DẠNG RICHTEXTBOX ===================
        private void btnB_Click(object sender, EventArgs e)
        {
            if (rtbComment.SelectionFont != null)
            {
                var f = rtbComment.SelectionFont;
                rtbComment.SelectionFont = new Font(f, f.Style ^ FontStyle.Bold);
            }
        }

        private void btnI_Click(object sender, EventArgs e)
        {
            if (rtbComment.SelectionFont != null)
            {
                var f = rtbComment.SelectionFont;
                rtbComment.SelectionFont = new Font(f, f.Style ^ FontStyle.Italic);
            }
        }

        private void btnU_Click(object sender, EventArgs e)
        {
            if (rtbComment.SelectionFont != null)
            {
                var f = rtbComment.SelectionFont;
                rtbComment.SelectionFont = new Font(f, f.Style ^ FontStyle.Underline);
            }
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
            MucDoUuTien mucDo;
            if (!Enum.TryParse(mucDoStr, out mucDo))
                mucDo = MucDoUuTien.TrungBinh;

            // Lưu vào database
            try
            {
                using (SqlConnection conn = new SqlConnection("Data Source=LAPTOP-J4N69Q1T\\ANHTHU;Initial Catalog=ql_thoigian;Integrated Security=True"))
                {
                    conn.Open();
                    string query = @"INSERT INTO CongViec (TieuDe, MoTa, ThoiGian_BD, ThoiGian_KT, MucDoUuTien, TrangThai, NguoiDuocGiao)
                                     VALUES (@tieude, @mota, @bd, @kt, @mucdo, @trangthai, @nguoiduocgiao)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@tieude", tieuDe);
                        cmd.Parameters.AddWithValue("@mota", moTa);
                        cmd.Parameters.AddWithValue("@bd", tgBD);
                        cmd.Parameters.AddWithValue("@kt", tgKT);
                        cmd.Parameters.AddWithValue("@mucdo", mucDo.ToString());
                        cmd.Parameters.AddWithValue("@trangthai", TrangThai.ChuaLam.ToString());
                        cmd.Parameters.AddWithValue("@nguoiduocgiao", tenNguoiDuocGiao); // 👈 dùng tên hoặc mã, tùy DB

                        cmd.ExecuteNonQuery();
                    }
                }

                // Tạo object Task để trả về form chính
                AssignedTask = new Task
                {
                    TieuDe = tieuDe,
                    MoTa = moTa,
                    NguoiDuocGiao = tenNguoiDuocGiao,
                    ThoiGian_BD = tgBD,
                    ThoiGian_KT = tgKT,
                    MucDoUuTien = mucDo,
                    TrangThai = TrangThai.ChuaLam
                };

                MessageBox.Show("Thêm công việc mới thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu công việc mới: " + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
