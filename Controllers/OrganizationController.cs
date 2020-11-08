using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NuCloudWeb.Models;

namespace NuCloudWeb.Controllers {
    public class OrganizationController : Controller {
        public IActionResult Index() {
            return View();
        }

        public async Task<ActionResult> Members() {
            List<Member> t = await DB.Instance.GetMembers();
            return View(t);
        }

        [HttpGet]
        [Route("Organization/Organization/{cod:int}")]
        public async Task<ActionResult> Organization([FromRoute] int cod) {
            Coordination coord = await DB.Instance.GetCoordination(cod);
            coord.Children = await DB.Instance.CoordinationZones(cod);
            coord.Members = new List<Member>(); // get members
            return View(coord);
        }
    }
}
