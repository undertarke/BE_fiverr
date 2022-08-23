namespace SoloDevApp.Repository.Models
{
    public class ChiTietLoaiCongViec
    {
        public int Id { get; set; }
        public string TenChiTiet { get; set; }
        public int MaLoaiCongViec { get; set; }
        public string DanhSachChiTiet { get; set; }
        public string HinhAnh { get; set; }

    }
}