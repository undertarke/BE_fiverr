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
    [Route("api/thue-cong-viec")]
    [ApiController]
    [ApiKeyAuth("")]
    public class ThueCongViecController : ControllerBase
    {
        private readonly IThueCongViecService _thueCongViecService;

        public ThueCongViecController(IThueCongViecService thueCongViecService)
        {
            _thueCongViecService = thueCongViecService;
        }

       [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await _thueCongViecService.GetAllAsync();
        }

        [HttpGet("phan-trang-tim-kiem")]
        public async Task<IActionResult> GetPaging(int pageIndex, int pageSize, string keyword)
        {
            return await _thueCongViecService.GetPagingAsync(pageIndex, pageSize, keyword == null ? keyword : " TenCongViec LIKE N'%" + keyword + "%'");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return await _thueCongViecService.GetByIdAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromHeader] string token,[FromBody] ThueCongViecViewModel model)
        {
            string nguoiDungId = FuncUtilities.CheckToken(token, false);
            string sMess = FuncUtilities.TokenMessage(nguoiDungId, false);
            if (sMess != "")
                return new ResponseEntity(403, sMess);

            model.HoanThanh = false;
            return await _thueCongViecService.InsertAsync(model);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromHeader] string token, int id, [FromBody] ThueCongViecViewModel model)
        {
            string nguoiDungId = FuncUtilities.CheckToken(token, false);
            string sMess = FuncUtilities.TokenMessage(nguoiDungId, false);
            if (sMess != "")
                return new ResponseEntity(403, sMess);


            return await _thueCongViecService.UpdateAsync(id, model);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromHeader] string token, int id)
        {
            string nguoiDungId = FuncUtilities.CheckToken(token, false);
            string sMess = FuncUtilities.TokenMessage(nguoiDungId, false);
            if (sMess != "")
                return new ResponseEntity(403, sMess);

            return await _thueCongViecService.DeleteByIdAsync(id);
        }

        [HttpGet("lay-danh-sach-da-thue")]
        public async Task<IActionResult> GetCongViecDaThue([FromHeader] string token)
        {
            string nguoiDungId = FuncUtilities.CheckToken(token, false);
            string sMess = FuncUtilities.TokenMessage(nguoiDungId, false);
            if (sMess != "")
                return new ResponseEntity(403, sMess);

            return await _thueCongViecService.GetCongViecDaThue(nguoiDungId);
        }

        [HttpPost("hoan-thanh-cong-viec/{MaThueCongViec}")]
        public async Task<IActionResult> HoanThanhCongViec(string MaThueCongViec)
        {

            return await _thueCongViecService.HoanThanhCongViec(MaThueCongViec);
        }
    }
}