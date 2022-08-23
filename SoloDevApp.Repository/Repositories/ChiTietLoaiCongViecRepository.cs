using Microsoft.Extensions.Configuration;
using SoloDevApp.Repository.Infrastructure;
using SoloDevApp.Repository.Models;

namespace SoloDevApp.Repository.Repositories
{
    public interface IChiTietLoaiCongViecRepository : IRepository<ChiTietLoaiCongViec>
    {
    
    }

    public class ChiTietLoaiCongViecRepository : RepositoryBase<ChiTietLoaiCongViec>, IChiTietLoaiCongViecRepository
    {
        public ChiTietLoaiCongViecRepository(IConfiguration configuration)
            : base(configuration)
        {
        }

  
    }
}