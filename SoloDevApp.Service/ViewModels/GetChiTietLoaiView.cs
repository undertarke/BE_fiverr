using System.Collections.Generic;

namespace SoloDevApp.Service.ViewModels
{
    //dung cho method GET chi tiet loai cong viec
    public class GetChiTietLoaiView
    {
        public int Id { get; set; }
        public string TenNhom { get; set; }
        public string HinhAnh { get; set; }
        public int MaLoaiCongviec { get; set; }

        public List<ChiTietLoaiView> dsChiTietLoai { get; set; }
    }
}