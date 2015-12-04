using System;
using Microsoft.Practices.Unity;
using ISSolutions.Domain.Interfaces.Configuration;
using ISSolutions.Domain.Interfaces.Repositories;
using ISSolutions.Infrastructure.Repositories.EntityFramework;

namespace ISSolutions.Infrastructure.Installers
{
    public class RepositoriesInstaller : IInstaller
    {
        public void Install(IUnityContainer container)
        {
            container.RegisterType<ISessionRepository, SessionRepository>(new ContainerControlledLifetimeManager());
        }
    }
}
