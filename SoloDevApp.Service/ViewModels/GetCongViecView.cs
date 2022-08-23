using System.Collections.Generic;

namespace SoloDevApp.Service.ViewModels
{
    //dung cho method GET cong viec
    public class GetCongViecView
    {
        public int Id { get; set; }
        public CongViecViewModel CongViec { get; set; }
        public string TenLoaiCongViec { get; set; }
        public string TenNhomChiTietLoai { get; set; }
        public string TenChiTietLoai { get; set; }
        public string TenNguoiTao { get; set; }
        public string Avatar { get; set; }

    }
}