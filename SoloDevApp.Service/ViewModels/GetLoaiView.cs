using System.Collections.Generic;

namespace SoloDevApp.Service.ViewModels
{
    //dung cho method GET loai cong viec
    public class GetLoaiView
    {
        public int Id { get; set; }
        public string TenLoaiCongViec { get; set; }

        public List<GetChiTietLoaiView> dsNhomChiTietLoai { get; set; }
    }
}