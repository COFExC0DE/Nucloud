using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NuCloudWeb.Controllers;

namespace NuCloudWeb {
    public class Program {
        public static void Main(string[] args) {
            DB.Instance.SetDriver("bolt://52.72.13.205:51760", "neo4j", "slices-gland-proposes");
            Mongo.Instance.InsertTest();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
