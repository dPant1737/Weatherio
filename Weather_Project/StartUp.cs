using ApplicationSettings;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather_Project
{
    public class StartUp
    {
        public StartUp()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);

            IConfiguration configuration = builder.Build();
            ApplicationConfig = configuration.GetSection("WeatherSettings").Get<ApplicationConfig>();
        }

        public IApplicationConfig ApplicationConfig { get; set; }
    }
}
