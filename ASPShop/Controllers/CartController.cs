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

        public ViewResult Index(string returnUrl)
        {
            CartIndexViewModel model = new CartIndexViewModel { Cart = GetCart(), ReturnUrl = returnUrl };
            return View(model);
        }

        public Cart GetCart()
        {
            Cart cart = (Cart)Session["Cart"];
            if (cart == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }

        public RedirectToRouteResult AddToCart(int id, string returnUrl)
        {
            Product product = productService.GetProduct(id);

            if (product != null)
                GetCart().AddItem(product, 1);

            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(int productId, string returnUrl)
        {
            Product product = productService.GetProduct(productId);

            if (product != null)
                GetCart().RemoveLine(product);

            return RedirectToAction("Index", new { returnUrl });
        }
    }
}