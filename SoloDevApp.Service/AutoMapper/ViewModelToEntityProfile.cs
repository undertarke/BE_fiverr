using AutoMapper;
using Newtonsoft.Json;
using SoloDevApp.Repository.Models;
using SoloDevApp.Service.ViewModels;

namespace SoloDevApp.Service.AutoMapper
{
    public class ViewModelToEntityProfile : Profile
    {
        public ViewModelToEntityProfile()
        {
            CreateMap< SkillViewModel, Skill>();
            // CreateMap<NhomQuyenViewModel, NhomQuyen>().ForMember(dest => dest.DanhSachQuyen, m => m.MapFrom(src => JsonConvert.SerializeObject(src.DanhSachQuyen)));
            CreateMap<NguoiDungViewModel, NguoiDung>().ForMember(dest => dest.Skill, m => m.MapFrom(src => JsonConvert.SerializeObject(src.Skill))).ForMember(dest => dest.Certification, m => m.MapFrom(src => JsonConvert.SerializeObject(src.Certification)));
        }
    }
}