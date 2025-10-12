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
        public UC_Dangki()
        {
            InitializeComponent();
        }

        private void btnHuyDK_Click(object sender, EventArgs e)
        {
            txtTenDN.Text = " ";
            txtUserName.Text = " ";
            txtSdt.Text = " ";
            txtPass.Text = " ";
            txtSdt.Text = " ";
            txtDiaChi.Text = " ";
            txtPassagain.Text = " ";
            rdNam.Checked = false;
            rdNu.Checked = false;

            txtUserName.Focus();
        }

        private void rdNu_CheckedChanged(object sender, EventArgs e)
        {
            if (rdNu.Checked)
            {
                ptcGioitinh.Image = Properties.Resources.begai;
            }
        }

        private void rdNam_CheckedChanged(object sender, EventArgs e)
        {
            if (rdNam.Checked)
            {
                ptcGioitinh.Image = Properties.Resources.betrai;
            }
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenDN.Text) || string.IsNullOrWhiteSpace(txtPass.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Tên đăng nhập và Mật khẩu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (txtPass.Text != txtPassagain.Text)
            {
                MessageBox.Show("Mật khẩu nhập lại không khớp.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string tenDangNhap = txtTenDN.Text;
            string tenNguoiDung = txtUserName.Text;
            string matKhau = txtPass.Text;
            string email = txtEmail.Text;
            string soDienThoai = txtSdt.Text;
            string diaChi = txtDiaChi.Text;
            string gioiTinh = "";

            DangKiDataAccess dataAccess = new DangKiDataAccess();
            bool success = dataAccess.Dangki(tenDangNhap, tenNguoiDung, gioiTinh, matKhau, email, soDienThoai, diaChi);
            if (success)
            {
                MessageBox.Show("Đăng ký thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.ParentForm.Close(); // Đóng form sau khi đăng ký thành công
            }
            else
            {
                MessageBox.Show("Đăng ký không thành công. Tên đăng nhập có thể đã tồn tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
