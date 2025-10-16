using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
// Đảm bảo namespace DeAn_qlNhanVien được include nếu cần
// using DeAn_qlNhanVien; 

namespace DeAn_qlNhanVien.All_User_Control
{
    public partial class UC_Lichlam : UserControl
    {
        public UC_Lichlam()
        {
            InitializeComponent();
        }

        private void UC_Lichlam_Load(object sender, EventArgs e)
        {
            // 1. Cài đặt tên hiển thị cho các Tab (Giữ nguyên)
            if (tabControllichca.TabPages.Count >= 5)
            {
                tabControllichca.TabPages[0].Text = "THỨ HAI";
                tabControllichca.TabPages[1].Text = "THỨ BA";
                tabControllichca.TabPages[2].Text = "THỨ TƯ";
                tabControllichca.TabPages[3].Text = "THỨ NĂM";
                tabControllichca.TabPages[4].Text = "THỨ SÁU";
            }

            // 2. Thiết lập cấu trúc cột và style (Giữ nguyên)
            SetupDataGridView(dgvthu2);
            SetupDataGridView(dgvthu3);
            SetupDataGridView(dgvthu4);
            SetupDataGridView(dgvthu5);
            SetupDataGridView(dgvthu6);

            // 3. Nạp dữ liệu mẫu CA SẴN cho TẤT CẢ các ngày (Dùng hàm mới)
            LoadShiftData_Sample(dgvthu2, "Thứ Hai");
            LoadShiftData_Sample(dgvthu3, "Thứ Ba");
            LoadShiftData_Sample(dgvthu4, "Thứ Tư");
            LoadShiftData_Sample(dgvthu5, "Thứ Năm");
            LoadShiftData_Sample(dgvthu6, "Thứ Sáu");

            // 4. Bắt sự kiện chuyển Tab và CellValueChanged (Điều chỉnh CellValueChanged)
            tabControllichca.SelectedIndexChanged += tabControllichca_SelectedIndexChanged;
            dgvthu2.CellValueChanged += Dgv_CellValueChanged_Grouped;
            dgvthu3.CellValueChanged += Dgv_CellValueChanged_Grouped;
            dgvthu4.CellValueChanged += Dgv_CellValueChanged_Grouped;
            dgvthu5.CellValueChanged += Dgv_CellValueChanged_Grouped;
            dgvthu6.CellValueChanged += Dgv_CellValueChanged_Grouped;


            // Cập nhật tổng ca lần đầu
            UpdateShiftTotals();
        }

        // =========================================================
        // PHƯƠNG THỨC HỖ TRỢ: Lấy Dữ liệu Phân Ca Mẫu
        // =========================================================
        private List<(string Ten, string VaiTro, int StartHour, int EndHour)> GetSampleShiftAssignments(string dayOfWeek)
        {
            var assignments = new List<(string, string, int, int)>();

            if (dayOfWeek == "Thứ Hai")
            {
                // Ca 1 (7:00-11:00)
                assignments.Add(("", "", 7, 11));
                assignments.Add(("", "", 7, 11));
            }
            else if (dayOfWeek == "Thứ Ba")
            {
                // Ca 1 (7:00-11:00)
                assignments.Add(("", "", 7, 11));
                assignments.Add(("", "", 7, 11));
            }
            else if (dayOfWeek == "Thứ Tư")
            {
                // Ca 1 (7:00-11:00)
                assignments.Add(("", "", 7, 11));
                assignments.Add(("", "", 7, 11));
            }
            else if (dayOfWeek == "Thứ Năm")
            {
                // Ca 2 (11:00-15:00)
                assignments.Add(("", "", 11, 15));
                assignments.Add(("", "", 11, 15));
            }
            else if (dayOfWeek == "Thứ Sáu")
            {
                // Ca Full (7:00-15:00)
                assignments.Add(("", "", 7, 15));
                // Ca 1 (7:00-11:00)
                assignments.Add(("", "", 7, 11));
                // Một nhân viên khác làm 11:00-15:00
                assignments.Add(("", "", 11, 15));
            }

            return assignments;
        }


