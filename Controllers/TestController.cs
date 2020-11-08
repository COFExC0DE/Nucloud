using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace NuCloudWeb.Controllers {
    public class TestController : Controller {
        // 
        // GET: /Test/
        public IActionResult Index() {
            return View();
        }

        // 
        // GET: /Test/Caca/ 
        public async Task<IActionResult> Caca() {
            DB.Instance.SetDriver("bolt://54.236.12.7:33833", "neo4j", "tablet-admission-register");
            List<string> t = await DB.Instance.GetAll();
            return Ok(t.Aggregate((i,j) => i + j));
        }

    }
}
