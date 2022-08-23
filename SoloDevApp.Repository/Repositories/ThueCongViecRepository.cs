using Microsoft.Extensions.Configuration;
using SoloDevApp.Repository.Infrastructure;
using SoloDevApp.Repository.Models;

namespace SoloDevApp.Repository.Repositories
{
    public interface IThueCongViecRepository : IRepository<ThueCongViec>
    {
    
    }

    public class ThueCongViecRepository : RepositoryBase<ThueCongViec>, IThueCongViecRepository
    {
        public ThueCongViecRepository(IConfiguration configuration)
            : base(configuration)
        {
        }

  
    }
}