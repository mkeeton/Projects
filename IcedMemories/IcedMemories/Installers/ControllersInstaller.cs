using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System.Web;
using System.Web.Configuration;

namespace IcedMemories.Installers
{
  public class ControllersInstaller : IWindsorInstaller
  {
    public void Install(IWindsorContainer container, IConfigurationStore store)
    {
      container.Register(Classes.FromThisAssembly()
          .BasedOn<IController>()
          .LifestyleTransient());
      container.Register(
          Component.For<IcedMemories.Data.Interfaces.IDbContext>()
              .UsingFactoryMethod(_ => IcedMemories.Data.DbContext.DapperAccessDbContext.Create(WebConfigurationManager.ConnectionStrings["AccessConnection"].ConnectionString)).LifestylePerWebRequest()
,
          Component.For<IcedMemories.Infrastructure.Interfaces.IUnitOfWork>()
              .UsingFactoryMethod(k => IcedMemories.Infrastructure.UnitOfWorkOle.Create(k.Resolve<IcedMemories.Data.Interfaces.IDbContext>())).LifestylePerWebRequest()
      );
    }
  }
}