using BulkyBook.DataAccess.Repository.Generic;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository.Company
{
    public interface ICompanyRepository : IGenericRepository<CompanyModel>
    {
        void Update(CompanyModel obj);
    }
}
