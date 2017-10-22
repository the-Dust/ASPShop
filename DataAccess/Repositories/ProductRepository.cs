using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;
using DataAccess.Repositories.Base;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    public class ProductRepository : AbstractRepository, IProductRepository
    {
        public IEnumerable<Product> GetProducts()
        {
            return context.Products.Include(x=>x.ProductType).ToArray();
        }

        public IEnumerable<Product> GetProducts(Expression<Func<Product, bool>> func)
        {
            return context.Products.Include(x => x.ProductType).Where(func).ToArray();
        }

        public Product GetProduct(int id)
        {
            return context.Products.Include(x => x.ProductType).FirstOrDefault(x=> x.Id==id);
        }

        public Product GetProduct(Expression<Func<Product, bool>> func)
        {
            return context.Products.Include(x => x.ProductType).FirstOrDefault(func);
        }

        public void RemoveProduct(int productId)
        {
            Product product = GetProduct(productId);
            context.Products.Remove(product);
            context.SaveChanges();
        }

        public void UpdateProduct(Product product)
        {
            Product oldProduct = GetProduct(product.Id);
            oldProduct.Cost = product.Cost;
            oldProduct.Description = product.Description;
            oldProduct.Name = product.Name;
            //oldProduct.ProductType = product.ProductType;
            oldProduct.ProductTypeId = product.ProductTypeId;

            context.SaveChanges();
        }

        public void AddProduct(Product product)
        {
            context.Products.Add(product);
            context.SaveChanges();
        }
    }
}
