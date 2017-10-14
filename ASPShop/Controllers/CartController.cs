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
    public class CartController : Controller
    {
        private IProductService productService = null;

        public CartController(IProductService productService)
        {
            this.productService = productService;
        }

        public ViewResult Index(Cart cart, string returnUrl)
        {
            CartIndexViewModel model = new CartIndexViewModel { Cart = cart, ReturnUrl = returnUrl };
            return View(model);
        }

        public RedirectToRouteResult AddToCart(Cart cart, int id, string returnUrl)
        {
            Product product = productService.GetProduct(id);

            if (product != null)
                cart.AddItem(product, 1);

            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int productId, string returnUrl)
        {
            Product product = productService.GetProduct(productId);

            if (product != null)
                cart.RemoveLine(product);

            return RedirectToAction("Index", new { returnUrl });
        }

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }
    }
}