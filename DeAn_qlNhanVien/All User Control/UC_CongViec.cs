using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeAn_qlNhanVien.All_User_Control;
using DeAn_qlNhanVien.DataAccess;
using DeAn_qlNhanVien;// TaskDataAccess
using System.Windows.Forms;

namespace DeAn_qlNhanVien.All_User_Control
{
    // Giả định tên các FlowLayoutPanel: flpViecmoi, flpDanglam, flpHoanthanh
    public partial class UC_CongViec : UserControl
    {
        private readonly TaskDataAccess dataAccess = new TaskDataAccess();
        private List<Task> allTasks = new List<Task>();

        public UC_CongViec()
        {
            InitializeComponent();
        }

        // === 1. LOAD DỮ LIỆU KHI USER CONTROL MỞ ===
        // Lưu ý: Đảm bảo bạn đã kết nối sự kiện Load này trong Designer
        private void UC_CongViec_Load(object sender, EventArgs e)
        {
            LoadTasksToFlowPanels();
        }

        private void LoadTasksToFlowPanels()
        {
            // Xóa các thẻ cũ trước khi nạp lại
            flpViecmoi.Controls.Clear();
            flpDanglam.Controls.Clear();
            flpHoanthanh.Controls.Clear();

            try
            {
                // Gọi DataAccess để lấy tất cả công việc
                allTasks = dataAccess.GetAllTasks();

                // Phân loại và thêm vào các FlowLayoutPanel
                foreach (Task task in allTasks)
                {
                    UC_TaskCard taskCard = new UC_TaskCard();
                    taskCard.LoadTaskData(task);

                    // ĐĂNG KÝ SỰ KIỆN DI CHUYỂN THẺ
                    taskCard.TaskStatusChanged += TaskCard_TaskStatusChanged;

                    switch (task.TrangThai)
                    {
                        case TrangThai.ChuaLam:
                            flpViecmoi.Controls.Add(taskCard);
                            break;
                        case TrangThai.DangLam: // Sửa lỗi chính tả nếu trong Enum là DangLam
                            flpDanglam.Controls.Add(taskCard);
                            break;
                        case TrangThai.HoanThanh:
                            flpHoanthanh.Controls.Add(taskCard);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu công việc: " + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // === 2. XỬ LÝ SỰ KIỆN KHI TRẠNG THÁI THẺ THAY ĐỔI (Di chuyển thẻ) ===
        private void TaskCard_TaskStatusChanged(object sender, TaskEventArgs e)
        {
            UC_TaskCard card = sender as UC_TaskCard;
            if (card == null) return;

            // 1. Loại bỏ thẻ khỏi FlowLayoutPanel cũ
            card.Parent.Controls.Remove(card);

            // 2. Thêm thẻ vào FlowLayoutPanel mới tương ứng
            switch (e.UpdatedTask.TrangThai)
            {
                case TrangThai.ChuaLam:
                    flpViecmoi.Controls.Add(card);
                    break;
                case TrangThai.DangLam: // Sửa lỗi chính tả nếu trong Enum là DangLam
                    flpDanglam.Controls.Add(card);
                    break;
                case TrangThai.HoanThanh:
                    flpHoanthanh.Controls.Add(card);
                    break;
            }
        }

        // === 3. XỬ LÝ NÚT GIAO VIỆC MỚI ===
        private void btnGiaoviec_Click(object sender, EventArgs e)
        {
            // Giả định tên nút là btnGiaoviec
            // Giả định form Giao Việc là frmGiaoviec và có thuộc tính AssignedTask sau khi DialogResult.OK
            using (frmGiaoviec giaoviecForm = new frmGiaoviec())
            {
                if (giaoviecForm.ShowDialog() == DialogResult.OK)
                {
                    Task newTask = giaoviecForm.AssignedTask;
                    if (newTask != null)
                    {
                        try
                        {
                            // 1. LƯU VÀO CSDL (DataAccess sẽ trả về MaCV mới trong newTask)
                            dataAccess.SaveNewTask(newTask);

                            // 2. TẠO VÀ THÊM THẺ MỚI VÀO cột "Việc mới" (Giả định trạng thái ban đầu là ChuaLam)
                            UC_TaskCard newUC_TaskCard = new UC_TaskCard();
                            newUC_TaskCard.LoadTaskData(newTask);
                            newUC_TaskCard.TaskStatusChanged += TaskCard_TaskStatusChanged;

                            flpViecmoi.Controls.Add(newUC_TaskCard);

                            MessageBox.Show("Giao việc thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lỗi khi lưu công việc mới: " + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }

        }

    }
}