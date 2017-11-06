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
        public int PageSize { get; set; } = 10; 

        public AdminController(IProductService productService, IProductTypeService productTypeService)
        {
            this.productService = productService;
            this.productTypeService = productTypeService;
        }

        public ViewResult Index(int page=1)
        {
            var tempProducts = productService.GetProducts();

            var products = tempProducts.OrderBy(x => x.Id).Skip((page - 1) * PageSize).Take(PageSize);

            PagingInfo pagingInfo = new PagingInfo { CurrentPage = page, ItemsPerPage = PageSize, TotalItems = tempProducts.Count() };

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

            ViewBag.Types = productTypeService.GetProductTypes().Select(x => x.Description);

            return PartialView("Modal/_ProductPartial", product);
        }

        [HttpPost]
        public ActionResult UpdateProduct(Product product)
        {
            ViewBag.Types = productTypeService.GetProductTypes().Select(x => x.Description);

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
        public ActionResult AddProduct([Bind(Exclude = "Id")] Product product)
        {
            return UpdateProduct(product);
        }

        [HttpPost]
        public JsonResult RemoveProduct(int productId)
        {
            productService.RemoveProduct(productId);
            return Json(new { IsRemoved = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase upload, int productId, int currentPage)
        {
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