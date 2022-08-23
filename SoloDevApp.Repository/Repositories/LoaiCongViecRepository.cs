using Microsoft.Extensions.Configuration;
using SoloDevApp.Repository.Infrastructure;
using SoloDevApp.Repository.Models;

namespace SoloDevApp.Repository.Repositories
{
    public interface ILoaiCongViecRepository : IRepository<LoaiCongViec>
    {
    
    }

    public class LoaiCongViecRepository : RepositoryBase<LoaiCongViec>, ILoaiCongViecRepository
    {
        public LoaiCongViecRepository(IConfiguration configuration)
            : base(configuration)
        {
        }

  
    }
}