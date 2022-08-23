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
    public interface IBinhLuanService : IService<BinhLuan, BinhLuanViewModel>
    {
        // Task<ResponseEntity> GetCongViecDaThue(string nguoiDungId);

    }

    public class BinhLuanService : ServiceBase<BinhLuan, BinhLuanViewModel>, IBinhLuanService
    {
        private readonly IBinhLuanRepository _binhLuanRepository;
        private readonly ICongViecRepository _congViecRepository;
        private readonly ICongViecService _congViecService;

        private readonly ILoaiCongViecRepository _loaiCongViecRepository;
        private readonly IChiTietLoaiCongViecRepository _chiTietLoaiCongViecRepository;
        private readonly IChiTietLoaiCongViecService _chiTietLoaiCongViecService;
        private readonly INguoiDungRepository _nguoiDungRepository;

        private readonly IAppSettings _appSettings;
        private readonly IFileService _fileService;
        private readonly string URL_MAIN = "https://localhost:5001";

        public BinhLuanService(IBinhLuanRepository binhLuanRepository,
            ILoaiCongViecRepository loaiCongViecRepository,
            IChiTietLoaiCongViecService chiTietLoaiCongViecService,
            IChiTietLoaiCongViecRepository chiTietLoaiCongViecRepository,
            INguoiDungRepository nguoiDungRepository,
            ICongViecRepository congViecRepository,
            ICongViecService congViecService,
        IMapper mapper,
            IAppSettings appSettings,
            IFileService fileService
            )
            : base(binhLuanRepository, mapper)
        {
            _binhLuanRepository = binhLuanRepository;
            _loaiCongViecRepository = loaiCongViecRepository;
            _chiTietLoaiCongViecService = chiTietLoaiCongViecService;
            _chiTietLoaiCongViecRepository = chiTietLoaiCongViecRepository;
            _nguoiDungRepository = nguoiDungRepository;
            _appSettings = appSettings;
            _fileService = fileService;

            _congViecService = congViecService;
            _congViecRepository = congViecRepository;

        }

       /* public async Task<ResponseEntity> HoanThanhCongViec(string MaBinhLuan)
        {
            try
            {
                BinhLuan BinhLuan = await _BinhLuanRepository.GetByIdAsync(int.Parse(MaBinhLuan));

                if(BinhLuan == null)
                    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, "Không có công việc nào được thuê");

                BinhLuan.HoanThanh = true;
                await _BinhLuanRepository.UpdateAsync(BinhLuan.Id, BinhLuan);

                return new ResponseEntity(StatusCodeConstants.OK, BinhLuan);
            }
            catch
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER);
            }
        }
        */

    }
}