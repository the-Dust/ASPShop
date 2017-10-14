using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;
using DataAccess.Repositories.Base;

namespace DataAccess.Repositories
{
    public class ProductTypeRepository : AbstractRepository, IProductTypeRepository
    {
        public IEnumerable<ProductType> GetProductTypes()
        {
            return context.ProductTypes.ToArray();
        }

        public IEnumerable<ProductType> GetProductTypes(Expression<Func<ProductType, bool>> func)
        {
            return context.ProductTypes.Where(func).ToArray();
        }

        public int GetProductTypeId(ref string category)
        {
            string temp = category;

            var productType = context.ProductTypes.FirstOrDefault(x => x.Name == temp);

            if (productType == null)
            {
                category = "Catalogue";
                return -1;
            }

            return productType.Id;
        }
    }
}
