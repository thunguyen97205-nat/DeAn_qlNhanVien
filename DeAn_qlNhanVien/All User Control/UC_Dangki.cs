using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeAn_qlNhanVien.All_User_Control
{
    public partial class UC_Dangki : UserControl
    {
        // Khởi tạo DataAccess để sử dụng logic CSDL
        private DangKiDataAccess dataAccess = new DangKiDataAccess();

        public UC_Dangki()
        {
            InitializeComponent();
        }

        private void btnHuyDK_Click(object sender, EventArgs e)
        {
            // Sử dụng .Clear() thay vì gán " " để xóa sạch nội dung TextBox
            txtTenDN.Clear();
            txtUserName.Clear();
            txtSdt.Clear();
            txtPass.Clear();
            txtPassagain.Clear();
            txtEmail.Clear(); // Thêm Clear cho Email
            txtDiaChi.Clear();

            // Đặt lại Radio Button
            rdNam.Checked = false;
            rdNu.Checked = false;

            // Đặt lại ảnh giới tính (nếu có)
            if (ptcGioitinh != null)
            {
                ptcGioitinh.Image = null;
            }

            txtTenDN.Focus(); // Focus vào ô Tên đăng nhập
        }

        private void rdNu_CheckedChanged(object sender, EventArgs e)
        {
            if (rdNu.Checked)
            {
                // Giả định: ptcGioitinh là tên PictureBox của bạn, Properties.Resources.begai là ảnh bạn đã thêm
                ptcGioitinh.Image = Properties.Resources.begai;
            }
        }

        private void rdNam_CheckedChanged(object sender, EventArgs e)
        {
            if (rdNam.Checked)
            {
                // Giả định: ptcGioitinh là tên PictureBox của bạn, Properties.Resources.betrai là ảnh bạn đã thêm
                ptcGioitinh.Image = Properties.Resources.betrai;
            }
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            // --- 1. KIỂM TRA DỮ LIỆU BẮT BUỘC ---
            if (string.IsNullOrWhiteSpace(txtTenDN.Text) ||
                string.IsNullOrWhiteSpace(txtUserName.Text) ||
                string.IsNullOrWhiteSpace(txtPass.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text)) // Email là trường NOT NULL trong DB
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Tên đăng nhập, Tên người dùng, Mật khẩu và Email.", "Lỗi Thiếu Thông Tin", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // --- 2. KIỂM TRA MẬT KHẨU KHỚP ---
            if (txtPass.Text != txtPassagain.Text)
            {
                MessageBox.Show("Mật khẩu nhập lại không khớp.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassagain.Clear();
                txtPassagain.Focus();
                return;
            }

            // --- 3. XỬ LÝ GIỚI TÍNH ---
            string gioiTinh = "";
            if (rdNam.Checked)
            {
                gioiTinh = "Nam";
            }
            else if (rdNu.Checked)
            {
                gioiTinh = "Nữ";
            }
            else
            {
                MessageBox.Show("Vui lòng chọn Giới tính.", "Lỗi Thiếu Thông Tin", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // --- 4. GÁN GIÁ TRỊ VÀ GỌI DATAACCESS ---
            string tenDangNhap = txtTenDN.Text.Trim();
            string tenNguoiDung = txtUserName.Text.Trim();
            string matKhau = txtPass.Text;
            string email = txtEmail.Text.Trim();

            // Xử lý các trường cho phép NULL (sdt, diachi) bằng cách gán null nếu ô nhập trống
            string soDienThoai = string.IsNullOrWhiteSpace(txtSdt.Text) ? null : txtSdt.Text.Trim();
            string diaChi = string.IsNullOrWhiteSpace(txtDiaChi.Text) ? null : txtDiaChi.Text.Trim();

            // Gọi hàm Đăng kí (Hàm này đã xử lý Transaction và Hashing trong DangKiDataAccess)
            bool success = dataAccess.DangKi(tenDangNhap, tenNguoiDung, gioiTinh, matKhau, email, soDienThoai, diaChi);

            // --- 5. XỬ LÝ KẾT QUẢ ---
            if (success)
            {
                MessageBox.Show("Đăng ký tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Xóa dữ liệu trên form
                btnHuyDK_Click(sender, e);

                // Thay vì đóng ParentForm, chúng ta có thể chuyển hướng về Form Đăng nhập
                // (Giả định ParentForm là Main Form chứa cả Login và Đăng ký)
                // Hoặc bạn có thể gọi một sự kiện/interface để Form cha xử lý việc chuyển đổi User Control.

                // Ví dụ: Sau khi đăng ký thành công, tự động đóng form Đăng ký (nếu nó là một Form riêng)
                // this.ParentForm.Close(); 
            }
            else
            {
                // Thông báo lỗi cụ thể (như lỗi trùng lặp) đã được xử lý bên trong DangKiDataAccess
            }
        }
    }
}
