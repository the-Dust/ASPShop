using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.BuisnessLogic.Base
{
    public interface IProductService
    {
        IEnumerable<Product> GetProducts();

        IEnumerable<Product> GetProducts(int productTypeId);

        IEnumerable<Product> GetProducts(Expression<Func<Product, bool>> func);

        Product GetProduct(int id);

        Product GetProduct(Expression<Func<Product, bool>> func);

        void RemoveProduct(int productId);

        void UpdateProduct(Product product);
    }
}
