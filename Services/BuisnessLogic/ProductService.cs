using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.BuisnessLogic.Base;
using DataAccess.Entities;
using System.Linq.Expressions;
using DataAccess.Repositories.Base;

namespace Services.BuisnessLogic
{
    public class ProductService : IProductService
    {
        private IProductRepository productRepository = null;

        public ProductService(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public IEnumerable<Product> GetProducts()
        {
            return productRepository.GetProducts();
        }

        public IEnumerable<Product> GetProducts(int productTypeId)
        {
            return productRepository.GetProducts().Where(x=>productTypeId==-1||x.ProductTypeId==productTypeId);
        }

        public IEnumerable<Product> GetProducts(Expression<Func<Product, bool>> func)
        {
            return productRepository.GetProducts(func);
        }

        public Product GetProduct(int id)
        {
            return productRepository.GetProduct(id);
        }

        public Product GetProduct(Expression<Func<Product, bool>> func)
        {
            return productRepository.GetProduct(func);
        }

        public void RemoveProduct(int productId)
        {
            productRepository.RemoveProduct(productId);
        }

        public void UpdateProduct(Product product)
        {
            productRepository.UpdateProduct(product);
        }
    }
}
