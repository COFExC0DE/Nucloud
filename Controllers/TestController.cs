using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NuCloudWeb.Models;

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
            List<Member> t = await DB.Instance.GetMembersOfGroup(2);
            return Ok(t.Select(x => x.Name).Aggregate((i,j) => i + j));
        }

        // 
        // GET: /Test/Crear/ 
        public async void Crear() {
            DB.Instance.SetDriver("bolt://54.236.12.7:33833", "neo4j", "tablet-admission-register");

        }

    }
}
