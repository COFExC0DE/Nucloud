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
        [Route("Organization/Cloud/{cod:int}")]
        public async Task<ActionResult> Cloud([FromRoute] int cod) {
            Cloud cloud = await DB.Instance.GetCloud(cod);
            cloud.Coordination = await DB.Instance.GetCoordination(cod);
            cloud.Coordination.Children = await DB.Instance.CoordinationZones(cod);
            cloud.Coordination.Members = await DB.Instance.GetMembersOfNode(cod, "Coordination");
            return View(cloud);
        }

        [HttpGet]
        [Route("Organization/AddZone/{cod:int}")]
        public IActionResult AddZone([FromRoute] int cod) {
            return View();
        }

        [HttpPost]
        [Route("Organization/AddZone/{cod:int}")]
        public IActionResult AddZone([FromRoute] int cod, Zone g) {
            DB.Instance.AddZone(cod, g);
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
