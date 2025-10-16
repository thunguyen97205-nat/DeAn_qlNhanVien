using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DeAn_qlNhanVien.All_User_Control;

namespace DeAn_qlNhanVien
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        public void ShowUserControl(UserControl uc)
        {
            pnlMain.Controls.Clear();  // Xóa UC cũ (nếu có)
            uc.Dock = DockStyle.Fill;          // Cho UC full khung
            pnlMain.Controls.Add(uc);   // Thêm UC mới vào panel
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void btnChamcong_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void uC_Dangki1_Load(object sender, EventArgs e)
        {

        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void guna2Button8_Click_1(object sender, EventArgs e)
        {
            
        }

        private void guna2Button6_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnDangky_Click(object sender, EventArgs e)
        {
            pnlMain.Controls.Clear();
            UC_Dangki uc = new UC_Dangki();
            uc.Dock = DockStyle.Fill;
            pnlMain.Controls.Add(uc);
        }

        private void btnCongviec_Click(object sender, EventArgs e)
        {
            pnlMain.Controls.Clear();
            UC_CongViec uc = new UC_CongViec();
            uc.Dock = DockStyle.Fill;
            pnlMain.Controls.Add(uc);
        }
    }
}
