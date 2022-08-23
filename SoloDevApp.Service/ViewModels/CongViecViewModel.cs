using System.Collections.Generic;

namespace SoloDevApp.Service.ViewModels
{
    public class CongViecViewModel
    {
        public int Id { get; set; }
        public string TenCongViec { get; set; }
        public int DanhGia { get; set; }
        public int GiaTien { get; set; }
        public int NguoiTao { get; set; }
       
        public string HinhAnh { get; set; }
        public string MoTa { get; set; }
        public int MaChiTietLoaiCongViec { get; set; }
        public string MoTaNgan { get; set; }
        public int SaoCongViec { get; set; }
    }
}