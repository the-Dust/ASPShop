﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using DataAccess.Repositories.Base;
using Services.BuisnessLogic.Base;
using DataAccess.Repositories;
using Services.BuisnessLogic;
using DataAccess.Entities;
using System.Configuration;
using DataAccess.Entities.Base;
using Web.Infrastructure.Base;

namespace Web.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private readonly IKernel kernel;

        public NinjectControllerFactory()
        {
            kernel = new StandardKernel();
            AddBindings();
        }

        public void AddBindings()
        {
            kernel.Bind<IUserRepository>().To<UserRepository>();
            kernel.Bind<IGroupRepository>().To<GroupRepository>();
            kernel.Bind<IRoleRepository>().To<RoleRepository>();
            kernel.Bind<IProductRepository>().To<ProductRepository>();
            kernel.Bind<IProductTypeRepository>().To<ProductTypeRepository>();

            kernel.Bind<IUserService>().To<UserService>();
            kernel.Bind<IGroupService>().To<GroupService>();
            kernel.Bind<IRoleService>().To<RoleService>();
            kernel.Bind<IProductService>().To<ProductService>();
            kernel.Bind<IProductTypeService>().To<ProductTypeService>();

            EmailSettings emailSettings = new EmailSettings
            {
                WriteAsFile = bool.Parse(ConfigurationManager
                    .AppSettings["Email.WriteAsFile"] ?? "false")
            };

            kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>()
                .WithConstructorArgument("settings", emailSettings);

            kernel.Bind<IAuthProvider>().To<FormAuthProvider>();
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType!=null? (IController)kernel.Get(controllerType): null;
        }
    }
}