        // =========================================================
        // PHƯƠNG THỨC 1: NẠP DỮ LIỆU PHÂN CA MẪU (ĐÃ TÍCH HỢP GỘP ẢO)
        // =========================================================
        private void LoadShiftData_Sample(DataGridView dgv, string dayOfWeek)
        {
            dgv.Rows.Clear();
            var assignments = GetSampleShiftAssignments(dayOfWeek);

            foreach (var item in assignments)
            {
                int rowIndex = dgv.Rows.Add();
                dgv.Rows[rowIndex].Cells["Ten"].Value = item.Ten;

                // Duyệt qua các cột giờ từ Giờ Bắt Đầu đến Giờ Kết Thúc
                for (int hour = item.StartHour; hour < item.EndHour; hour++)
                {
                    string columnName = $"Ca{hour}h";
                    if (dgv.Columns.Contains(columnName))
                    {
                        dgv.Rows[rowIndex].Cells[columnName].Value = item.VaiTro;

                        // Áp dụng Style Gộp
                        StyleShiftCells(dgv, rowIndex, columnName, item.VaiTro, hour == item.StartHour);
                    }
                }
                CalculateTotalHours(dgv, rowIndex);
            }
        }

        // =========================================================
        // PHƯƠNG THỨC HỖ TRỢ: Áp dụng style cho ô ca làm việc
        // =========================================================
        private void StyleShiftCells(DataGridView dgv, int rowIndex, string columnName, string role, bool isStartCell)
        {
            DataGridViewCell cell = dgv.Rows[rowIndex].Cells[columnName];

            // Nếu ô không rỗng (đã được phân ca)
            if (!string.IsNullOrEmpty(role))
            {
                cell.ReadOnly = !isStartCell; // Chỉ ô bắt đầu ca là không ReadOnly
                cell.Style.BackColor = isStartCell ? Color.LightBlue : Color.LightSkyBlue; // Màu đậm hơn cho ô đầu

                // Ẩn mũi tên ComboBox cho các ô không phải ô đầu tiên của ca
                if (!isStartCell && cell is DataGridViewComboBoxCell comboCell)
                {
                    // Không có cách chính thức để ẩn, nhưng có thể đổi màu foreground/back color
                    // Tuy nhiên, ta dùng ReadOnly để ngăn người dùng chỉnh sửa là đủ.
                }
            }
            else
            {
                cell.ReadOnly = false;
                cell.Style.BackColor = Color.White; // Reset màu nền
            }
        }

        // =========================================================
        // PHƯƠNG THỨC XỬ LÝ SỰ KIỆN KHI GIÁ TRỊ Ô THAY ĐỔI (Dành cho ca gộp)
        // =========================================================
        private void Dgv_CellValueChanged_Grouped(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;

            // Chỉ xử lý khi giá trị thay đổi trên hàng dữ liệu (e.RowIndex >= 0)
            // và cột ca làm việc (e.ColumnIndex > 0 và < dgv.Columns.Count - 1)
            if (e.RowIndex >= 0 && e.ColumnIndex > 0 && e.ColumnIndex < dgv.Columns.Count - 1)
            {
                string newRole = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString() ?? "";

                // --- Logic Gộp Ca ---

                // Lấy giờ bắt đầu của ca thay đổi (ví dụ: Ca7h -> 7)
                string columnName = dgv.Columns[e.ColumnIndex].Name;
                int startHour;
                if (!int.TryParse(columnName.Substring(2).TrimEnd('h'), out startHour))
                    return;

                // Xác định phạm vi ca (4 tiếng)
                int endHour = startHour + 4;

                // Nếu đây là ô bắt đầu Ca 1 (7:00 SA)
                if (startHour == 7)
                {
                    endHour = 11; // Ca 1: 7h đến 11h
                }
                // Nếu là ô bắt đầu Ca 2 (11:00 SA)
                else if (startHour == 11)
                {
                    endHour = 15; // Ca 2: 11h đến 15h
                }
                // Nếu là ô bắt đầu của 1h nào đó không thuộc ca chuẩn, chỉ xử lý 1h đó
                else
                {
                    endHour = startHour + 1;
                }

                // Nếu người dùng chọn một giá trị (role)
                if (!string.IsNullOrEmpty(newRole))
                {
                    // Gán vai trò cho các ô tiếp theo trong phạm vi ca (và style chúng)
                    for (int hour = startHour; hour < endHour; hour++)
                    {
                        string targetColName = $"Ca{hour}h";
                        if (dgv.Columns.Contains(targetColName))
                        {
                            dgv.Rows[e.RowIndex].Cells[targetColName].Value = newRole;
                            StyleShiftCells(dgv, e.RowIndex, targetColName, newRole, hour == startHour);
                        }
                    }
                }
                // Nếu người dùng chọn ô trống ("")
                else
                {
                    // Xóa tất cả các ô trong phạm vi ca (và reset style)
                    for (int hour = startHour; hour < endHour; hour++)
                    {
                        string targetColName = $"Ca{hour}h";
                        if (dgv.Columns.Contains(targetColName))
                        {
                            dgv.Rows[e.RowIndex].Cells[targetColName].Value = "";
                            StyleShiftCells(dgv, e.RowIndex, targetColName, "", hour == startHour);
                        }
                    }
                }

                // Tái tính toán tổng giờ và tổng ca
                CalculateTotalHours(dgv, e.RowIndex);
                UpdateShiftTotals();
            }
        }

