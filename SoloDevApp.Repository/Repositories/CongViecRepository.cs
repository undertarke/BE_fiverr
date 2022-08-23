using Microsoft.Extensions.Configuration;
using SoloDevApp.Repository.Infrastructure;
using SoloDevApp.Repository.Models;

namespace SoloDevApp.Repository.Repositories
{
    public interface ICongViecRepository : IRepository<CongViec>
    {
    
    }

    public class CongViecRepository : RepositoryBase<CongViec>, ICongViecRepository
    {
        public CongViecRepository(IConfiguration configuration)
            : base(configuration)
        {
        }

  
    }
}