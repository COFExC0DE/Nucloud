using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace NuCloud {
    public class Program {
        public static void Main(string[] args) {
            DB.Instance.SetDriver("neo4j://localhost:7687", "neo4j", "1234");
            CreateHostBuilder(args).Build().Run();
        }

        public static async Task DoShit() {
            List<string> t = await DB.Instance.GetAsync();
            t.ForEach(x => Console.WriteLine(x));
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
