﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Web.Infrastructure.Base;
using Web.Models.Login;
using Web.Controllers;
using System.Web.Mvc;

namespace UnitTests
{
    [TestClass]
    public class LoginTest
    {
        [TestMethod]
        public void CanLoginWithValidCredentials()
        {
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();

            mock.Setup(m => m.Authenticate("admin", "12345")).Returns(true);

            LoginUser user = new LoginUser { UserName = "admin", Password = "12345" };

            LoginController controller = new LoginController(mock.Object);

            ActionResult result = controller.Login(user, "/url");

            Assert.IsInstanceOfType(result, typeof(RedirectResult));
            Assert.AreEqual("/url", ((RedirectResult)result).Url);
        }

        [TestMethod]
        public void CannotLoginWithInvalidCredentials()
        {
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();

            mock.Setup(m => m.Authenticate("badlogin", "badpass")).Returns(false);

            LoginUser user = new LoginUser { UserName = "badlogin", Password = "badpass" };

            LoginController controller = new LoginController(mock.Object);

            ActionResult result = controller.Login(user, "/url");

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsFalse(((ViewResult)result).ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void CanLoginWithValidCredentialsModal()
        {
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();

            mock.Setup(m => m.Authenticate("admin", "12345")).Returns(true);

            LoginUser user = new LoginUser { UserName = "admin", Password = "12345" };

            LoginController controller = new LoginController(mock.Object);

            ActionResult result = controller.LoginModal(user);

            Assert.IsInstanceOfType(result, typeof(PartialViewResult));

            Assert.AreEqual("_LoginClosePartial", ((PartialViewResult)result).ViewName);
        }
        
        [TestMethod]
        public void CannotLoginWithInvalidCredentialsModal()
        {
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();

            mock.Setup(m => m.Authenticate("badlogin", "badpass")).Returns(false);

            LoginUser user = new LoginUser { UserName = "badlogin", Password = "badpass" };

            LoginController controller = new LoginController(mock.Object);

            ActionResult result = controller.LoginModal(user);

            Assert.IsInstanceOfType(result, typeof(PartialViewResult));

            Assert.AreEqual("_LoginInsidePartial", ((PartialViewResult)result).ViewName);

            Assert.IsFalse(((PartialViewResult)result).ViewData.ModelState.IsValid);
        }
    }
}
