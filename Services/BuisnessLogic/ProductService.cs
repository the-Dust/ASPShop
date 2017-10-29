using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.BuisnessLogic.Base;
using DataAccess.Entities;
using System.Linq.Expressions;
using DataAccess.Repositories.Base;
using System.Web;

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

        public void AddProduct(Product product)
        {
            productRepository.AddProduct(product);
        }

        public IEnumerable<int> GetHistory(HttpRequestBase request, HttpResponseBase response)
        {
            HttpCookie cookie = request.Cookies["history"];
            Queue<int> history;
            if (cookie != null)
            {
                history = new Queue<int>(cookie.Value.Split(',').Select(int.Parse));
            }
            else
            {
                history = new Queue<int>();
                cookie = new HttpCookie("history");
            }

            if (!history.Contains(int.Parse(request.QueryString["Id"])))
            {
                if (history.Count >= 4) history.Dequeue();

                history.Enqueue(int.Parse(request.QueryString["Id"]));
            }

            cookie.Value = String.Join(",", history);
            cookie.Expires = DateTime.Now.AddDays(365);
            response.Cookies.Add(cookie);

            int take = history.Count > 3 ? 3 : history.Count - 1;
            return history.Take(take);
        }

        public IEnumerable<Product> GetRecommend(Product product)
        {
            var products = GetProducts(p => p.ProductTypeId == product.ProductTypeId&&p.Id!= product.Id);
            if (products.Count() > 3)
            {
                Random rnd = new Random((int)DateTime.Now.Ticks);
                int i = rnd.Next(0, products.Count() - 2);
                return products.Skip(i).Take(3);
            }
            else
                return products; 
        }
    }
}
