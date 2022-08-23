using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SoloDevApp.Repository.Models;
using SoloDevApp.Repository.Repositories;
using SoloDevApp.Service.Constants;
using SoloDevApp.Service.Helpers;
using SoloDevApp.Service.Infrastructure;
using SoloDevApp.Service.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SoloDevApp.Service.Services
{
    public interface ILoaiCongViecService : IService<LoaiCongViec, LoaiCongViecViewModel>
    {
   


    }

    public class LoaiCongViecService : ServiceBase<LoaiCongViec, LoaiCongViecViewModel>, ILoaiCongViecService
    {
        private readonly ILoaiCongViecRepository _loaiCongViecRepository;

        public LoaiCongViecService(ILoaiCongViecRepository loaiCongViecRepository, 
            IMapper mapper,
            IAppSettings appSettings,
            IFileService fileService
            )
            : base(loaiCongViecRepository, mapper)
        {
            _loaiCongViecRepository = loaiCongViecRepository;
        }
       
    }
}