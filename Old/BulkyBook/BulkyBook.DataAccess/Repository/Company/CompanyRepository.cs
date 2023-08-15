using BulkyBook.DataAccess.Repository.Generic;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository.Company
{
    public class CompanyRepository : GenericRepository<CompanyModel>, ICompanyRepository
    {
        private ApplicationDBContext _db;
        public CompanyRepository(ApplicationDBContext db) : base(db)
        {
            _db = db;
        }

        void ICompanyRepository.Update(CompanyModel obj)
        {
            _db.Company.Update(obj);
        }
    }
}
