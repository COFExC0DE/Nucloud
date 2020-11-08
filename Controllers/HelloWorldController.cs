using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace NuCloudWeb.Controllers {
    public class HelloWorldController : Controller {
        // 
        // GET: /HelloWorld/
        public string Index() {
            return "This is my default action...";
        }

        // 
        // GET: /HelloWorld/Welcome/ 
        public string Welcome() {
            return "This is the Welcome action method...";
        }

        // 
        // GET: /HelloWorld/Caca/ 
        public async Task<IActionResult> Caca() {
            DB.Instance.SetDriver("neo4j://localhost:7687", "neo4j", "1234");
            List<string> t = await DB.Instance.GetAll();
            return Ok(t.Aggregate((i,j) => i + j));
        }

    }
}
