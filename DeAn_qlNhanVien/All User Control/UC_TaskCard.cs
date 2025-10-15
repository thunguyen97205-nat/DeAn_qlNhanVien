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
using DeAn_qlNhanVien.DataAccess;

namespace DeAn_qlNhanVien.All_User_Control
{
    public partial class UC_TaskCard : UserControl
    {
        // Khởi tạo DataAccess để sử dụng cho việc cập nhật CSDL
        private readonly TaskDataAccess dataAccess = new TaskDataAccess();

        public Task CurrentTask { get; private set; }

        // Khai báo sự kiện public để UC_CongViec lắng nghe
        public event EventHandler<TaskEventArgs> TaskStatusChanged;

        // Giả định tên các controls: txtTencv, txtNguoiht, txtMucdo, cmbTrangThai, btnHoanthanh, btnDanglam

        public UC_TaskCard()
        {
            InitializeComponent();
            if (cmbTrangThai != null)
            {
                // Gán dữ liệu Enum cho ComboBox TrangThai
                cmbTrangThai.DataSource = Enum.GetValues(typeof(TrangThai));
            }
        }

        // === PHƯƠNG THỨC ĐỔ DỮ LIỆU ===
        public void LoadTaskData(Task task)
        {
            CurrentTask = task;

            // Đổ dữ liệu vào các controls
            txtTencv.Text = task.TieuDe;
            txtNguoiht.Text = task.NguoiDuocGiao;
            txtMucdo.Text = task.MucDoUuTien.ToString();

            // Gán giá trị Enum hiện tại vào ComboBox
            cmbTrangThai.SelectedItem = task.TrangThai;

            // Cài đặt hiển thị nút: chỉ hiện nút chuyển tiếp khả thi
            btnHoanthanh.Visible = (task.TrangThai != TrangThai.HoanThanh);
            btnDanglam.Visible = (task.TrangThai == TrangThai.ChuaLam);
        }

        // === HÀM XỬ LÝ CẬP NHẬT CHUNG (Sử dụng TaskDataAccess) ===
        private void HandleStatusUpdate(TrangThai trangThaiMoi)
        {
            if (CurrentTask == null) return;

            try
            {
                // 1. Cập nhật CSDL
                dataAccess.UpdateTaskStatus(CurrentTask.MaCV, trangThaiMoi);

                // 2. Cập nhật đối tượng Task và UI
                CurrentTask.TrangThai = trangThaiMoi;
                cmbTrangThai.SelectedItem = trangThaiMoi;

                // Điều chỉnh hiển thị nút sau khi cập nhật
                btnHoanthanh.Visible = (trangThaiMoi != TrangThai.HoanThanh);
                btnDanglam.Visible = (trangThaiMoi == TrangThai.ChuaLam);

                // 3. Phát sự kiện lên Form chính để di chuyển thẻ
                // Dùng 'this' để truyền chính control này
                TaskStatusChanged?.Invoke(this, new TaskEventArgs(CurrentTask));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật trạng thái: " + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // === SỰ KIỆN THAY ĐỔI TRẠNG THÁI TRÊN ComboBox ===
        private void cmbTrangThai_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CurrentTask != null && cmbTrangThai.SelectedItem != null)
            {
                TrangThai trangThaiMoi = (TrangThai)cmbTrangThai.SelectedItem;

                // Nếu trạng thái thay đổi
                if (CurrentTask.TrangThai != trangThaiMoi)
                {
                    HandleStatusUpdate(trangThaiMoi);
                }
            }
        }

        // === SỰ KIỆN NÚT HOÀN THÀNH ===
        private void btnHoanthanh_Click(object sender, EventArgs e)
        {
            HandleStatusUpdate(TrangThai.HoanThanh);
        }

        // === SỰ KIỆN NÚT ĐANG LÀM ===
        private void btnDanglam_Click(object sender, EventArgs e)
        {
            HandleStatusUpdate(TrangThai.DangLam);
        }
    }
}