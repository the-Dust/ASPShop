using DataAccess.Entities;
using Services.BuisnessLogic.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class NavController : Controller
    {
        private IProductTypeService productTypeService = null;

        public NavController(IProductTypeService productTypeService)
        {
            this.productTypeService = productTypeService;
        }

        public PartialViewResult Menu(string category = null)
        {
            ViewBag.SelectedCategory = category;

            IEnumerable < ProductType > menu = productTypeService.GetProductTypes().OrderBy(x=>x.Id);

            return PartialView(menu);
        }
    }
}