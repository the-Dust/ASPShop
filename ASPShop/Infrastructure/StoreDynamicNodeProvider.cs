using DataAccess.Repositories.Base;
using MvcSiteMapProvider;
using Services.BuisnessLogic;
using Services.BuisnessLogic.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Infrastructure
{
    public class StoreDynamicNodeProvider : DynamicNodeProviderBase
    {
        private IProductService service = (IProductService)new NinjectControllerFactory().Kernel.GetService(typeof(IProductService));
        private IProductTypeService serviceType = (IProductTypeService)new NinjectControllerFactory().Kernel.GetService(typeof(IProductTypeService));

        public override IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode nodeX)
        {
            var nodes = new List<DynamicNode>();
            var types = serviceType.GetProductTypes().ToList();
            foreach (var type in types)
            {
                DynamicNode node = new DynamicNode();
                //unique key for each node
                node.Key = "type_" + type.Id.ToString();
                node.RouteValues.Add("category", type.Name);
                node.Action = "GetCatalogue";
                node.Controller = "Product";
                node.Title = type.Description;
                nodes.Add(node);
                var items = service.GetProducts(type.Id);
                if (items != null)
                {
                    foreach (var item in items)
                    {
                        DynamicNode node2 = new DynamicNode();
                        node2.Key = "product_" + item.Id.ToString();
                        node2.ParentKey = node.Key;
                        node2.RouteValues.Add("productId", item.Id);
                        node2.Action = "GetProductInfo";
                        node2.Controller = "Product";
                        node2.Title = item.Name;
                        nodes.Add(node2);
                    }
                }

            }
            return nodes;
        }


    }
}