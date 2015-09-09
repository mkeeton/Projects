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

namespace IcedMemories
{
    public class MvcApplication : System.Web.HttpApplication
    {
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
            });
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
