using BulkyBook.DataAccess.Repository.Generic;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository.CoverType
{
    public interface ICoverTypeRepository : IGenericRepository<CoverTypeModel>
    {
        void Update(CoverTypeModel obj);
    }
}
