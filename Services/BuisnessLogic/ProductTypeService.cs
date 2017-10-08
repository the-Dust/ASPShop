using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;
using Services.BuisnessLogic.Base;
using DataAccess.Repositories.Base;

namespace Services.BuisnessLogic
{
    public class ProductTypeService : IProductTypeService
    {
        private IProductTypeRepository productTypeRepository = null;

        public ProductTypeService(IProductTypeRepository productTypeRepository)
        {
            this.productTypeRepository = productTypeRepository;
        }

        public IEnumerable<ProductType> GetProductTypes()
        {
            return productTypeRepository.GetProductTypes();
        }

        public IEnumerable<ProductType> GetProductTypes(Expression<Func<ProductType, bool>> func)
        {
            return productTypeRepository.GetProductTypes(func);
        }
    }
}
