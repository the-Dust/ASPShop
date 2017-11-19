using DataAccess.Entities;
using DataAccess.Entities.Base;
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
        private IOrderProcessor orderProcessor = null;

        public CartController(IProductService productService, IOrderProcessor orderProcessor)
        {
            this.productService = productService;
            this.orderProcessor = orderProcessor;
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

        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }

        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Извините, Ваша корзина пуста!");
            }

            if (ModelState.IsValid)
            {
                orderProcessor.ProcessOrder(cart, shippingDetails);
                cart.Clear();
                return View("Completed");
            }

            else
            {
                return View(shippingDetails);
            }
        }
    }
}