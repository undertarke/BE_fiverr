using System.Collections.Generic;

namespace SoloDevApp.Service.ViewModels
{
    //dung de them chi tiet nhom loai cong viec
    public class ChiTietLoaiCongViecViewModel
    {
        public int Id { get; set; }
        public string TenChiTiet { get; set; }
        public int MaLoaiCongViec { get; set; }
        public List<int> DanhSachChiTiet { get; set; }
    }
}