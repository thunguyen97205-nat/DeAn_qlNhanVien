using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeAn_qlNhanVien.All_User_Control;

namespace DeAn_qlNhanVien
{
    // Lớp này dùng để truyền dữ liệu Task khi trạng thái thay đổi
    public class TaskEventArgs : EventArgs
    {
        public Task UpdatedTask { get; }
        public TaskEventArgs(Task updatedTask)
        {
            UpdatedTask = updatedTask;
        }
        // Thêm thuộc tính này để biết thẻ nào cần di chuyển
        public UC_TaskCard TaskCardControl { get; }
        public TaskEventArgs(Task updatedTask, UC_TaskCard taskCard)
        {
            UpdatedTask = updatedTask;
            TaskCardControl = taskCard;
        }
    }
}
