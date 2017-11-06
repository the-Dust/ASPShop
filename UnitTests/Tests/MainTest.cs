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
using NUnit;
using DataAccess.Repositories.Base;
using Services.BuisnessLogic;
using System.Reflection;
using System.Web;
using System.Linq.Expressions;
using System.IO;
using System.Net;

namespace UnitTests
{
    [TestClass]
    public class MainTest
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

        List<ProductType> listType = new List<ProductType>
            {
                new ProductType { Id=1, Name="Electric", Description="Струны для электрогитары" },
                new ProductType { Id=2, Name="Bass", Description="Струны для бас-гитары" },
                new ProductType { Id=3, Name="Acoustic", Description="Струны для акустической гитары" }
            };

        private void MockSetup(string str)
        {
            mock.Setup(m => m.GetProducts()).Returns(list);

            mock.Setup(m => m.GetProducts(It.IsAny<int>())).Returns<int>(i=>i==0?list:list.Where(x =>x.ProductTypeId ==i));

            mockType.Setup(m => m.GetProductTypeId(str)).Returns(listType.FirstOrDefault(y => y.Name == str)?.Id??-1);

            mockType.Setup(m=>m.GetProductTypes()).Returns(listType);
        }

        public MainTest()
        {
            MockSetup("");
        }

