using Microsoft.AspNetCore.Mvc;
using SoloDevApp.Api.Filters;
using SoloDevApp.Service.Services;
using SoloDevApp.Service.ViewModels;
using System.Threading.Tasks;

namespace SoloDevApp.Api.Controllers
{
    [Route("api/nguoidung")]
    [ApiController]
    [ApiKeyAuth("")]
    public class NguoiDungController : ControllerBase
    {
        private readonly INguoiDungService _nguoiDungService;

        public NguoiDungController(INguoiDungService nguoiDungService)
        {
            _nguoiDungService = nguoiDungService;
        }

        [HttpGet("layTatCaNguoiDung")]
        public async Task<IActionResult> Get()
        {
            return await _nguoiDungService.GetAllAsync();
        }

        [HttpGet("layNguoiDungTheoMa/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return await _nguoiDungService.GetByIdAsync(id);
        }

 

        [HttpPost("taoNguoiDung")]
        public async Task<IActionResult> Post([FromBody] NguoiDungViewModel model)
        {
            return await _nguoiDungService.InsertAsync(model);
        }

        [HttpPut("capNhatNguoiDung/{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] NguoiDungViewModel model)
        {
            return await _nguoiDungService.UpdateAsync(id, model);
        }

        [HttpDelete(("xoaNguoiDung"))]
        public async Task<IActionResult> Delete(int id)
        {
            return await _nguoiDungService.DeleteByIdAsync(id);
        }

        [HttpGet("timKiem/{TenNguoiDung}")]
        public async Task<IActionResult> GetByName(string TenNguoiDung)
        {
            return await _nguoiDungService.GetByName(TenNguoiDung);
        }
    }
}