using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeAn_qlNhanVien
{
    public class Task
    {
        public int MaCV { get; set; }
        public string TieuDe { get; set; }
        public string MoTa { get; set; }
        public DateTime ThoiGian_BD { get; set; }
        public DateTime ThoiGian_KT { get; set; }

        // Sử dụng Enum cho các trường có giá trị giới hạn
        public MucDoUuTien MucDoUuTien { get; set; }
        public TrangThai TrangThai { get; set; }

        // Dữ liệu từ bảng nhanvien (thông qua bảng PhanCong)
        public string NguoiDuocGiao { get; set; }

        // Constructor để gán giá trị mặc định, tương ứng với "DEFAULT" trong SQL
        public Task()
        {
            MucDoUuTien = MucDoUuTien.TrungBinh;
            TrangThai = TrangThai.ChuaLam;
        }
    }

    // Các Enum tương ứng với các ràng buộc CHECK trong SQL
    public enum MucDoUuTien
    {
        Thap,
        TrungBinh,
        Cao
    }

    public enum TrangThai
    {
        ChuaLam,
        DangLam,
        HoanThanh,
    }
}
