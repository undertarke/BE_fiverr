using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SoloDevApp.Api.Filters;
using SoloDevApp.Repository.Models;
using SoloDevApp.Service.Services;
using SoloDevApp.Service.Utilities;
using SoloDevApp.Service.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoloDevApp.Api.Controllers
{
    [Route("api/cong-viec")]
    [ApiController]
    [ApiKeyAuth("")]
    public class CongViecController : ControllerBase
    {
        private readonly ICongViecService _congViecService;

        public CongViecController(ICongViecService congViecService)
        {
            _congViecService = congViecService;
        }

       [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await _congViecService.GetAllAsync();
        }

        [HttpGet("phan-trang-tim-kiem")]
        public async Task<IActionResult> GetPaging(int pageIndex, int pageSize, string keyword)
        {
            return await _congViecService.GetPagingAsync(pageIndex, pageSize, keyword == null ? keyword : " TenCongViec LIKE N'%" + keyword + "%'");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return await _congViecService.GetByIdAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromHeader] string token,[FromBody] CongViecViewModel model)
        {
            string nguoiDungId = FuncUtilities.CheckToken(token, true);
            string sMess = FuncUtilities.TokenMessage(nguoiDungId,true);
            if (sMess != "")
                return new ResponseEntity(403, sMess);

            return await _congViecService.InsertAsync(model);
        }

        [HttpPost("upload-hinh-cong-viec/{MaCongViec}")]
        public async Task<IActionResult> UploadHinhNhomLoai([FromHeader] string token, int MaCongViec, [FromForm] Photo files)
        {
            List<int> lstCheck = JsonConvert.DeserializeObject<List<int>>("[1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35]");

            if (lstCheck.Find(n => n == MaCongViec) != 0)
            {
                return new ResponseEntity(403, "Không có quyền");

            }

            string nguoiDungId = FuncUtilities.CheckToken(token, true);
            string sMess = FuncUtilities.TokenMessage(nguoiDungId, true);
            if (sMess != "")
                return new ResponseEntity(403, sMess);

            return await _congViecService.UploadHinhCongViec(MaCongViec, files);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromHeader] string token, int id, [FromBody] CongViecViewModel model)
        {
            string nguoiDungId = FuncUtilities.CheckToken(token, true);
            string sMess = FuncUtilities.TokenMessage(nguoiDungId,true);
            if (sMess != "")
                return new ResponseEntity(403, sMess);

            List<int> lstCheck = JsonConvert.DeserializeObject<List<int>>("[1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35]");

            if (lstCheck.Find(n=> n == id) != 0)
            {
                return new ResponseEntity(403, "Không có quyền");

            }

            return await _congViecService.UpdateAsync(id, model);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromHeader] string token, int id)
        {
            string nguoiDungId = FuncUtilities.CheckToken(token, true);
            string sMess = FuncUtilities.TokenMessage(nguoiDungId,true);
            if (sMess != "")
                return new ResponseEntity(403, sMess);

            List<int> lstCheck = JsonConvert.DeserializeObject<List<int>>("[1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35]");

            if (lstCheck.Find(n => n == id) != 0)
            {
                return new ResponseEntity(403, "Không có quyền");

            }

            return await _congViecService.DeleteByIdAsync(id);
        }


        [HttpGet("lay-menu-loai-cong-viec")]
        public async Task<IActionResult> GetMenuLoaiCongViec()
        {
            return await _congViecService.GetMenuLoaiCongViec(0);
        }

        [HttpGet("lay-chi-tiet-loai-cong-viec/{MaLoaiCongViec}")]
        public async Task<IActionResult> GetChiTietLoaiCongViec(int MaLoaiCongViec)
        {
            return await _congViecService.GetMenuLoaiCongViec(MaLoaiCongViec);
        }

        [HttpGet("lay-cong-viec-theo-chi-tiet-loai/{MaChiTietLoai}")]
        public async Task<IActionResult> GetCongViecTheoChiTietLoai(int MaChiTietLoai)
        {
            return await _congViecService.GetCongViecTheoChiTietLoai(MaChiTietLoai,null);
        }
        [HttpGet("lay-cong-viec-chi-tiet/{MaCongViec}")]
        public async Task<IActionResult> GetCongViecChiTiet(int MaCongViec)
        {
            return await _congViecService.GetCongViecChiTiet(MaCongViec);
        }
        [HttpGet("lay-danh-sach-cong-viec-theo-ten/{TenCongViec}")]
        public async Task<IActionResult> GetCongViecTheoTen(string TenCongViec)
        {
            return await _congViecService.GetCongViecTheoChiTietLoai(0, TenCongViec);
        }

      
    }
}