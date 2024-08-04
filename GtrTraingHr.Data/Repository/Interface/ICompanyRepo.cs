using GtrTraingHr.Models;

namespace GtrTraingHr.Data.Repository.Interface
{
    public interface ICompanyRepo:IRepository<Company>
    {
        Task<List<Company>> GetActiveCompany(bool isactive);
    }
}
