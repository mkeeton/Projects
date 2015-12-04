using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;

namespace ISSolutions.Domain.Interfaces.Configuration
{
    public interface IInstaller
    {
        void Install(IUnityContainer container);
    }
}
