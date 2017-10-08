using DataAccess.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services.BuisnessLogic.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Web.Controllers;
using Web.Models;
using Web.Models.HtmlHelpers;

namespace GameStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        Mock<IProductService> mock = new Mock<IProductService>();

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

        private void MockSetup()
        {
            mock.Setup(m => m.GetProducts()).Returns(list);

            mock.Setup(m => m.GetProducts(It.IsAny<int>())).Returns<int>(i=>i==0?list:list.Where(x =>x.ProductTypeId ==i));

        }

        public UnitTest1()
        {
            MockSetup();
        }

        [TestMethod]
        public void CanPaginate()
        {
            // Организация (arrange)
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            // Действие (act)
            ProductCatalogue result = (ProductCatalogue)((ViewResult)controller.GetCatalogue(0,3)).Model;

            // Утверждение (assert)
            List<Product> products = result.Products.ToList();
            Assert.IsTrue(products.Count == 3);
            Assert.AreEqual(products[0].Name, "Product7");
            Assert.AreEqual(products[1].Name, "Product8");
            Assert.AreEqual(products[2].Name, "Product9");
        }

        [TestMethod]
        public void CanGeneratePageLinks()
        {
            HtmlHelper myHelper = null;

            // Организация - создание объекта PagingInfo
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            // Организация - настройка делегата с помощью лямбда-выражения
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            // Действие
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            // Утверждение
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
                result.ToString());
        }

        [TestMethod]
        public void CanSendPaginationViewModel()
        {
            // Организация (arrange)
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            // Act
            ProductCatalogue result = (ProductCatalogue)((ViewResult)controller.GetCatalogue(productTypeId: 0, page: 2)).Model;

            // Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 9);
            Assert.AreEqual(pageInfo.TotalPages, 3);
        }

        [TestMethod]
        public void CanFilterProducts()
        {
            // Организация (arrange)
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 8;

            // Action
            List<Product> result = ((ProductCatalogue)((ViewResult)controller.GetCatalogue(2, 1)).Model).Products.ToList();

            // Assert
            Assert.AreEqual(result.Count(), 2);
            Assert.IsTrue(result[0].Name == "Product4" && result[0].ProductTypeId == 2);
            Assert.IsTrue(result[1].Name == "Product8" && result[1].ProductTypeId == 2);
        }
    }


}
