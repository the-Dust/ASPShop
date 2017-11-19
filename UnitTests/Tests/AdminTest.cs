using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services.BuisnessLogic.Base;
using DataAccess.Entities;
using System.Collections.Generic;
using Web.Controllers;
using System.Web.Mvc;
using System.Linq;
using Web.Models;

namespace UnitTests
{
    [TestClass]
    public class AdminTest
    {
        [TestMethod]
        public void IndexContainsAllProducts()
        {
            Mock<IProductService> mock = new Mock<IProductService>();

            mock.Setup(m => m.GetProducts()).Returns(new List<Product>
            {
                new Product { Id=1, ProductTypeId=3, Name="Product1", Cost=1200.00},
                new Product { Id=2, ProductTypeId=3, Name="Product2", Cost=1200.00},
                new Product { Id=3, ProductTypeId=3, Name="Product3", Cost=1200.00},
                new Product { Id=4, ProductTypeId=2, Name="Product4", Cost=1200.00},
                new Product { Id=5, ProductTypeId=3, Name="Product5", Cost=1200.00},
                new Product { Id=6, ProductTypeId=3, Name="Product6", Cost=1200.00},
                new Product { Id=7, ProductTypeId=1, Name="Product7", Cost=1200.00},
                new Product { Id=8, ProductTypeId=2, Name="Product8", Cost=1200.00},
                new Product { Id=9, ProductTypeId=3, Name="Product9", Cost=1200.00},
            });

            AdminController controller = new AdminController(mock.Object, null) { PageSize=9};

            List<Product> result = ((ProductCatalogue)controller.Index().ViewData.Model).Products.ToList();

            Assert.AreEqual(9, result.Count);
            Assert.AreEqual("Product1", result[0].Name);
            Assert.AreEqual("Product2", result[1].Name);
            Assert.AreEqual("Product3", result[2].Name);

        }

        [TestMethod]
        public void CanEditProduct()
        {
            Mock<IProductService> mock = new Mock<IProductService>();
            Mock<IProductTypeService> mockType = new Mock<IProductTypeService>();

            List<Product> list = new List<Product>
            {
                new Product { Id=1, ProductTypeId=3, Name="Product1", Cost=1200.00},
                new Product { Id=2, ProductTypeId=3, Name="Product2", Cost=1200.00},
                new Product { Id=3, ProductTypeId=3, Name="Product3", Cost=1200.00},
                new Product { Id=4, ProductTypeId=2, Name="Product4", Cost=1200.00},
                new Product { Id=5, ProductTypeId=3, Name="Product5", Cost=1200.00},
                new Product { Id=6, ProductTypeId=3, Name="Product6", Cost=1200.00},
                new Product { Id=7, ProductTypeId=1, Name="Product7", Cost=1200.00},
                new Product { Id=8, ProductTypeId=2, Name="Product8", Cost=1200.00},
                new Product { Id=9, ProductTypeId=3, Name="Product9", Cost=1200.00},
            };

            mock.Setup(m => m.GetProducts()).Returns(list);
            mockType.Setup(m => m.GetProductTypes()).Returns(new ProductType[] { new ProductType() { Name="", Description = "" } });

            mock.Setup(m => m.GetProduct(It.IsAny<int>())).Returns<int>(x => list.FirstOrDefault(p => p.Id == x));

            AdminController controller = new AdminController(mock.Object, mockType.Object);

            Product p1 = ((PartialViewResult)controller.GetProduct(1)).Model as Product;
            Product p2 = ((PartialViewResult)controller.GetProduct(2)).Model as Product;
            Product p3 = ((PartialViewResult)controller.GetProduct(3)).Model as Product;

            Assert.AreEqual(1, p1.Id);
            Assert.AreEqual(2, p2.Id);
            Assert.AreEqual(3, p3.Id);

        }

