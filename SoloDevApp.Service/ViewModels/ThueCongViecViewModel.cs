using System.Collections.Generic;

namespace SoloDevApp.Service.ViewModels
{
    public class ThueCongViecViewModel
    {
        public int Id { get; set; }
        public int MaCongViec { get; set; }
        public int MaNguoiThue { get; set; }
        public string NgayThue { get; set; }    
        public bool HoanThanh { get; set; }

    }
}