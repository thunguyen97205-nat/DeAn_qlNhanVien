using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeAn_qlNhanVien
{
    public partial class frmGiaoviec : Form
    {
        public Task AssignedTask { get; private set; }
        public frmGiaoviec()
        {
            InitializeComponent();
        }

        private void btnB_Click(object sender, EventArgs e)
        {
            if (rtbComment.SelectionFont != null)
            {
                Font currentFont = rtbComment.SelectionFont;
                FontStyle newStyle = currentFont.Style ^ FontStyle.Bold; // toggle
                rtbComment.SelectionFont = new Font(currentFont, newStyle);
            }
        }

        private void btnI_Click(object sender, EventArgs e)
        {
            if (rtbComment.SelectionFont != null)
            {
                Font currentFont = rtbComment.SelectionFont;
                FontStyle newStyle = currentFont.Style ^ FontStyle.Italic;
                rtbComment.SelectionFont = new Font(currentFont, newStyle);
            }
        }

        private void btnU_Click(object sender, EventArgs e)
        {
            if (rtbComment.SelectionFont != null)
            {
                Font currentFont = rtbComment.SelectionFont;
                FontStyle newStyle = currentFont.Style ^ FontStyle.Underline;
                rtbComment.SelectionFont = new Font(currentFont, newStyle);
            }
        }

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

        private void btnCapnhat_Click(object sender, EventArgs e)
        {
            String tieuDe = txtTieude.Text;
            String caGiao = cmbCagiao.SelectedItem?.ToString();
            DateTime thoigianBD = dtpBD.Value;
            DateTime thoigianKT = dtpKT.Value;
            MucDoUuTien mucDo = (MucDoUuTien)cmbMucdo.SelectedItem;
            // Tạo một đối tượng Task mới
            AssignedTask = new Task
            {
                TieuDe = tieuDe,
                NguoiDuocGiao = caGiao,
                ThoiGian_BD = thoigianBD,
                ThoiGian_KT = thoigianKT, 
                MucDoUuTien = mucDo
           };

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
