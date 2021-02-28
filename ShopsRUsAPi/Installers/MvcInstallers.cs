using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ShopsRUsAPi.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Models;
using System.Threading.Tasks;
using ShopsRUsAPi.Filters;
using FluentValidation.AspNetCore;
using ShopsRUsAPi.Validators;

namespace ShopsRUsAPi.Installers
{
    public class MvcInstallers : IInstaller
    {
        public void InstallSevices(IServiceCollection services, IConfiguration configuration)
        {
       

            services.AddMvc(
                options => {
                    options.EnableEndpointRouting = false;
                   options.Filters.Add<ValidationFilter>();

                }).AddFluentValidation(mvcConfiguration => mvcConfiguration.RegisterValidatorsFromAssemblyContaining<Startup>())
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "ShopsRUs API", Version = "V1" });
                //var security = new Dictionary<string, IEnumerable<string>>
                //{
                //    {"Bearer", new string[0]}
                //};

              

            });
            services.AddControllersWithViews()
                .AddNewtonsoftJson( options => 
                options.SerializerSettings.ReferenceLoopHandling= Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );




        }
    }
}
  
