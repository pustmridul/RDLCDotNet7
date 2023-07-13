using rdlc.Entities;

namespace rdlc.Contracts
{
    public interface ICompanyRepository
    {
        public Task<List<Company>> GetCompanies();
    }
}
