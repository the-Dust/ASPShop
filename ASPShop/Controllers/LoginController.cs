using Web.Models.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Threading;

namespace Web.Controllers
{
    public class LoginController : Controller
    {
        //[HttpGet]
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult LoginLink()
        {
            //var cookie = FormsAuthentication.GetAuthCookie(FormsAuthentication.FormsCookieName, true);

            var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            if (cookie != null)
            {
                var userName = Thread.CurrentPrincipal.Identity.Name;
                return PartialView("_LoginLink", new LoginLinkUser { IsLoggedIn = true, Login = userName });
            }
            return PartialView("_LoginLink", new LoginLinkUser { IsLoggedIn = false });
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginUser user)
        {
            if (!string.IsNullOrWhiteSpace(user.UserName) && 
                !string.IsNullOrWhiteSpace(user.Password) &&
                ModelState.IsValid)
            {
                FormsAuthentication.SetAuthCookie(user.UserName, true);
                return RedirectToAction("Index", "Default");
            }    
            return View();
        }

        [HttpPost]
        public ActionResult LoginModal(LoginUser user)
        {
            if (!string.IsNullOrWhiteSpace(user.UserName) &&
                !string.IsNullOrWhiteSpace(user.Password) &&
                ModelState.IsValid)
            {
                FormsAuthentication.SetAuthCookie(user.UserName, true);

                return Json(new { IsLoggedIn = true }, JsonRequestBehavior.AllowGet);
            }
            return PartialView("../Default/Modal/_LoginPartial", user); 
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Default");
        }

        [HttpGet]
        public ActionResult GetLoginPartial()
        {
            return PartialView("../Default/Modal/_LoginPartial");
        }
    }
}