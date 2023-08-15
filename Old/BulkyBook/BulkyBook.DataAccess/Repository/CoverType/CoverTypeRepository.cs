using BulkyBook.DataAccess.Repository.Generic;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository.CoverType
{
    public class CoverTypeRepository : GenericRepository<CoverTypeModel>, ICoverTypeRepository
    {
        private ApplicationDBContext _db;
        public CoverTypeRepository(ApplicationDBContext db) : base(db)
        {
            _db = db;
        }

        void ICoverTypeRepository.Update(CoverTypeModel obj)
        {
            _db.CoverType.Update(obj);
        }
    }
}
