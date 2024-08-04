using GtrTraingHr.Data.Repository.Interface;
using GtrTraingHr.Models;
using Microsoft.EntityFrameworkCore;

namespace GtrTraingHr.Data.Repository.Implemention
{
    public class CompanyRepo : Repository<Company>, ICompanyRepo
    {
        public CompanyRepo(GtrDbContext db) : base(db)
        {
        }

        public async Task<List<Company>> GetActiveCompany(bool isactive)
        {
            return await db.Companies.Where(x => x.IsInactive == isactive).ToListAsync();
        }
    }
}
