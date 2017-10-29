using Services.BuisnessLogic.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using DataAccess.Entities;
using Web.Models;

namespace Web.Controllers
{

    public class ProductController : Controller
    {
        private IProductService productService = null;
        private IProductTypeService productTypeService = null;

        public int PageSize { get; set; } = 4;

        public ProductController(IProductService productService, IProductTypeService productTypeService)
        {
            this.productService = productService;
            this.productTypeService = productTypeService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var products = productService.GetProducts();

            return View(products);
        }

        [HttpGet]
        public ViewResult GetCatalogue(string category=null, int page=1)
        {
            int productTypeId = productTypeService.GetProductTypeId(ref category);

            var tempProducts = productService.GetProducts(productTypeId);

            var products = tempProducts.OrderBy(x => x.Id).Skip((page - 1) * PageSize).Take(PageSize);

            PagingInfo pagingInfo = new PagingInfo { CurrentPage = page, ItemsPerPage = PageSize, TotalItems = tempProducts.Count() };

            ProductCatalogue model = new ProductCatalogue { Products=products, PagingInfo=pagingInfo, CurrentCategory = category };

            return View(model);
        }

        [HttpGet]
        public ActionResult GetConcreteProduct(Product product)
        {
            return PartialView(product);
        }

        [HttpGet]
        public ActionResult GetConcreteProductById(int productId)
        {
            var product = productService.GetProduct(productId);

            return PartialView("GetConcreteProduct", product);
        }

        [HttpGet]
        public ActionResult GetProductInfo(int productId)
        {
            var product = productService.GetProduct(productId);

            ViewBag.History = productService.GetHistory(Request, Response, productId);

            ViewBag.Recommend = productService.GetRecommend(product);

            return View(product);
        }

        [HttpGet]
        public JsonResult RemoveProduct(int productId)
        {
            productService.RemoveProduct(productId);
            return Json(new { IsRemoved = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                productService.UpdateProduct(product);

                return Json(new { IsSaved = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { IsSaved = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetProductPartial(int productId)
        {
            var product = productService.GetProduct(productId);

            return PartialView("Modal/_ProductPartial", product);
        }
    }
}