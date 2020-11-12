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
        //
        [Route("Zone/Zone/{cod:int}")]
        public async Task<ActionResult> Zone([FromRoute] int cod) {
            Zone n = await DB.Instance.GetZone(cod);
            n.Children = await DB.Instance.ZoneBranches(cod);
            n.Members = await DB.Instance.GetMembersOfNode(cod, "Zona");
            n.Leaders = await DB.Instance.GetLeaders("Zona", cod);
            return View(n);
        }

        [HttpGet]
        //Call view to add branch
        [Route("Zone/AddBranch/{cod:int}")]
        public IActionResult AddBranch([FromRoute] int cod) {
            return View();
        }

        [HttpPost]
        //Instance that refers to add Branch
        [Route("Zone/AddBranch/{cod:int}")]
        public IActionResult AddBranch([FromRoute] int cod, Branch g) {
            DB.Instance.AddBranch(cod, g);
            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpGet]
        [Route("Zone/AssignLeader/{cod:int}")]
        public async Task<ActionResult> AssignLeader([FromRoute] int cod) {
            Chanchito chanchito = new Chanchito {
                Members = await DB.Instance.GetMembersOfNode(cod, "Zona")
            };

            return View(chanchito);
        }

        [HttpPost]
        //Gets Parent, makes a member a leader
        [Route("Zone/AssignLeader/{cod:int}")]
        public async Task<ActionResult> AssignLeader([FromRoute] int cod, Chanchito c) {
            DB.Instance.MakeMemberNodeLeader(cod, c.Ced, "Zona");
            int i = await DB.Instance.GetParentCode("Coordination", "Zona", cod);
            DB.Instance.AddMemberToCoord(i, c.Ced);
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
