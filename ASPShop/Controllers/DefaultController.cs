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
        private IUserService userService = null;
        private IRoleService roleService = null;
        private IGroupService groupService = null;

        public DefaultController(IUserService userService, 
                                IRoleService roleService, IGroupService groupService)
        {
            this.userService = userService;
            this.roleService = roleService;
            this.groupService = groupService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetUsers()
        {
            var result = userService.GetAllUsers();
            return Json(result.ToArray(), JsonRequestBehavior.AllowGet);
        }
    }
}