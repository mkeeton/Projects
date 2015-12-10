using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System.Web;
using System.Web.Configuration;
using ISSolutions.Core.Data.Interfaces;
using ISSolutions.Core.Data.DbContext;
using ISSolutions.Core.Infrastructure.Interfaces;
using Authentication.ISSolutions.Infrastructure.Interfaces;
using Authentication.ISSolutions.Infrastructure;
using Authentication.ISSolutions.Infrastructure.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Http;

namespace ISSolutions.Home.IOC.CastleWindsor.Installers
{
  public class ControllersInstaller : IWindsorInstaller
  {
    public void Install(IWindsorContainer container, IConfigurationStore store)
    {
      container.Register(Classes.FromThisAssembly()
          .BasedOn<IController>()
          .LifestyleTransient());
      container.Register(Classes.FromThisAssembly()
          .BasedOn<ApiController>()
          .LifestyleTransient());
      container.Register(
          Component.For<IDbContext>()
              .UsingFactoryMethod(_ => new DbContextSql(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString)).LifestylePerWebRequest()
      );

      container.Register(
                Component.For<Authentication.ISSolutions.Domain.Models.LoginList>()
                .UsingFactoryMethod(_ => new Authentication.ISSolutions.Domain.Models.LoginList(360))
                .LifeStyle.Singleton
      );

      container.Register(
                Component.For<IUserStore<Authentication.ISSolutions.Domain.Models.User,System.Guid>>()
                .ImplementedBy<Authentication.ISSolutions.Infrastructure.Repositories.Sql.UserStore<Authentication.ISSolutions.Domain.Models.User>>()
                .LifeStyle.PerWebRequest
      );

      container.Register(
          Component.For<ILoginRepository>()
          .ImplementedBy<LoginRepository>()
          .LifeStyle.PerWebRequest
      );

      container.Register(
          Component.For<ISessionRepository>()
          .ImplementedBy<SessionRepository>()
          .LifeStyle.PerWebRequest
      );

      container.Register(
                Component.For<UnitOfWork>()
                .ImplementedBy<UnitOfWork>()
                .LifeStyle.PerWebRequest
      );

      container.Register(
          Component.For<ApplicationUserManager>()
              .UsingFactoryMethod(_ => HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()).LifestylePerWebRequest()
      );

      container.Register(
          Component.For<ApplicationSignInManager>()
              .UsingFactoryMethod(_ => HttpContext.Current.GetOwinContext().GetUserManager<ApplicationSignInManager>()).LifestylePerWebRequest()
      );

    }

  }
}