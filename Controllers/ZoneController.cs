using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NuCloudWeb.Models;

namespace NuCloudWeb.Controllers {
    public class ZoneController : Controller {
        public IActionResult Index() {
            return View();
        }

        [HttpGet]
        [Route("Zone/Zone/{cod:int}")]
        public async Task<ActionResult> Zone([FromRoute] int cod) {
            Zone n = await DB.Instance.GetZone(cod);
            n.Children = await DB.Instance.ZoneBranches(cod);
            n.Members = await DB.Instance.GetMembersOfNode(cod, "Zona");
            return View(n);
        }
    }
}