        [TestMethod]
        public void CanPaginate()
        {
            // Организация (arrange)
            ProductController controller = new ProductController(mock.Object, mockType.Object);
            controller.PageSize = 3;

            // Действие (act)
            ProductCatalogue result = (ProductCatalogue)((ViewResult)controller.GetCatalogue(null,3)).Model;

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
                + @"<a class=""btn btn-default  selected"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
                result.ToString());
        }

        [TestMethod]
        public void CanSendPaginationViewModel()
        {
            // Организация (arrange)
            ProductController controller = new ProductController(mock.Object, mockType.Object);
            controller.PageSize = 3;

            // Act
            ProductCatalogue result = (ProductCatalogue)((ViewResult)controller.GetCatalogue(category: null, page: 2)).Model;

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
            ProductController controller = new ProductController(mock.Object, mockType.Object);
            controller.PageSize = 20;

            string str = "Acoustic";
            MockSetup(str);

            // Action
            List<Product> result = ((ProductCatalogue)((ViewResult)controller.GetCatalogue(str, 1)).Model).Products.ToList();

            // Assert
            Assert.AreEqual(6, result.Count());
            Assert.IsTrue(result[0].Name == "Product1" && result[0].ProductTypeId == 3);
            Assert.IsTrue(result[1].Name == "Product2" && result[1].ProductTypeId == 3);
            Assert.IsTrue(result[2].Name == "Product3" && result[2].ProductTypeId == 3);
            Assert.IsTrue(result[3].Name == "Product5" && result[3].ProductTypeId == 3);
            Assert.IsTrue(result[4].Name == "Product6" && result[4].ProductTypeId == 3);
            Assert.IsTrue(result[5].Name == "Product9" && result[5].ProductTypeId == 3);
        }

        [TestMethod]
        public void CanCreateCategories()
        {
            NavController controller = new NavController(mockType.Object);

            List<ProductType> result = ((IEnumerable<ProductType>)controller.Menu().Model).ToList();

            Assert.AreEqual(result.Count, 3);
            Assert.AreEqual(result[0].Description, "Струны для электрогитары");
            Assert.AreEqual(result[1].Description, "Струны для бас-гитары");
            Assert.AreEqual(result[2].Description, "Струны для акустической гитары");
        }

        [TestMethod]
        public void IndicatesSelectedCategory()
        {
            NavController controller = new NavController(mockType.Object);

            string categoryToSelect = "Acoustic";

            string result = controller.Menu(categoryToSelect).ViewBag.SelectedCategory;

            Assert.AreEqual(categoryToSelect, result);
        }

        [TestMethod]
        public void GenerateSpecificCategoryCount()
        {
            ProductController controller = new ProductController(mock.Object, mockType.Object);

            controller.PageSize = 3;

            string str = "Electric";

            MockSetup(str);

            int res1 = ((ProductCatalogue)controller.GetCatalogue(str).Model).PagingInfo.TotalItems;

            str = "Acoustic";

            MockSetup(str);

            int res2 = ((ProductCatalogue)controller.GetCatalogue(str).Model).PagingInfo.TotalItems;

            str = "Bass";

            MockSetup(str);

            int res3 = ((ProductCatalogue)controller.GetCatalogue(str).Model).PagingInfo.TotalItems;
            int resAll = ((ProductCatalogue)controller.GetCatalogue(null).Model).PagingInfo.TotalItems;

            Assert.AreEqual(1, res1);
            Assert.AreEqual(res2, 6);
            Assert.AreEqual(res3, 2);
            Assert.AreEqual(resAll, 9);
        }

        [TestMethod]
        public void CanGetRecommend()
        {
            Mock<IProductRepository> mRepository = new Mock<IProductRepository>();

            mRepository.Setup(m => m.GetProducts()).Returns(list);
            mRepository.Setup(m => m.GetProducts(It.IsAny<Expression<Func<Product, bool>>>())).Returns<Expression<Func<Product, bool>>>(e=> list.Where(e.Compile()));
            mRepository.Setup(m => m.GetProduct(It.IsAny<int>())).Returns<int>(i => list.FirstOrDefault(x => x.Id == i));

            IProductService service = new ProductService(mRepository.Object);

            Product product1 = new Product { Id = 1, ProductTypeId = 3 };
            Product product4 = new Product { Id = 4, ProductTypeId = 2 };
            Product product7 = new Product { Id = 7, ProductTypeId = 1 };

            var result1 = service.GetRecommend(product1);
            var result4 = service.GetRecommend(product4);
            var result7 = service.GetRecommend(product7).ToArray();

            Assert.AreEqual(3, result1.Count());
            Assert.AreEqual(result1.Distinct().Count(), result1.Count());
            Assert.IsTrue(result1.All(x => x.ProductTypeId == 3));
            Assert.IsFalse(result1.Contains(product1));

            Assert.AreEqual(1, result4.Count());
            Assert.AreEqual(result4.Distinct().Count(), result4.Count());
            Assert.IsTrue(result4.All(x => x.ProductTypeId == 2));
            Assert.IsFalse(result1.Contains(product4));

            Assert.AreEqual(0, result7.Count());
            Assert.AreEqual(result7.Distinct().Count(), result7.Count());
            Assert.IsTrue(result7.All(x => x.ProductTypeId == 1));
            Assert.IsFalse(result1.Contains(product7));
        }

        [TestMethod]
        public void CanGetHistory()
        {
            ProductServiceGetHistoryMock mock = new ProductServiceGetHistoryMock();

            string test = string.Join(", ", mock.GetHistory(1));

            Assert.AreEqual("", test); //Cookie: 1
            Assert.AreEqual("1", mock.Cookie);

            test = string.Join(", ", mock.GetHistory(2));

            Assert.AreEqual("1", test);  //Cookie: 1, 2
            Assert.AreEqual("1, 2", mock.Cookie);

            test = string.Join(", ", mock.GetHistory(2));

            Assert.AreEqual("1", test);  //Cookie: 1, 2
            Assert.AreEqual("1, 2", mock.Cookie);

            test = string.Join(", ", mock.GetHistory(3));

            Assert.AreEqual("1, 2", test); //Cookie: 1, 2, 3
            Assert.AreEqual("1, 2, 3", mock.Cookie);

            test = string.Join(", ", mock.GetHistory(1));

            Assert.AreEqual("1, 2, 3", test); //Cookie: 1, 2, 3
            Assert.AreEqual("1, 2, 3", mock.Cookie);

            test = string.Join(", ", mock.GetHistory(4));

            Assert.AreEqual("1, 2, 3", test); //Cookie: 1, 2, 3, 4
            Assert.AreEqual("1, 2, 3, 4", mock.Cookie);

            test = string.Join(", ", mock.GetHistory(5));

            Assert.AreEqual("2, 3, 4", test); //Cookie: 1, 2, 3, 4, 5
            Assert.AreEqual("1, 2, 3, 4, 5", mock.Cookie);

            test = string.Join(", ", mock.GetHistory(10));

            Assert.AreEqual("3, 4, 5", test); //Cookie: 1, 2, 3, 4, 5
            Assert.AreEqual("1, 2, 3, 4, 5", mock.Cookie);

            test = string.Join(", ", mock.GetHistory(7));

            Assert.AreEqual("3, 4, 5", test); //Cookie: 1, 2, 3, 4, 5, 7
            Assert.AreEqual("1, 2, 3, 4, 5, 7", mock.Cookie);

            test = string.Join(", ", mock.GetHistory(8));

            Assert.AreEqual("4, 5, 7", test); //Cookie: 1, 2, 3, 4, 5, 7, 8
            Assert.AreEqual("1, 2, 3, 4, 5, 7, 8", mock.Cookie);

            //emulation of removing some products from db
            mock.ProductIds = new[] { 1, 2, 3, 4 };

            test = string.Join(", ", mock.GetHistory(1));

            //not 5,7,8
            Assert.AreEqual("2, 3, 4", test); //Cookie: 1, 2, 3, 4, 5, 7, 8, 1
            Assert.AreEqual("1, 2, 3, 4, 5, 7, 8, 1", mock.Cookie);

            test = string.Join(", ", mock.GetHistory(4));

            Assert.AreEqual("3, 4, 1", test);
            Assert.AreEqual("1, 2, 3, 4, 5, 7, 8, 1", mock.Cookie);

            test = string.Join(", ", mock.GetHistory(2));

            Assert.AreEqual("3, 4, 1", test);
            Assert.AreEqual("1, 2, 3, 4, 5, 7, 8, 1, 2", mock.Cookie);

            
            mock.ProductIds = new[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            test = string.Join(", ", mock.GetHistory(2));

            Assert.AreEqual("7, 8, 1", test);
            Assert.AreEqual("1, 2, 3, 4, 5, 7, 8, 1, 2", mock.Cookie);

            test = string.Join(", ", mock.GetHistory(1));

            Assert.AreEqual("8, 1, 2", test);
            Assert.AreEqual("1, 2, 3, 4, 5, 7, 8, 1, 2", mock.Cookie);

            test = string.Join(", ", mock.GetHistory(7));

            Assert.AreEqual("8, 1, 2", test);
            Assert.AreEqual("1, 2, 3, 4, 5, 7, 8, 1, 2, 7", mock.Cookie);

            test = string.Join(", ", mock.GetHistory(6));

            Assert.AreEqual("1, 2, 7", test);
            Assert.AreEqual("2, 3, 4, 5, 7, 8, 1, 2, 7, 6", mock.Cookie);

            mock.ProductIds = new[] { 2, 8, 10 };

            test = string.Join(", ", mock.GetHistory(10));

            Assert.AreEqual("8, 2", test);
            Assert.AreEqual("3, 4, 5, 7, 8, 1, 2, 7, 6, 10", mock.Cookie);

            test = string.Join(", ", mock.GetHistory(2));

            Assert.AreEqual("8, 2, 10", test);
            Assert.AreEqual("3, 4, 5, 7, 8, 1, 2, 7, 6, 10", mock.Cookie);

        }


        
    }

    public class ProductServiceGetHistoryMock
    {
        //This method mocks IProductService.GetHistory() method
        public string Cookie { get; set; }
        public int[] ProductIds { get; set; } = new[] { 1, 2, 3, 4, 5, 6, 7, 8 };

        public IEnumerable<int> GetHistory(int productId)
        {
            Queue<int> history;
            if (Cookie != null)
            {
                history = new Queue<int>(Cookie.Split(',').Select(int.Parse));
            }
            else
            {
                history = new Queue<int>();
                Cookie = "";
            }

            //deleting inactual items, then deleting repetitive items from the end that's why double reverse() used 
            var outputHistory = history.Where(x => ProductIds.Contains(x)).Reverse().Distinct().Reverse().ToArray();
            int itemsToSkip = outputHistory.Count() > 3 ? outputHistory.Count() - 3 : 0;

            if (ProductIds.Contains(productId) && !outputHistory.Skip(itemsToSkip).Contains(productId))
            {
                if (history.Count >= 10) history.Dequeue();

                history.Enqueue(productId);
            }

            Cookie = String.Join(", ", history);

            
            itemsToSkip = outputHistory.Count() > 3 ? outputHistory.Count() - 3 : 0;
            int step = outputHistory.LastOrDefault() == productId ? 1 : 0;
            int take = itemsToSkip - step == -1 ? outputHistory.Count() - 1 : 3;
            return outputHistory.Skip(itemsToSkip - step).Take(take);
        }
    }
}
