using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Castle.Windsor;
using Castle.Windsor.Installer;
using System.Web.Http.Dispatcher;
using Authentication.ISSolutions.Domain.Models;

namespace ISSolutions.Home
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
        BootstrapContainer();
      }

      private static void BootstrapContainer()
      {
        container = new WindsorContainer()
            .Install(FromAssembly.This());
        var controllerFactory = new IOC.CastleWindsor.Factories.WindsorControllerFactory(container.Kernel);
        ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerActivator), new IOC.CastleWindsor.Factories.WindsorHttpControllerActivator(container));
      }

      public static IWindsorContainer GetContainer()
      {
        return container;
      }

      protected void Application_End()
      {
        container.Dispose();
      }

      protected void Session_End(object sender, EventArgs e)
      {
        LoginList currentLogins = container.Kernel.Resolve<LoginList>();
        var currentLogin = currentLogins.Logins.Where(x => x.SessionId == Session.SessionID && x.LogoutDate == null).FirstOrDefault();
        if (currentLogin != null)
        {
          currentLogins.Logins.Remove(currentLogin);
        }
        var currentSession = currentLogins.ClientSessions.Where(x => x.LocalSessionID == Session.SessionID).FirstOrDefault();
        if (currentSession != null)
        {
          currentLogins.ClientSessions.Remove(currentSession);
        }
      }
    }
}