        // =========================================================
        // PHƯƠNG THỨC 2, 3, 5, 6, HỖ TRỢ (GIỮ NGUYÊN)
        // =========================================================
        private void SetupDataGridView(DataGridView dgv)
        {
            // GIỮ NGUYÊN CẤU HÌNH ĐÃ CHỈNH SỬA Ở LẦN TRƯỚC VỚI STYLE ĐẸP HƠN
            dgv.Columns.Clear();
            dgv.RowHeadersVisible = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToResizeRows = false;

            // Style (Tối Giản/Hiện Đại)
            dgv.EnableHeadersVisualStyles = false;
            dgv.RowTemplate.Height = 30;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 62, 80);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(45, 62, 80);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font(dgv.Font, FontStyle.Bold);
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = Color.Black;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            dgv.DefaultCellStyle.SelectionBackColor = Color.LightSkyBlue;
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.GridColor = Color.Gainsboro;
            dgv.BorderStyle = BorderStyle.None;

            // Cột TÊN
            dgv.Columns.Add("Ten", "TÊN");
            dgv.Columns["Ten"].ReadOnly = true;
            dgv.Columns["Ten"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            // Cột Giờ (7:00 SA đến 3:00 CH)
            for (int hour = 7; hour <= 15; hour++)
            {
                string headerText = (hour < 12) ? $"{hour}:00 SA" : (hour == 12 ? "12:00 CH" : $"{hour - 12}:00 CH");
                string columnName = $"Ca{hour}h";

                DataGridViewComboBoxColumn comboCol = new DataGridViewComboBoxColumn();
                comboCol.Name = columnName;
                comboCol.HeaderText = headerText;
                comboCol.Width = 75;
                comboCol.Items.AddRange("");
                dgv.Columns.Add(comboCol);
            }

            // Cột TỔNG
            dgv.Columns.Add("TongGio", "TỔNG");
            dgv.Columns["TongGio"].ReadOnly = true;
            dgv.Columns["TongGio"].Width = 60;

            // KHÔNG GÁN LẠI SỰ KIỆN Ở ĐÂY. SỰ KIỆN Dgv_CellValueChanged_Grouped ĐÃ ĐƯỢC GÁN Ở HÀM Load.
        }

        // Duyệt qua các cột ca (từ cột 1 đến cột 10, bỏ qua cột TÊN và TỔNG GIỜ)
        private void CalculateTotalHours(DataGridView dgv, int rowIndex)
        {
            // GIỮ NGUYÊN
            int totalHours = 0;
            for (int col = 1; col < dgv.Columns.Count - 1; col++)
            {
                if (dgv.Rows[rowIndex].Cells[col].Value != null)
                {
                    string ca = dgv.Rows[rowIndex].Cells[col].Value.ToString();
                    // Chỉ tính các ca làm việc, bỏ qua "ôm" và ""
                    if (!string.IsNullOrEmpty(ca) && ca.ToLower() != "")
                    {
                        totalHours++;
                    }
                }
            }

            dgv.Rows[rowIndex].Cells["TongGio"].Value = totalHours;
        }

