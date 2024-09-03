using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using app.DAL.Data;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace ConfigureManager.cs
{
    public static class ConfigureDependencies
    {
        public static void ConfigureDependency(this IServiceCollection services)
        {
            services.AddScoped<DataContext>();
            
        }


    }
}