        [TestMethod]
        public void CannotEditNonexistentProduct()
        {
            Mock<IProductService> mock = new Mock<IProductService>();
            Mock<IProductTypeService> mockType = new Mock<IProductTypeService>();

            List<Product> list = new List<Product>
            {
                new Product { Id=1, ProductTypeId=3, Name="Product1", Cost=1200.00},
                new Product { Id=2, ProductTypeId=3, Name="Product2", Cost=1200.00},
                new Product { Id=3, ProductTypeId=3, Name="Product3", Cost=1200.00},
                new Product { Id=4, ProductTypeId=2, Name="Product4", Cost=1200.00},
                new Product { Id=5, ProductTypeId=3, Name="Product5", Cost=1200.00},
                new Product { Id=6, ProductTypeId=3, Name="Product6", Cost=1200.00},
                new Product { Id=7, ProductTypeId=1, Name="Product7", Cost=1200.00},
                new Product { Id=8, ProductTypeId=2, Name="Product8", Cost=1200.00},
                new Product { Id=9, ProductTypeId=3, Name="Product9", Cost=1200.00},
            };

            mock.Setup(m => m.GetProducts()).Returns(list);
            mockType.Setup(m => m.GetProductTypes()).Returns(new ProductType[] { new ProductType() { Name = "", Description = "" } });
            mock.Setup(m => m.GetProduct(It.IsAny<int>())).Returns<int>(x => list.FirstOrDefault(p => p.Id == x));

            AdminController controller = new AdminController(mock.Object, mockType.Object);

            Product p1 = ((PartialViewResult)controller.Edit(10)).Model as Product;

            Assert.AreEqual(null, p1);
        }

        [TestMethod]
        public void CanSaveValidChanges()
        {
            Mock<IProductService> mock = new Mock<IProductService>();
            Mock<IProductTypeService> mockType = new Mock<IProductTypeService>();

            AdminController controller = new AdminController(mock.Object, mockType.Object);

            Product p1 = new Product { Name="Test"};

            ActionResult result = controller.UpdateProduct(p1);


            mock.Verify(m => m.UpdateProduct(p1));

            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }

        [TestMethod]
        public void CannotSaveInvalidChanges()
        {
            Mock<IProductService> mock = new Mock<IProductService>();
            Mock<IProductTypeService> mockType = new Mock<IProductTypeService>();

            AdminController controller = new AdminController(mock.Object, mockType.Object);

            Product p1 = new Product { Name = "Test" };

            controller.ModelState.AddModelError("error", "error");

            ActionResult result = controller.UpdateProduct(p1);

            mock.Verify(m => m.UpdateProduct(It.IsAny<Product>()), Times.Never);

            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }

        [TestMethod]
        public void CanAddNewProduct()
        {
            Mock<IProductService> mock = new Mock<IProductService>();
            Mock<IProductTypeService> mockType = new Mock<IProductTypeService>();

            AdminController controller = new AdminController(mock.Object, mockType.Object);

            Product p1 = new Product { Name = "Test" };

            ActionResult result = controller.AddProduct(p1);


            mock.Verify(m => m.UpdateProduct(p1));

            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }

        [TestMethod]
        public void CanDeleteValidProducts()
        {
            Mock<IProductService> mock = new Mock<IProductService>();
            Mock<IProductTypeService> mockType = new Mock<IProductTypeService>();

            List<Product> list = new List<Product>
            {
                new Product { Id=1, ProductTypeId=3, Name="Product1", Cost=1200.00},
                new Product { Id=2, ProductTypeId=3, Name="Product2", Cost=1200.00},
                new Product { Id=3, ProductTypeId=3, Name="Product3", Cost=1200.00},
                new Product { Id=4, ProductTypeId=2, Name="Product4", Cost=1200.00},
                new Product { Id=5, ProductTypeId=3, Name="Product5", Cost=1200.00},
                new Product { Id=6, ProductTypeId=3, Name="Product6", Cost=1200.00},
                new Product { Id=7, ProductTypeId=1, Name="Product7", Cost=1200.00},
                new Product { Id=8, ProductTypeId=2, Name="Product8", Cost=1200.00},
                new Product { Id=9, ProductTypeId=3, Name="Product9", Cost=1200.00},
            };

            AdminController controller = new AdminController(mock.Object, mockType.Object);

            controller.RemoveProduct(2);

            mock.Verify(m => m.RemoveProduct(2), Times.Once);
        }
    }
}
