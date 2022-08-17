using AutoMapper;
using SoloDevApp.Repository.Models;
using SoloDevApp.Repository.Repositories;
using SoloDevApp.Service.Constants;
using SoloDevApp.Service.Infrastructure;
using SoloDevApp.Service.ViewModels;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoloDevApp.Service.Services
{
    public interface INguoiDungService : IService<NguoiDung, NguoiDungViewModel>
    {
        Task<ResponseEntity> GetByName(string TenNguoiDung);

    }

    public class NguoiDungService : ServiceBase<NguoiDung, NguoiDungViewModel>, INguoiDungService
    {
        private readonly INguoiDungRepository _nguoiDungRepository;

        public NguoiDungService(INguoiDungRepository nguoiDungRepository, IMapper mapper)
            : base(nguoiDungRepository, mapper)
        {
            _nguoiDungRepository = nguoiDungRepository;
        }

        public async Task<ResponseEntity> GetByName(string TenNguoiDung)
        {
            try
            {
              
                string formatName = TenNguoiDung.ToLower().Trim();
                IEnumerable<NguoiDung> entity = await _nguoiDungRepository.GetAllAsync();

                List<NguoiDung> nguoiDung = entity.Where(n => n.Name.Contains(formatName)).ToList();

             

                return new ResponseEntity(StatusCodeConstants.OK, nguoiDung);
            }
            catch
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER);
            }
        }

        /*  private async Task<string> GenerateToken(NguoiDung entity)
          {
              try
              {
                  NhomQuyen nhomQuyen = await _nhomQuyenRepository.GetByIdAsync(entity.MaNhomQuyen);
                  if (nhomQuyen == null)
                      return string.Empty;

                  List<string> roles = JsonConvert.DeserializeObject<List<string>>(nhomQuyen.DanhSachQuyen);

                  List<Claim> claims = new List<Claim>();
                  claims.Add(new Claim(ClaimTypes.Name, entity.Id));
                  claims.Add(new Claim(ClaimTypes.Email, entity.Email));
                  foreach (var item in roles)
                  {
                      claims.Add(new Claim(ClaimTypes.Role, item.Trim()));
                  }

                  var secret = Encoding.ASCII.GetBytes(_appSettings.Secret);
                  var token = new JwtSecurityToken(
                      claims: claims,
                      notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                      expires: new DateTimeOffset(DateTime.Now.AddMinutes(60)).DateTime,
                      signingCredentials: new SigningCredentials(new SymmetricSecurityKey(secret),
                          SecurityAlgorithms.HmacSha256Signature)
                  );
                  return new JwtSecurityTokenHandler().WriteToken(token);
              }
              catch (Exception ex)
              {
                  Console.WriteLine(ex);
                  throw;
              }
          }*/
    }
}