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
        //Get Coordination instances
        [Route("Organization/Cloud/{cod:int}")]
        public async Task<ActionResult> Cloud([FromRoute] int cod) {
            Cloud cloud = await DB.Instance.GetCloud(cod);
            cloud.Coordination = await DB.Instance.GetCoordination(cod);
            cloud.Coordination.Chief = await DB.Instance.GetChief(cod);
            cloud.Coordination.Children = await DB.Instance.CoordinationZones(cod);
            cloud.Coordination.Members = await DB.Instance.GetMembersOfNode(cod, "Coordination");
            return View(cloud);
        }

        [HttpGet]
        //Function for Add Zone
        [Route("Organization/AddZone/{cod:int}")]
        public IActionResult AddZone() {
            return View();
        }

        [HttpPost]
        //Instance that addZone
        [Route("Organization/AddZone/{cod:int}")]
        public IActionResult AddZone([FromRoute] int cod, Zone g) {
            DB.Instance.AddZone(cod, g);
            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpGet]
        //Call that insterface of AddChief
        [Route("Organization/AddChief/{cod:int}")]
        public IActionResult AddChief() {
            return View();
        }

        [HttpPost]
        //View that adds Chief
        [Route("Organization/AddChief/{cod:int}")]
        public IActionResult AddChief([FromRoute] int cod, Chief chief) {
            DB.Instance.CreateChief(new Chief() {
                Name = chief.Name,
                LastName = chief.LastName,
                Ced = chief.Ced,
                Start = chief.Start,
                End = chief.End,
                Phone = chief.Phone,
                Email = chief.Email
            }, cod);
            return View();
        }
    }
}
