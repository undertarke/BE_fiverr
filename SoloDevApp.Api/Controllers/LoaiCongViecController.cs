using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SoloDevApp.Api.Filters;
using SoloDevApp.Service.Services;
using SoloDevApp.Service.Utilities;
using SoloDevApp.Service.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoloDevApp.Api.Controllers
{
    [Route("api/loai-cong-viec")]
    [ApiController]
    [ApiKeyAuth("")]
    public class LoaiCongViecController : ControllerBase
    {
        private readonly ILoaiCongViecService _loaiCongViecService;

        public LoaiCongViecController(ILoaiCongViecService loaiCongViecService)
        {
            _loaiCongViecService = loaiCongViecService;
        }

       [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await _loaiCongViecService.GetAllAsync();
        }

        [HttpGet("phan-trang-tim-kiem")]
        public async Task<IActionResult> GetPaging(int pageIndex, int pageSize, string keyword)
        {
            return await _loaiCongViecService.GetPagingAsync(pageIndex, pageSize, keyword == null ? keyword : " TenLoaiCongViec LIKE N'%" + keyword + "%'");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return await _loaiCongViecService.GetByIdAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromHeader] string token,[FromBody] LoaiCongViecViewModel model)
        {
            string nguoiDungId = FuncUtilities.CheckToken(token, true);
            string sMess = FuncUtilities.TokenMessage(nguoiDungId,true);
            if (sMess != "")
                return new ResponseEntity(403, sMess);

            return await _loaiCongViecService.InsertAsync(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromHeader] string token, int id, [FromBody] LoaiCongViecViewModel model)
        {
            string nguoiDungId = FuncUtilities.CheckToken(token, true);
            string sMess = FuncUtilities.TokenMessage(nguoiDungId,true);
            if (sMess != "")
                return new ResponseEntity(403, sMess);

            List<int> lstCheck = JsonConvert.DeserializeObject<List<int>>("[1,2,3,4,5]");

            if (lstCheck.Find(n=> n == id) != 0)
            {
                return new ResponseEntity(403, "Không có quyền");

            }

            return await _loaiCongViecService.UpdateAsync(id, model);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromHeader] string token, int id)
        {
            string nguoiDungId = FuncUtilities.CheckToken(token, true);
            string sMess = FuncUtilities.TokenMessage(nguoiDungId,true);
            if (sMess != "")
                return new ResponseEntity(403, sMess);

            List<int> lstCheck = JsonConvert.DeserializeObject<List<int>>("[1,2,3,4,5]");
            if (lstCheck.Find(n => n == id) != 0)
            {
                return new ResponseEntity(403, "Không có quyền");

            }

            return await _loaiCongViecService.DeleteByIdAsync(id);
        }
    }
}