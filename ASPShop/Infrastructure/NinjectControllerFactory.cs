using System;
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
        public IKernel Kernel { get; private set; }

        public NinjectControllerFactory()
        {
            Kernel = new StandardKernel();
            AddBindings();
        }

        public void AddBindings()
        {
            Kernel.Bind<IUserRepository>().To<UserRepository>();
            Kernel.Bind<IGroupRepository>().To<GroupRepository>();
            Kernel.Bind<IRoleRepository>().To<RoleRepository>();
            Kernel.Bind<IProductRepository>().To<ProductRepository>();
            Kernel.Bind<IProductTypeRepository>().To<ProductTypeRepository>();

            Kernel.Bind<IUserService>().To<UserService>();
            Kernel.Bind<IGroupService>().To<GroupService>();
            Kernel.Bind<IRoleService>().To<RoleService>();
            Kernel.Bind<IProductService>().To<ProductService>();
            Kernel.Bind<IProductTypeService>().To<ProductTypeService>();

            EmailSettings emailSettings = new EmailSettings
            {
                WriteAsFile = bool.Parse(ConfigurationManager
                    .AppSettings["Email.WriteAsFile"] ?? "false")
            };

            Kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>()
                .WithConstructorArgument("settings", emailSettings);

            Kernel.Bind<IAuthProvider>().To<FormAuthProvider>();
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType!=null? (IController)Kernel.Get(controllerType): null;
        }
    }
}