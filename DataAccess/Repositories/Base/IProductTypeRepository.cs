using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;
using System.Linq.Expressions;

namespace DataAccess.Repositories.Base
{
    public interface IProductTypeRepository
    {
        IEnumerable<ProductType> GetProductTypes();

        IEnumerable<ProductType> GetProductTypes(Expression<Func<ProductType, bool>> func);

        int GetProductTypeId(ref string category);
    }
}
