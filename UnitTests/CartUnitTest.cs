using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataAccess.Entities;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Services.BuisnessLogic.Base;
using Web.Controllers;
using System.Web.Mvc;
using Web.Models;

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

            Assert.AreEqual(0, cart.Lines.Where(x => x.Product == product2).Count());
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

        [TestMethod]
        public void CanAddToCart()
        {
            Mock<IProductService> mock = new Mock<IProductService>();

            mock.Setup(x => x.GetProduct(It.IsAny<int>())).Returns(
            new Product { Id = 1, ProductTypeId = 3, Name = "Product1", Cost = 580.00 });

            Cart cart = new Cart();

            CartController controller = new CartController(mock.Object);

            controller.AddToCart(cart, 1, null);

            Assert.AreEqual(1, cart.Lines.Count());
            Assert.AreEqual(1, cart.Lines.ToList()[0].Product.Id);
        }

        [TestMethod]
        public void AddingProductToCartGoesToCartScreen()
        {
            Mock<IProductService> mock = new Mock<IProductService>();

            mock.Setup(x => x.GetProduct(It.IsAny<int>())).Returns(
            new Product { Id = 1, ProductTypeId = 3, Name = "Product1", Cost = 580.00 });

            Cart cart = new Cart();

            CartController controller = new CartController(mock.Object);

            RedirectToRouteResult result = controller.AddToCart(cart, 1, "myUrl");

            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("myUrl", result.RouteValues["returnUrl"]);
        }

        [TestMethod]
        public void CanViewCartContents()
        {
            Cart cart = new Cart();

            CartController controller = new CartController(null);

            CartIndexViewModel result = (CartIndexViewModel)controller.Index(cart, "myUrl").ViewData.Model;

            Assert.AreSame(cart, result.Cart);
            Assert.AreEqual("myUrl", result.ReturnUrl);
        }
    }
}
