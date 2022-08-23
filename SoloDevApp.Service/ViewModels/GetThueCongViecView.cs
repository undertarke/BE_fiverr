using System.Collections.Generic;

namespace SoloDevApp.Service.ViewModels
{
    //dung cho method GET cong viec nguoi dung da thue
    public class GetThueCongViecView
    {
        public int Id { get; set; }
        public string NgayThue { get; set; }
        public bool HoanThanh { get; set; }
        public CongViecViewModel CongViec { get; set; }
       

    }
}