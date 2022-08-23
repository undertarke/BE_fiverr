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
    [Route("api/chi-tiet-loai-cong-viec")]
    [ApiController]
    [ApiKeyAuth("")]
    public class ChiTietLoaiCongViecController : ControllerBase
    {
        private readonly IChiTietLoaiCongViecService _chiTietLoaiCongViecService;

        public ChiTietLoaiCongViecController(IChiTietLoaiCongViecService chiTietLoaiCongViecService)
        {
            _chiTietLoaiCongViecService = chiTietLoaiCongViecService;
        }


       [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await _chiTietLoaiCongViecService.LayTatCa();
        }

        [HttpGet("phan-trang-tim-kiem")]
        public async Task<IActionResult> GetPaging(int pageIndex, int pageSize, string keyword)
        {
        
            return await _chiTietLoaiCongViecService.LayPhanTrang(pageIndex, pageSize, keyword != null ? keyword : "");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return await _chiTietLoaiCongViecService.LayTheoId(id);
        }




        [HttpPost("them-nhom-chi-tiet-loai")]
        public async Task<IActionResult> ThemNhomLoaiCongViec([FromHeader] string token, [FromBody] ChiTietLoaiCongViecViewModel model)
        {
            string nguoiDungId = FuncUtilities.CheckToken(token, true);
            string sMess = FuncUtilities.TokenMessage(nguoiDungId, true);
            if (sMess != "")
                return new ResponseEntity(403, sMess);

            return await _chiTietLoaiCongViecService.InsertAsync(model);
        }

        [HttpPost("upload-hinh-nhom-loai-cong-viec/{MaNhomLoaiCongViec}")]
        public async Task<IActionResult> UploadHinhNhomLoai([FromHeader] string token, int MaNhomLoaiCongViec, [FromForm] Photo files)
        {
            List<int> lstCheck = JsonConvert.DeserializeObject<List<int>>("[1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30]");

            if (lstCheck.Find(n => n == MaNhomLoaiCongViec) != 0)
            {
                return new ResponseEntity(403, "Không có quyền");

            }

            string nguoiDungId = FuncUtilities.CheckToken(token, true);
            string sMess = FuncUtilities.TokenMessage(nguoiDungId, true);
            if (sMess != "")
                return new ResponseEntity(403, sMess);

            return await _chiTietLoaiCongViecService.UploadHinhNhomLoai(MaNhomLoaiCongViec, files);

        }
        [HttpPut("sua-nhom-chi-tiet-loai/{id}")]
        public async Task<IActionResult> SuaNhomLoaiCongViec([FromHeader] string token, int id, [FromBody] ChiTietLoaiCongViecViewModel model)
        {
            string nguoiDungId = FuncUtilities.CheckToken(token, true);
            string sMess = FuncUtilities.TokenMessage(nguoiDungId, true);
            if (sMess != "")
                return new ResponseEntity(403, sMess);

            List<int> lstCheck = JsonConvert.DeserializeObject<List<int>>("[1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30]");

            if (lstCheck.Find(n => n == id) != 0)
            {
                return new ResponseEntity(403, "Không có quyền");

            }

            return await _chiTietLoaiCongViecService.UpdateAsync(id, model);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromHeader] string token,[FromBody] ChiTietLoaiView model)
        {
            string nguoiDungId = FuncUtilities.CheckToken(token, true);
            string sMess = FuncUtilities.TokenMessage(nguoiDungId, true);
            if (sMess != "")
                return new ResponseEntity(403, sMess);

            return await _chiTietLoaiCongViecService.ThemChiTietLoaiCongViec(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromHeader] string token, int id, [FromBody] ChiTietLoaiView model)
        {
            string nguoiDungId = FuncUtilities.CheckToken(token, true);
            string sMess = FuncUtilities.TokenMessage(nguoiDungId, true);
            if (sMess != "")
                return new ResponseEntity(403, sMess);

            List<int> lstCheck = JsonConvert.DeserializeObject<List<int>>("[1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30]");

            if (lstCheck.Find(n => n == id) != 0)
            {
                return new ResponseEntity(403, "Không có quyền");

            }

            return await _chiTietLoaiCongViecService.SuaChiTietLoaiCongViec(id,model);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromHeader] string token, int id)
        {
            string nguoiDungId = FuncUtilities.CheckToken(token, true);
            string sMess = FuncUtilities.TokenMessage(nguoiDungId, true);
            if (sMess != "")
                return new ResponseEntity(403, sMess);

            return await _chiTietLoaiCongViecService.DeleteByIdAsync(id);
        }
    }
}