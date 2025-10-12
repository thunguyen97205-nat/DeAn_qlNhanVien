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
    public partial class UC_CongViec : UserControl
    {
        public UC_CongViec()
        {
            InitializeComponent();
        }

        private void flpDanglam_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnGiaoviec_Click(object sender, EventArgs e)
        {
            frmGiaoviec giaoviecForm = new frmGiaoviec();
            giaoviecForm.Show();
            if(giaoviecForm.ShowDialog() == DialogResult.OK)
            {
                // Lấy đối tượng Task đã được tạo từ Form giao việc
                Task newTask = giaoviecForm.AssignedTask;
                if(newTask != null)
                {
                    // Tạo một TaskCard mới
                    UC_TaskCard newUC_TaskCard = new UC_TaskCard();
                    // Đổ dữ liệu từ Task vào thẻ
                    newUC_TaskCard.LoadTaskData(newTask);
                    // Thêm thẻ vào FlowLayoutPanel của cột "Việc mới"
                    flpViecmoi.Controls.Add(newUC_TaskCard);

                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            LoadAndDisplayTasks();
        }
        private void LoadAndDisplayTasks()
        {
            // 1. Xóa tất cả các thẻ hiện có trên Form
            flpViecmoi.Controls.Clear();
            flpDanglam.Controls.Clear();
            flpHoanthanh.Controls.Clear();

            // 2. Lấy dữ liệu công việc mới nhất từ CSDL
            TaskDataAccess dataAccess = new TaskDataAccess();
            List<Task> allTasks = dataAccess.GetAllTasksFromDatabase(); // Viết phương thức này để truy vấn CSDL

            // 3. Tạo lại các thẻ và thêm vào các FlowLayoutPanel tương ứng
            foreach (var task in allTasks)
            {
                UC_TaskCard card = new UC_TaskCard();
                card.LoadTaskData(task);

                if (task.TrangThai == TrangThai.HoanThanh)
                {
                    flpHoanthanh.Controls.Add(card);
                }
                else if (task.TrangThai == TrangThai.DangLam)
                {
                    flpDanglam.Controls.Add(card);
                }
                else
                {
                    flpViecmoi.Controls.Add(card);
                }
            }
        }
    }
}
