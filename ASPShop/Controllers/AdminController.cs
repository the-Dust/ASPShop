using DataAccess.Entities;
using Services.BuisnessLogic.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    [Authorize()]
    public class AdminController : Controller
    {
        private IProductService productService = null;
        private IProductTypeService productTypeService = null;

        public AdminController(IProductService productService, IProductTypeService productTypeService)
        {
            this.productService = productService;
            this.productTypeService = productTypeService;
        }

        public ViewResult Index()
        {
            var products = productService.GetProducts();

            return View(products);
        }

        [HttpGet]
        public ActionResult Edit(int productId)
        {
            var product = productService.GetProduct(productId);

            ViewBag.Types = productTypeService.GetProductTypes().Select(x => x.Name);

            return PartialView("Modal/_ProductPartial", product);
        }

        [HttpPost]
        public JsonResult UpdateProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                if (product.Id == 0)
                {
                    productService.AddProduct(product);
                }

                productService.UpdateProduct(product);

                return Json(new { IsSaved = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { IsSaved = false }, JsonRequestBehavior.AllowGet);
        }
    }
}