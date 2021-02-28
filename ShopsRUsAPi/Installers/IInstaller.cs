using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopsRUsAPi.Installers
{
  public  interface IInstaller
    {
        void InstallSevices(IServiceCollection services, IConfiguration configuration);
    }
}
