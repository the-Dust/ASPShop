using Services.BuisnessLogic.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class DefaultController : Controller
    {
        private IProductService productService = null;

        public DefaultController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var products = productService.GetProducts();
            ViewBag.Index = true;
            return View(products);
        }

        [HttpGet]
        public ActionResult Contacts()
        {
            ViewBag.Contacts = true;
            return View();
        }
    }
}