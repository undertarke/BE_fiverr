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
    public interface ICongViecService : IService<CongViec, CongViecViewModel>
    {
        Task<ResponseEntity> GetMenuLoaiCongViec(int id);
        Task<ResponseEntity> GetCongViecTheoChiTietLoai(int id, string keyword);
        Task<ResponseEntity> GetCongViecChiTiet(int id);
        Task<ResponseEntity> UploadHinhCongViec(int MaCongViec, Photo file);

        string LayTenChiTietLoaiTheoMaSub(IEnumerable<LoaiCongViec> dsLoaiCongViec, IEnumerable<ChiTietLoaiCongViec> dsChiTietLoai, int id);
    }

    public class CongViecService : ServiceBase<CongViec, CongViecViewModel>, ICongViecService
    {
        private readonly ICongViecRepository _congViecRepository;
        private readonly ILoaiCongViecRepository _loaiCongViecRepository;
        private readonly IChiTietLoaiCongViecRepository _chiTietLoaiCongViecRepository;
        private readonly IChiTietLoaiCongViecService _chiTietLoaiCongViecService;
        private readonly INguoiDungRepository _nguoiDungRepository;

        private readonly IAppSettings _appSettings;
        private readonly IFileService _fileService;
        private readonly string URL_MAIN = "https://localhost:5001";

        public CongViecService(ICongViecRepository congViecRepository,
            ILoaiCongViecRepository loaiCongViecRepository,
            IChiTietLoaiCongViecService chiTietLoaiCongViecService,
            IChiTietLoaiCongViecRepository chiTietLoaiCongViecRepository,
            INguoiDungRepository nguoiDungRepository,
        IMapper mapper,
            IAppSettings appSettings,
            IFileService fileService
            )
            : base(congViecRepository, mapper)
        {
            _congViecRepository = congViecRepository;
            _loaiCongViecRepository = loaiCongViecRepository;
            _chiTietLoaiCongViecService = chiTietLoaiCongViecService;
            _chiTietLoaiCongViecRepository = chiTietLoaiCongViecRepository;
            _nguoiDungRepository = nguoiDungRepository;

            _appSettings = appSettings;
            _fileService = fileService;
        }
        public async Task<ResponseEntity> UploadHinhCongViec(int MaCongViec, Photo file)
        {
            try
            {
                CongViec congViec = await _congViecRepository.GetByIdAsync(MaCongViec);
                if(congViec == null)
                    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, "Không tìm thấy công việc");


                if (file.formFile == null)
                    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, "Không tìm thấy hình để thêm");

                //kiem tra dugn luong hinh chi cho phep hinh < 1Mb
                if (file.formFile.Length > 1000000)
                    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, "Dung lượng hình phải dưới 1Mb");

                if (file.formFile.ContentType != "image/jpg" && file.formFile.ContentType != "image/jpeg" && file.formFile.ContentType != "image/png" && file.formFile.ContentType != "image/gif")
                    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, "Chỉ cho phép dịnh dạng (jpg, jpeg, png, gif)");

                string filePath = "";

                filePath = await _fileService.SaveFileAsync(file.formFile, "images");

                congViec.HinhAnh = URL_MAIN + filePath;
                await _congViecRepository.UpdateAsync(congViec.Id, congViec);

                return new ResponseEntity(StatusCodeConstants.OK, congViec);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ResponseEntity> GetCongViecChiTiet(int id)
        {
            try
            {
                CongViec congViec = await _congViecRepository.GetByIdAsync(id);
                IEnumerable<LoaiCongViec> dsLoaiCongViec = await _loaiCongViecRepository.GetAllAsync();
                IEnumerable<ChiTietLoaiCongViec> dsChiTietLoai = await _chiTietLoaiCongViecRepository.GetAllAsync();

                List<GetCongViecView> lstCongViec = new List<GetCongViecView>();

                ChiTietLoaiCongViec chiTietLoai = dsChiTietLoai.FirstOrDefault(n => n.Id == congViec.MaChiTietLoaiCongViec);
                NguoiDung nguoiDung = await _nguoiDungRepository.GetByIdAsync(congViec.NguoiTao);

                GetCongViecView congViecView = new GetCongViecView();
                congViecView.Id = congViec.Id;
                congViecView.CongViec = _mapper.Map<CongViecViewModel>(congViec);

                congViecView.TenLoaiCongViec = LayTenChiTietLoaiTheoMaSub(dsLoaiCongViec, dsChiTietLoai, chiTietLoai.Id);
                congViecView.TenNhomChiTietLoai = LayTenChiTietLoaiTheoMaSub(null, dsChiTietLoai, chiTietLoai.Id);
                congViecView.TenChiTietLoai = chiTietLoai.TenChiTiet;

                congViecView.TenNguoiTao = nguoiDung.Name;
                congViecView.Avatar = nguoiDung.Avatar;

                lstCongViec.Add(congViecView);


                return new ResponseEntity(StatusCodeConstants.OK, lstCongViec);
            }
            catch
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER);
            }
        }

        public async Task<ResponseEntity> GetCongViecTheoChiTietLoai(int id, string keyword)
        {
            try
            {
                IEnumerable<CongViec> dsCongViec = await _congViecRepository.GetMultiByConditionAsync("MaChiTietLoaiCongViec", id);
                if (id == 0)
                {
                    dsCongViec = await _congViecRepository.GetAllAsync();
                    dsCongViec = dsCongViec.Where(n=>n.TenCongViec.ToLower().Trim().Contains(keyword.ToLower().Trim()));
                }

                IEnumerable<LoaiCongViec> dsLoaiCongViec = await _loaiCongViecRepository.GetAllAsync();
                IEnumerable<ChiTietLoaiCongViec> dsChiTietLoai = await _chiTietLoaiCongViecRepository.GetAllAsync();

                List<GetCongViecView> lstCongViec = new List<GetCongViecView>();
                foreach(CongViec congViec in dsCongViec)
                {
                    ChiTietLoaiCongViec chiTietLoai = dsChiTietLoai.FirstOrDefault(n=>n.Id== congViec.MaChiTietLoaiCongViec);
                    NguoiDung nguoiDung = await _nguoiDungRepository.GetByIdAsync(congViec.NguoiTao);

                    GetCongViecView congViecView = new GetCongViecView();
                    congViecView.Id = congViec.Id;
                    congViecView.CongViec = _mapper.Map<CongViecViewModel>(congViec);

                    congViecView.TenLoaiCongViec = LayTenChiTietLoaiTheoMaSub(dsLoaiCongViec, dsChiTietLoai, chiTietLoai.Id);
                    congViecView.TenNhomChiTietLoai = LayTenChiTietLoaiTheoMaSub(null,dsChiTietLoai, chiTietLoai.Id);
                    congViecView.TenChiTietLoai = chiTietLoai.TenChiTiet;

                    congViecView.TenNguoiTao = nguoiDung.Name;
                    congViecView.Avatar=nguoiDung.Avatar;

                    lstCongViec.Add(congViecView);
                }
               
                return new ResponseEntity(StatusCodeConstants.OK, lstCongViec);
            }
            catch
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER);
            }
        }

        public string LayTenChiTietLoaiTheoMaSub(IEnumerable<LoaiCongViec> dsLoaiCongViec,IEnumerable<ChiTietLoaiCongViec> dsChiTietLoai, int id)
        {
            ChiTietLoaiCongViec chiTietLoai = dsChiTietLoai.Where(n => n.DanhSachChiTiet.Contains(id.ToString())).ToList()[0];
            if(chiTietLoai!= null)
            {
                if (dsLoaiCongViec == null)
                {
                    return chiTietLoai.TenChiTiet;
                }
                else
                {
                    LoaiCongViec loaiCongViec = dsLoaiCongViec.FirstOrDefault(n => n.Id == chiTietLoai.MaLoaiCongViec);
                    if (loaiCongViec != null)
                        return loaiCongViec.TenLoaiCongViec;
                    else
                        return "Không có dữ liệu";
                }
            }
            return "Không có dữ liệu";
            
        }

        public async Task<ResponseEntity> GetMenuLoaiCongViec(int id)
        {
            try
            {
                IEnumerable<LoaiCongViec> dsLoaiCongViec = await _loaiCongViecRepository.GetAllAsync();
                //kiem tra co id thi get theo id loai cong viec de lay cho trang chi tiet loai cong viec
                if(id != 0)
                    dsLoaiCongViec = dsLoaiCongViec.Where(n=>n.Id == id).ToList();


                //lay tat ca danh sach chi tiet loai cong viec
                IEnumerable<ChiTietLoaiCongViec> dsChiTiet = await _chiTietLoaiCongViecRepository.GetAllAsync();
                //lay danh sach nhom chi tiet
                IEnumerable<ChiTietLoaiCongViec> dsNhom = dsChiTiet.Where(n => n.MaLoaiCongViec != 0);

                //lay chi tiet loai cong viec
                List<GetChiTietLoaiView> dsGetChiTiet = _chiTietLoaiCongViecService.LayDanhSachChiTietLoaiCongViec(dsChiTiet, dsNhom);

                List<GetLoaiView> dsGetLoaiView = new List<GetLoaiView>();
                foreach(LoaiCongViec loaiCongViec in dsLoaiCongViec)
                {
                    GetLoaiView getLoaiView = new GetLoaiView(); 
                    getLoaiView.Id=loaiCongViec.Id;
                    getLoaiView.TenLoaiCongViec = loaiCongViec.TenLoaiCongViec;

                    getLoaiView.dsNhomChiTietLoai = dsGetChiTiet.Where(n => n.MaLoaiCongviec == loaiCongViec.Id).ToList();

                    dsGetLoaiView.Add(getLoaiView);
                }

                return new ResponseEntity(StatusCodeConstants.OK, dsGetLoaiView);
            }
            catch
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER);
            }
        }

    }
}