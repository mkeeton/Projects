using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Threading.Tasks;
using System.Security.Principal;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using IcedMemories.Infrastructure;
using AutoMapper;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace IcedMemories
{
    public class MvcApplication : System.Web.HttpApplication
    {

        private static IWindsorContainer container;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Mapper.Initialize(cfg =>
            {
              cfg.CreateMap<IcedMemories.Domain.Models.Cake, Models.CakeViewModel>();
              cfg.CreateMap<IcedMemories.Domain.Models.SearchCategory, Models.SearchCategorySelection>();
              cfg.CreateMap<IcedMemories.Domain.Models.SearchCategoryOption, Models.SearchCategoryOptionSelection>();
            });
            BootstrapContainer();
        }

        protected void Application_End()
        {
          container.Dispose();
        }

        private static void BootstrapContainer()
        {
          container = new WindsorContainer()
              .Install(FromAssembly.This());
          var controllerFactory = new Factories.WindsorControllerFactory(container.Kernel);
          ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        }

        public static IWindsorContainer GetContainer()
        {
          return container;
        }
        //protected void Application_OnAuthenticateRequest(Object src, EventArgs e)
        //{
        //  HttpContext currentContext = HttpContext.Current;
        //  if (currentContext.User != null)
        //  {
        //    if (currentContext.User.Identity.IsAuthenticated)
        //    {
        //      UnitOfWork _workManager = currentContext.GetOwinContext().Get<UnitOfWork>();
        //      if (_workManager != null)
        //      {
        //        IList<string> _roles = _workManager.UserManager.GetRolesAsync(new Guid(HttpContext.Current.User.Identity.GetUserId())).Result;
        //        HttpContext.Current.User = new GenericPrincipal(HttpContext.Current.User.Identity, _roles.ToArray<string>());
        //      }
        //    }
        //  }
        //}
    }
}
