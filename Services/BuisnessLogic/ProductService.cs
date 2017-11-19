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

            int productId = -1;
            int.TryParse(request.QueryString["productId"], out productId);

            var productIds = GetProducts().Select(p => p.Id);

            //deleting inactual items, then deleting repetitive items from the end that's why double reverse() used 
            var outputHistory = history.Where(x => productIds.Contains(x)).Reverse().Distinct().Reverse().ToArray();
            int itemsToSkip = outputHistory.Count() > 3 ? outputHistory.Count() - 3 : 0;

            if (productIds.Contains(productId) && !outputHistory.Skip(itemsToSkip).Contains(productId))
            {
                //depth of history=10
                if (history.Count >= 10) history.Dequeue();

                history.Enqueue(productId);
            }

            cookie.Value = String.Join(",", history);
            cookie.Expires = DateTime.Now.AddDays(365);
            response.Cookies.Add(cookie);

            itemsToSkip = outputHistory.Count() > 3 ? outputHistory.Count() - 3 : 0;
            int step = outputHistory.LastOrDefault() == productId ? 1 : 0;
            int take = itemsToSkip - step == -1 ? outputHistory.Count() - 1 : 3;
            return outputHistory.Skip(itemsToSkip - step).Take(take);
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
