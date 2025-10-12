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

namespace DeAn_qlNhanVien
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
        
        }

        private void lkQuenmk_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmQuenMK quenMKForm = new frmQuenMK();
            quenMKForm.Show();
            this.Hide(); //giup an form dang nhap
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            if (txtUserName.Text == "AnhThu" && txtPassWord.Text == "972005")
            {
                Dashboard ds = new Dashboard();
                this.Hide();
                ds.Show();
            }
            else
            {
                MessageBox.Show("Tên dăng nhập hoặc mật khẩu sai, vui lòng nhập lại!");
                txtPassWord.Clear();
            }
        }
    }
}
