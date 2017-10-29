using Web.Models.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Threading;
using Web.Infrastructure.Base;

namespace Web.Controllers
{
    public class LoginController : Controller
    {
        IAuthProvider authProvider;

        public LoginController(IAuthProvider authProvider)
        {
            this.authProvider = authProvider;
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult LoginLink(string partial = "_LoginLink")
        {
            bool auth = System.Web.HttpContext.Current.User?.Identity.IsAuthenticated ?? false;

            if (auth)
            {
                var userName = Thread.CurrentPrincipal.Identity.Name;
                return PartialView(partial, new LoginLinkUser { IsLoggedIn = true, Login = userName });
            }
            return PartialView(partial, new LoginLinkUser { IsLoggedIn = false });
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginUser user, string returnUrl=null)
        {
            if (!string.IsNullOrWhiteSpace(user.UserName) &&
                !string.IsNullOrWhiteSpace(user.Password) &&
                ModelState.IsValid)
            {
                if (authProvider.Authenticate(user.UserName, user.Password))
                {
                    return Redirect(returnUrl ?? Url.Action("Index", "Admin"));
                }
                else
                {
                    ModelState.AddModelError("Enter", "Неправильный логин или пароль");
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult LoginModal(LoginUser user)
        {
            if (!string.IsNullOrWhiteSpace(user.UserName) &&
                !string.IsNullOrWhiteSpace(user.Password) &&
                ModelState.IsValid)
            {
                if (authProvider.Authenticate(user.UserName, user.Password))
                {
                    return View("_LoginClosePartial");
                }
                else
                {
                    ModelState.AddModelError("Enter", "Неправильный логин или пароль");
                    return PartialView("_LoginInsidePartial", user);
                }
            }
            else
            {
                return PartialView("_LoginInsidePartial", user);
            }
        }

        public ActionResult Logout()
        {
            authProvider.SignOut();

            return RedirectToAction("Index", "Default");
        }

        [HttpGet]
        public ActionResult GetLoginPartial()
        {
            return PartialView("../Default/Modal/_LoginPartial");
        }
    }
}