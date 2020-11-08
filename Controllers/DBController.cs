using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Neo4j.Driver;

namespace NuCloudWeb.Controllers {
    public class DBController : Controller {
        /*
        public IActionResult Index() {
            return View();
        }

        // GET: /Test
        public static async void Index() {
            DB.Instance.SetDriver("neo4j://localhost:7687", "neo4j", "1234");
            List<string> t = await DB.Instance.GetAll();
            t.ForEach(x => Console.WriteLine(x));
        } */

        // GET: /HelloWorld
        public string Index() {
            return "caca";
        }
    }
}
