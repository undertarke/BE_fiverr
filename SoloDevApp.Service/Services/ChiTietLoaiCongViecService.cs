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
    public interface IChiTietLoaiCongViecService : IService<ChiTietLoaiCongViec, ChiTietLoaiCongViecViewModel>
    {
        Task<ResponseEntity> LayTatCa();
        Task<ResponseEntity> LayTheoId(int id);

        Task<ResponseEntity> ThemChiTietLoaiCongViec(ChiTietLoaiView model);
        Task<ResponseEntity> SuaChiTietLoaiCongViec(int id, ChiTietLoaiView model);
        Task<ResponseEntity> LayPhanTrang(int pageIndex, int pageSize, string keyword);


        List<GetChiTietLoaiView> LayDanhSachChiTietLoaiCongViec(IEnumerable<ChiTietLoaiCongViec> dsChiTiet, IEnumerable<ChiTietLoaiCongViec> dsNhom);

        Task<ResponseEntity> UploadHinhNhomLoai(int MaNhomLoaiCongViec, Photo file);
    }

    public class ChiTietLoaiCongViecService : ServiceBase<ChiTietLoaiCongViec, ChiTietLoaiCongViecViewModel>, IChiTietLoaiCongViecService
    {
        private readonly IChiTietLoaiCongViecRepository _chiTietLoaiCongViecRepository;
        private readonly IFileService _fileService;
        private readonly IAppSettings _appSettings;

        public ChiTietLoaiCongViecService(IChiTietLoaiCongViecRepository chiTietLoaiCongViecRepository, 
            IMapper mapper,
            IAppSettings appSettings,
            IFileService fileService
            )
            : base(chiTietLoaiCongViecRepository, mapper)
        {
            _chiTietLoaiCongViecRepository = chiTietLoaiCongViecRepository;
            _fileService = fileService;
            _appSettings = appSettings;
        }
        public async Task<ResponseEntity> UploadHinhNhomLoai(int MaNhomLoaiCongViec, Photo file)
        {
            try
            {
                ChiTietLoaiCongViec chiTietLoaiCongViec = await _chiTietLoaiCongViecRepository.GetByIdAsync(MaNhomLoaiCongViec);
                
                if(chiTietLoaiCongViec == null || chiTietLoaiCongViec.MaLoaiCongViec == 0)
                    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, "Không tìm thấy nhóm chi tiết loại công việc");

                if (file.formFile == null)
                    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, "Không tìm thấy hình để thêm");

                //kiem tra dugn luong hinh chi cho phep hinh < 1Mb
                if (file.formFile.Length > 1000000)
                    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, "Dung lượng hình phải dưới 1Mb");

                if (file.formFile.ContentType != "image/jpg" && file.formFile.ContentType != "image/jpeg" && file.formFile.ContentType != "image/png" && file.formFile.ContentType != "image/gif")
                    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, "Chỉ cho phép dịnh dạng (jpg, jpeg, png, gif)");

                string filePath = "";

                filePath = await _fileService.SaveFileAsync(file.formFile, "images");

                chiTietLoaiCongViec.HinhAnh = _appSettings.UrlMain + filePath;
                await _chiTietLoaiCongViecRepository.UpdateAsync(chiTietLoaiCongViec.Id, chiTietLoaiCongViec);


                return new ResponseEntity(StatusCodeConstants.OK, chiTietLoaiCongViec);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ResponseEntity> LayPhanTrang(int pageIndex, int pageSize, string keyword)
        {
            try
            {

                //lay tat ca danh sach chi tiet loai cong viec
                IEnumerable<ChiTietLoaiCongViec> dsChiTiet = await _chiTietLoaiCongViecRepository.GetAllAsync();
                //lay danh sach nhom chi tiet
                IEnumerable<ChiTietLoaiCongViec> dsNhom = dsChiTiet.Where(n => n.MaLoaiCongViec != 0);


                List<GetChiTietLoaiView> dsGetChiTiet = LayDanhSachChiTietLoaiCongViec(dsChiTiet, dsNhom);
              
                //tim kiem
                dsGetChiTiet = dsGetChiTiet.Where(n => n.TenNhom.ToLower().Trim().Contains(keyword.ToLower().Trim())).ToList();

                //phan trang
                int iSkip = (pageIndex - 1) * pageSize;
                List<GetChiTietLoaiView> dsGetChiTietPaging = dsGetChiTiet.Where(n=>n.TenNhom.ToLower().Trim().Contains(keyword.ToLower().Trim())).Skip(iSkip).Take(pageSize).ToList();

                PagingData<GetChiTietLoaiView> pageData= new PagingData<GetChiTietLoaiView>();
                pageData.PageSize = pageSize;
                pageData.PageIndex = pageIndex; 
                pageData.Keywords = keyword;
                pageData.Data = dsGetChiTietPaging;
                pageData.TotalRow = dsGetChiTiet.Count();

                return new ResponseEntity(StatusCodeConstants.OK, pageData);
            }
            catch
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER);
            }
        }

        public async Task<ResponseEntity> LayTatCa()
        {
            try
            {
                //lay tat ca danh sach chi tiet loai cong viec
                IEnumerable<ChiTietLoaiCongViec> dsChiTiet = await _chiTietLoaiCongViecRepository.GetAllAsync();
                //lay danh sach nhom chi tiet
                IEnumerable<ChiTietLoaiCongViec> dsNhom = dsChiTiet.Where(n => n.MaLoaiCongViec != 0);


                List<GetChiTietLoaiView> dsGetChiTiet = LayDanhSachChiTietLoaiCongViec(dsChiTiet, dsNhom);


                return new ResponseEntity(StatusCodeConstants.OK, dsGetChiTiet);
            }
            catch
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER);
            }
        }

        public async Task<ResponseEntity> LayTheoId(int id)
        {
            try
            {
                //lay tat ca danh sach chi tiet loai cong viec
                IEnumerable<ChiTietLoaiCongViec> dsChiTiet = await _chiTietLoaiCongViecRepository.GetAllAsync();
                //lay danh sach nhom chi tiet
                IEnumerable<ChiTietLoaiCongViec> dsNhom = dsChiTiet.Where(n => n.MaLoaiCongViec != 0);


                List<GetChiTietLoaiView> dsGetChiTiet = LayDanhSachChiTietLoaiCongViec(dsChiTiet, dsNhom);

                GetChiTietLoaiView getChiTiet = dsGetChiTiet.FirstOrDefault(n => n.Id == id);
                if(getChiTiet != null)
                    return new ResponseEntity(StatusCodeConstants.OK, getChiTiet);
                else
                {
                    ChiTietLoaiCongViec chiTietLoai = await _chiTietLoaiCongViecRepository.GetByIdAsync(id);
                    if(chiTietLoai != null)
                    {
                        ChiTietLoaiView chiTietLoaiView = new ChiTietLoaiView();
                        chiTietLoaiView.Id = chiTietLoai.Id;
                        chiTietLoaiView.TenChiTiet = chiTietLoai.TenChiTiet;

                        return new ResponseEntity(StatusCodeConstants.OK, chiTietLoaiView);
                    }else
                        return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, "Không tồn tại");



                }

            }
            catch
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER);
            }
        }

        public List<GetChiTietLoaiView> LayDanhSachChiTietLoaiCongViec(IEnumerable<ChiTietLoaiCongViec> dsChiTiet, IEnumerable<ChiTietLoaiCongViec> dsNhom)
        {
            //tao va them danh sach view tra cho user
            List<GetChiTietLoaiView> dsGetChiTiet = new List<GetChiTietLoaiView>();
            foreach (ChiTietLoaiCongViec nhom in dsNhom)
            {
                GetChiTietLoaiView getChiTiet = new GetChiTietLoaiView();
                getChiTiet.Id= nhom.Id;
                getChiTiet.TenNhom = nhom.TenChiTiet;
                getChiTiet.HinhAnh = nhom.HinhAnh;

                //lay loai cong viec
                getChiTiet.MaLoaiCongviec = nhom.MaLoaiCongViec;

                // duyet lay danh sach chi tiet loai cong viec 
                //map qua viewmodel
                ChiTietLoaiCongViecViewModel nhomChiTietLoai = _mapper.Map<ChiTietLoaiCongViecViewModel>(nhom);
                List<ChiTietLoaiView> lstGetChiTietLoai = new List<ChiTietLoaiView>();
                foreach (int item in nhomChiTietLoai.DanhSachChiTiet)
                {
                    ChiTietLoaiCongViec chiTietLoai = dsChiTiet.FirstOrDefault(n => n.Id == item);
                    if (chiTietLoai != null)
                    {
                        ChiTietLoaiView chiTietLoaiView = new ChiTietLoaiView();
                        chiTietLoaiView.Id = chiTietLoai.Id;
                        chiTietLoaiView.TenChiTiet = chiTietLoai.TenChiTiet;
                        lstGetChiTietLoai.Add(chiTietLoaiView);
                    }

                }

                getChiTiet.dsChiTietLoai = lstGetChiTietLoai;
                dsGetChiTiet.Add(getChiTiet);
            }

            return dsGetChiTiet;
        }

        public async Task<ResponseEntity> ThemChiTietLoaiCongViec(ChiTietLoaiView model)
        {
            try
            {
                ChiTietLoaiCongViec chiTietLoaiCongViec = new ChiTietLoaiCongViec();
                chiTietLoaiCongViec.TenChiTiet= model.TenChiTiet;
                chiTietLoaiCongViec.MaLoaiCongViec = 0;
                chiTietLoaiCongViec.HinhAnh = "";
                chiTietLoaiCongViec.DanhSachChiTiet = "[]";
                await _chiTietLoaiCongViecRepository.InsertAsync(chiTietLoaiCongViec);

                return new ResponseEntity(StatusCodeConstants.OK, chiTietLoaiCongViec);
            }
            catch
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER);
            }
        }

        public async Task<ResponseEntity> SuaChiTietLoaiCongViec(int id, ChiTietLoaiView model)
        {
            try
            {
                ChiTietLoaiCongViec chiTietLoaiCongViec = await _chiTietLoaiCongViecRepository.GetByIdAsync(id);
                chiTietLoaiCongViec.TenChiTiet = model.TenChiTiet;
                chiTietLoaiCongViec.MaLoaiCongViec = 0;
                chiTietLoaiCongViec.HinhAnh = "";
                chiTietLoaiCongViec.DanhSachChiTiet = "[]";
                await _chiTietLoaiCongViecRepository.UpdateAsync(chiTietLoaiCongViec.Id,chiTietLoaiCongViec);

                return new ResponseEntity(StatusCodeConstants.OK, chiTietLoaiCongViec);
            }
            catch
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER);
            }
        }
    }
}