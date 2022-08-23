using AutoMapper;
using Newtonsoft.Json;
using SoloDevApp.Repository.Models;
using SoloDevApp.Service.ViewModels;
using System.Collections.Generic;

namespace SoloDevApp.Service.AutoMapper
{
    public class EntityToViewModelProfile : Profile
    {
        public EntityToViewModelProfile()
        {
            CreateMap<Skill, SkillViewModel>();
            //  CreateMap<NhomQuyen, NhomQuyenViewModel>().ForMember(dest => dest.DanhSachQuyen, m => m.MapFrom(src => JsonConvert.DeserializeObject<List<string>>(src.DanhSachQuyen)));
              CreateMap<NguoiDung, NguoiDungViewModel>().ForMember(dest => dest.Skill, m => m.MapFrom(src => JsonConvert.DeserializeObject<List<string>>(src.Skill))).ForMember(dest => dest.Certification, m => m.MapFrom(src => JsonConvert.DeserializeObject<List<string>>(src.Certification))).ForMember(dest => dest.BookingJob, m => m.MapFrom(src => JsonConvert.DeserializeObject<List<string>>(src.BookingJob)));
            CreateMap<LoaiCongViec, LoaiCongViecViewModel>();
            CreateMap<ChiTietLoaiCongViec, ChiTietLoaiCongViecViewModel>().ForMember(dest => dest.DanhSachChiTiet, m => m.MapFrom(src => JsonConvert.DeserializeObject<List<int>>(src.DanhSachChiTiet)));
            CreateMap<CongViec, CongViecViewModel>();
            CreateMap<ThueCongViec, ThueCongViecViewModel>();
            CreateMap<BinhLuan, BinhLuanViewModel>();

        }
    }
}