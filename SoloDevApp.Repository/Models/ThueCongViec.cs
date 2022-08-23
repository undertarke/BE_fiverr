namespace SoloDevApp.Repository.Models
{
    public class ThueCongViec
    {
        public int Id { get; set; }
        public int MaCongViec { get; set; }
        public int MaNguoiThue { get; set; }
        public string NgayThue { get; set; }
        public bool HoanThanh { get; set; }
    }
}