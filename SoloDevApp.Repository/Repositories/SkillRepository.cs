﻿using Microsoft.Extensions.Configuration;
using SoloDevApp.Repository.Infrastructure;
using SoloDevApp.Repository.Models;

namespace SoloDevApp.Repository.Repositories
{
    public interface ISkillRepository : IRepository<Skill>
    {
       // Task<IEnumerable<GiangDay>> GetByGiangVienId(string giangVienId);

    }

    public class SkillRepository : RepositoryBase<Skill>, ISkillRepository
    {
        public SkillRepository(IConfiguration configuration)
            : base(configuration)
        {
        }

      /*  public async Task<IEnumerable<GiangDay>> GetByGiangVienId(string giangVienId)
        {
            string query = $"SELECT * FROM {_table} WHERE GiangVienId = '{giangVienId}';";
            using (var conn = CreateConnection())
            {
                try
                {
                    return await conn.QueryAsync<GiangDay>(query, null, null, null, CommandType.Text);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }*/
    }
}