namespace DeAn_qlNhanVien.All_User_Control
{
    partial class UC_TaskCard
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtNguoiht = new System.Windows.Forms.TextBox();
            this.txtMucdo = new System.Windows.Forms.TextBox();
            this.txtTencv = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lbTenCV = new System.Windows.Forms.Label();
            this.cmbTrangThai = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // txtNguoiht
            // 
            this.txtNguoiht.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNguoiht.Location = new System.Drawing.Point(204, 147);
            this.txtNguoiht.Name = "txtNguoiht";
            this.txtNguoiht.Size = new System.Drawing.Size(244, 22);
            this.txtNguoiht.TabIndex = 19;
            // 
            // txtMucdo
            // 
            this.txtMucdo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMucdo.Location = new System.Drawing.Point(204, 108);
            this.txtMucdo.Name = "txtMucdo";
            this.txtMucdo.Size = new System.Drawing.Size(244, 22);
            this.txtMucdo.TabIndex = 17;
            // 
            // txtTencv
            // 
            this.txtTencv.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTencv.Location = new System.Drawing.Point(204, 32);
            this.txtTencv.Name = "txtTencv";
            this.txtTencv.Size = new System.Drawing.Size(244, 22);
            this.txtTencv.TabIndex = 16;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(46, 71);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 22);
            this.label5.TabIndex = 15;
            this.label5.Text = "Trạng thái:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(46, 145);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(155, 22);
            this.label4.TabIndex = 14;
            this.label4.Text = "Người hoàn thành:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(45, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(137, 22);
            this.label3.TabIndex = 13;
            this.label3.Text = "Mức độ ưu tiên:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(46, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 22);
            this.label1.TabIndex = 12;
            // 
            // lbTenCV
            // 
            this.lbTenCV.AutoSize = true;
            this.lbTenCV.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTenCV.Location = new System.Drawing.Point(46, 31);
            this.lbTenCV.Name = "lbTenCV";
            this.lbTenCV.Size = new System.Drawing.Size(127, 22);
            this.lbTenCV.TabIndex = 11;
            this.lbTenCV.Text = "Tên công việc:";
            // 
            // cmbTrangThai
            // 
            this.cmbTrangThai.FormattingEnabled = true;
            this.cmbTrangThai.Items.AddRange(new object[] {
            "Chưa làm",
            "Đang làm",
            "Hoàn thành"});
            this.cmbTrangThai.Location = new System.Drawing.Point(204, 69);
            this.cmbTrangThai.Name = "cmbTrangThai";
            this.cmbTrangThai.Size = new System.Drawing.Size(243, 24);
            this.cmbTrangThai.TabIndex = 20;
            this.cmbTrangThai.SelectedIndexChanged += new System.EventHandler(this.cmbTrangThai_SelectedIndexChanged);
            // 
            // UC_TaskCard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmbTrangThai);
            this.Controls.Add(this.txtNguoiht);
            this.Controls.Add(this.txtMucdo);
            this.Controls.Add(this.txtTencv);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbTenCV);
            this.Name = "UC_TaskCard";
            this.Size = new System.Drawing.Size(493, 200);
            this.Load += new System.EventHandler(this.UC_TaskCard_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtNguoiht;
        private System.Windows.Forms.TextBox txtMucdo;
        private System.Windows.Forms.TextBox txtTencv;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbTenCV;
        private System.Windows.Forms.ComboBox cmbTrangThai;
    }
}