        private void tabControllichca_SelectedIndexChanged(object sender, EventArgs e)
        {
            // GIỮ NGUYÊN LOGIC NẠP DỮ LIỆU MẪU
            string dayOfWeekText = tabControllichca.SelectedTab.Text;
            string dayOfWeekLower = "";

            if (dayOfWeekText == "THỨ HAI") dayOfWeekLower = "Thứ Hai";
            else if (dayOfWeekText == "THỨ BA") dayOfWeekLower = "Thứ Ba";
            else if (dayOfWeekText == "THỨ TƯ") dayOfWeekLower = "Thứ Tư";
            else if (dayOfWeekText == "THỨ NĂM") dayOfWeekLower = "Thứ Năm";
            else if (dayOfWeekText == "THỨ SÁU") dayOfWeekLower = "Thứ Sáu";

            DataGridView dgv = GetDgvByDay(dayOfWeekLower);

            if (dgv != null)
            {
                LoadShiftData_Sample(dgv, dayOfWeekLower);
            }
        }

        // =========================================================
        // PHƯƠNG THỨC 5: XỬ LÝ SỰ KIỆN CHUYỂN TAB (Giữ nguyên)
        // =========================================================
        private void tabControllichca_SelectedInde(object sender, EventArgs e)
        {
            UpdateShiftTotals();
        }




        private DataGridView GetCurrentDataGridView()
        {
            // GIỮ NGUYÊN
            if (tabControllichca.SelectedTab.Controls.Count > 0)
            {
                return tabControllichca.SelectedTab.Controls.OfType<DataGridView>().FirstOrDefault();
            }
            return null;
        }

        private DataGridView GetDgvByDay(string dayOfWeek)
        {
            // GIỮ NGUYÊN
            switch (dayOfWeek.ToUpper())
            {
                case "THỨ HAI": return dgvthu2;
                case "THỨ BA": return dgvthu3;
                case "THỨ TƯ": return dgvthu4;
                case "THỨ NĂM": return dgvthu5;
                case "THỨ SÁU": return dgvthu6;
                default: return null;
            }
        }

        private void UpdateShiftTotals()
        {
            // GIỮ NGUYÊN
            DataGridView currentDgv = GetCurrentDataGridView();

            if (currentDgv == null) return;

            // 2. Tính toán tổng số người trên mỗi ca
            Dictionary<string, int> shiftCounts = new Dictionary<string, int>();

            for (int col = 1; col < currentDgv.Columns.Count - 1; col++)
            {
                shiftCounts.Add(currentDgv.Columns[col].HeaderText, 0);
            }

            foreach (DataGridViewRow row in currentDgv.Rows)
            {
                for (int col = 1; col < currentDgv.Columns.Count - 1; col++)
                {
                    if (row.Cells[col].Value != null && !string.IsNullOrEmpty(row.Cells[col].Value.ToString()) && row.Cells[col].Value.ToString().ToLower() != "ôm")
                    {
                        string header = currentDgv.Columns[col].HeaderText;
                        if (shiftCounts.ContainsKey(header))
                        {
                            shiftCounts[header]++;
                        }
                    }
                }
            }
            // 3. Hiển thị kết quả lên Label (lblTongCa)
            string result = "TỔNG NGƯỜI TRÊN CA: ";
            foreach (var item in shiftCounts)
            {
                result += $"{item.Key}: {item.Value} người; ";
            }

            // Kiểm tra và hiển thị kết quả lên Label
            Control[] foundControls = this.Controls.Find("lblTongCa", true);
            if (foundControls.Length > 0 && foundControls[0] is Label)
            {
                Label lblTongCa = (Label)foundControls[0];
                lblTongCa.Text = result.TrimEnd(';', ' ');
            }
        }
    }
}