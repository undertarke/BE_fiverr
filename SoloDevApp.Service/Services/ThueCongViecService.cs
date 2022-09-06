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
    public interface IThueCongViecService : IService<ThueCongViec, ThueCongViecViewModel>
    {
         Task<ResponseEntity> GetCongViecDaThue(string nguoiDungId);
        Task<ResponseEntity> HoanThanhCongViec(string MaThueCongViec);

    }

    public class ThueCongViecService : ServiceBase<ThueCongViec, ThueCongViecViewModel>, IThueCongViecService
    {
        private readonly IThueCongViecRepository _thueCongViecRepository;
        private readonly ICongViecRepository _congViecRepository;
        private readonly ICongViecService _congViecService;

        private readonly ILoaiCongViecRepository _loaiCongViecRepository;
        private readonly IChiTietLoaiCongViecRepository _chiTietLoaiCongViecRepository;
        private readonly IChiTietLoaiCongViecService _chiTietLoaiCongViecService;
        private readonly INguoiDungRepository _nguoiDungRepository;

        private readonly IAppSettings _appSettings;
        private readonly IFileService _fileService;

        public ThueCongViecService(IThueCongViecRepository thueCongViecRepository,
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
            : base(thueCongViecRepository, mapper)
        {
            _thueCongViecRepository = thueCongViecRepository;
            _loaiCongViecRepository = loaiCongViecRepository;
            _chiTietLoaiCongViecService = chiTietLoaiCongViecService;
            _chiTietLoaiCongViecRepository = chiTietLoaiCongViecRepository;
            _nguoiDungRepository = nguoiDungRepository;
            _appSettings = appSettings;
            _fileService = fileService;

            _congViecService = congViecService;
            _congViecRepository = congViecRepository;

        }

        public async Task<ResponseEntity> HoanThanhCongViec(string MaThueCongViec)
        {
            try
            {
                ThueCongViec thueCongViec = await _thueCongViecRepository.GetByIdAsync(int.Parse(MaThueCongViec));

                if(thueCongViec == null)
                    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, "Không có công việc nào được thuê");

                thueCongViec.HoanThanh = true;
                await _thueCongViecRepository.UpdateAsync(thueCongViec.Id, thueCongViec);

                return new ResponseEntity(StatusCodeConstants.OK, thueCongViec);
            }
            catch
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER);
            }
        }
        public async Task<ResponseEntity> GetCongViecDaThue(string nguoiDungId)
        {
            try
            {
                IEnumerable<ThueCongViec> dsThueCongViec = await _thueCongViecRepository.GetMultiByConditionAsync("MaNguoiThue",nguoiDungId);
         
                List<GetThueCongViecView> lstThueCongViec = new List<GetThueCongViecView>();

                foreach (ThueCongViec thueCongViec in dsThueCongViec)
                {

                    CongViec congViec = await _congViecRepository.GetByIdAsync(thueCongViec.MaCongViec);
                    if (congViec != null)
                    {
                        GetThueCongViecView getThueCongViec = new GetThueCongViecView();
                        getThueCongViec.Id = thueCongViec.Id;
                        getThueCongViec.NgayThue = thueCongViec.NgayThue;
                        getThueCongViec.HoanThanh = thueCongViec.HoanThanh;
                        getThueCongViec.CongViec = _mapper.Map<CongViecViewModel>(congViec);
                        lstThueCongViec.Add(getThueCongViec);
                    }

                }
      

                return new ResponseEntity(StatusCodeConstants.OK, lstThueCongViec);
            }
            catch
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER);
            }
        }

    }
}