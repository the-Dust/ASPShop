using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class Cart
    {
        public IEnumerable<CartLine> Lines { get { return lineCollection; } }

        private List<CartLine> lineCollection = new List<CartLine>();

        public void AddItem(Product product, int quantity)
        {
            CartLine line = lineCollection.FirstOrDefault(x => x.Product.Id == product.Id);

            if (line == null)
            {
                lineCollection.Add(new CartLine { Product = product, Quantity = quantity });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public void RemoveLine(Product product)
        {
            lineCollection.RemoveAll(x=> x.Product.Id==product.Id);
        }

        public double ComputeTotalValue()
        {
            return lineCollection.Sum(x=>x.Product.Cost*x.Quantity);
        }

        public void Clear()
        {
            lineCollection.Clear();
        }
    }
}
