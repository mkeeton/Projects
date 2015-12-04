using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Web;
using Microsoft.Practices.Unity;
using ISSolutions.Domain.Interfaces.Configuration;
using System.Collections.Generic;

namespace ISSolutions.Framework.Configuration
{
    public class ComponentInitialiser : IComponentInitialiser
    {
        public static IUnityContainer Container { get; set; }

        public void Initialise()
        {
            Container = new UnityContainer();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                                .Where(a => a.FullName.StartsWith("ISSolutions."))
                                .ToList();

            var installers = assemblies.SelectMany(TryGetExportedTypes)
                                        .Where(t => typeof(IInstaller).IsAssignableFrom(t) && t.IsClass)
                                        .ToList();

            foreach (var installerType in installers)
            {
                var installer = (IInstaller)Activator.CreateInstance(installerType);
                installer.Install(Container);
            }
        }

        private static IEnumerable<Type> TryGetExportedTypes(Assembly assembly)
        {
            try
            {
                return assembly.GetExportedTypes();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return new Type[] { };
        }

        public void Dispose()
        {
            Container.Dispose();
        }
    }
}
