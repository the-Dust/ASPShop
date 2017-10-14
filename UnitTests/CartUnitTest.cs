using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataAccess.Entities;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests
{
    [TestClass]
    public class CartUnitTest
    {
        [TestMethod]
        public void CanAddNewLines()
        {
            Product product1 = new Product { Id = 1, ProductTypeId = 3, Name = "Product1", Cost = 1200.00 };
            Product product2 = new Product { Id = 2, ProductTypeId = 3, Name = "Product2", Cost = 1200.00 };

            Cart cart = new Cart();

            cart.AddItem(product1, 1);
            cart.AddItem(product2, 1);

            List<CartLine> list = cart.Lines.ToList();

            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(list[0].Product, product1);
            Assert.AreEqual(list[1].Product, product2);
        }

        [TestMethod]
        public void CanAddQuantityForExistingLines()
        {
            Product product1 = new Product { Id = 1, ProductTypeId = 3, Name = "Product1", Cost = 1200.00 };
            Product product2 = new Product { Id = 2, ProductTypeId = 3, Name = "Product2", Cost = 1200.00 };

            Cart cart = new Cart();

            cart.AddItem(product1, 3);
            cart.AddItem(product2, 1);
            cart.AddItem(product1, 2);

            List<CartLine> list = cart.Lines.ToList();

            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(list[0].Quantity, 5);
            Assert.AreEqual(list[1].Quantity, 1);
        }

        [TestMethod]
        public void CanRemoveLine()
        {
            Product product1 = new Product { Id = 1, ProductTypeId = 3, Name = "Product1", Cost = 1200.00 };
            Product product2 = new Product { Id = 2, ProductTypeId = 3, Name = "Product2", Cost = 1200.00 };
            Product product3 = new Product { Id = 3, ProductTypeId = 3, Name = "Product3", Cost = 1200.00 };

            Cart cart = new Cart();

            cart.AddItem(product1, 3);
            cart.AddItem(product2, 1);
            cart.AddItem(product3, 2);
            cart.AddItem(product2, 4);

            cart.RemoveLine(product2);

            Assert.AreEqual(0, cart.Lines.Where(x=>x.Product== product2).Count());
            Assert.AreEqual(2, cart.Lines.Count());
        }

        [TestMethod]
        public void CalculateCartTotal()
        {
            Product product1 = new Product { Id = 1, ProductTypeId = 3, Name = "Product1", Cost = 580.00 };
            Product product2 = new Product { Id = 2, ProductTypeId = 1, Name = "Product2", Cost = 1200.00 };
            Product product3 = new Product { Id = 3, ProductTypeId = 2, Name = "Product3", Cost = 930.00 };

            Cart cart = new Cart();

            cart.AddItem(product1, 3);
            cart.AddItem(product2, 1);
            cart.AddItem(product3, 2);
            cart.AddItem(product2, 4);

            Assert.AreEqual(9600, cart.ComputeTotalValue());
        }

        [TestMethod]
        public void CanClearContent()
        {
            Product product1 = new Product { Id = 1, ProductTypeId = 3, Name = "Product1", Cost = 580.00 };
            Product product2 = new Product { Id = 2, ProductTypeId = 1, Name = "Product2", Cost = 1200.00 };
            Product product3 = new Product { Id = 3, ProductTypeId = 2, Name = "Product3", Cost = 930.00 };

            Cart cart = new Cart();

            cart.AddItem(product1, 3);
            cart.AddItem(product2, 1);
            cart.AddItem(product3, 2);
            cart.AddItem(product2, 4);

            cart.Clear();

            Assert.AreEqual(0, cart.Lines.Count());
        }
    }
}
