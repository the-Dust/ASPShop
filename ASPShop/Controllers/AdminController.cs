using DataAccess.Entities;
using Services.BuisnessLogic.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    [Authorize(Users ="admin")]
    public class AdminController : Controller
    {
        private IProductService productService = null;
        private IProductTypeService productTypeService = null;

        public AdminController(IProductService productService, IProductTypeService productTypeService)
        {
            this.productService = productService;
            this.productTypeService = productTypeService;
        }

        public ViewResult Index(int page=1)
        {
            int pageSize = 4;

            var tempProducts = productService.GetProducts();

            var products = tempProducts.OrderBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize);

            PagingInfo pagingInfo = new PagingInfo { CurrentPage = page, ItemsPerPage = pageSize, TotalItems = tempProducts.Count() };

            ProductCatalogue model = new ProductCatalogue { Products = products, PagingInfo = pagingInfo};

            return View(model);
        }

        
        public PartialViewResult Edit(int productId)
        {
            return PartialView("Modal/_Edit", productId);
        }

        public PartialViewResult GetProduct(int productId)
        {
            var product = productService.GetProduct(productId);

            ViewBag.Types = productTypeService.GetProductTypes().Select(x => x.Name);

            return PartialView("Modal/_ProductPartial", product);
        }

        [HttpPost]
        //public ActionResult UpdateProduct([Bind(Exclude = "Id")] Product product)
        public ActionResult UpdateProduct(Product product)
        {
            ViewBag.Types = productTypeService.GetProductTypes().Select(x => x.Name);

            if (ModelState.IsValid)
            {
                if (product.Id == 0)
                {
                    product.ImageLink = "noimg.jpg";
                    productService.AddProduct(product);
                }

                productService.UpdateProduct(product);

                TempData["message"] = string.Format($"Изменения в товаре \"{product.Name}\" были сохранены");

            }
            return PartialView("Modal/_ProductPartial", product);
        }

        [HttpPost]
        public JsonResult RemoveProduct(int productId)
        {
            productService.RemoveProduct(productId);
            return Json(new { IsRemoved = true }, JsonRequestBehavior.AllowGet);
        }
        //noimg.jpg

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase upload, int productId, int currentPage)
        {
            ViewBag.Types = productTypeService.GetProductTypes().Select(x => x.Name);

            if (upload != null)
            {
                string fileName = System.IO.Path.GetFileName(upload.FileName);

                upload.SaveAs(Server.MapPath("~/Content/Images/" + fileName));

                var product = productService.GetProduct(productId);

                product.ImageLink = fileName;

                productService.UpdateProduct(product);
            }

            return RedirectToAction("Index", new {page = currentPage });
        }


    }
}