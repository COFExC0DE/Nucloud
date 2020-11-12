using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NuCloudWeb.Models;

namespace NuCloudWeb.Controllers {

    //
    public class BranchController : Controller {
        public IActionResult Index() {
            return View();
        }

        [HttpGet]
        [Route("Branch/Branch/{cod:int}")]
        //Instances of database getters of a branch
        public async Task<ActionResult> Branch([FromRoute] int cod) {
            Branch n = await DB.Instance.GetBranch(cod);
            n.Children = await DB.Instance.BranchGroups(cod);
            n.Members = await DB.Instance.GetMembersOfNode(cod, "Rama");
            n.Leaders = await DB.Instance.GetLeaders("Rama", cod);
            return View(n);
        }

        [HttpGet]
        [Route("Branch/AddGroup/{cod:int}")]
        //Interface for the Add Group view
        public IActionResult AddGroup([FromRoute] int cod) {            
            return View();
        }

        [HttpPost]
        //Instance to add member add group
        [Route("Branch/AddGroup/{cod:int}")]
        public IActionResult AddGroup([FromRoute] int cod, Group g) {
            DB.Instance.AddGroup(cod, g);
            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpGet]
        //Instance to get Member
        [Route("Branch/AddMember/{cod:int}")]
        public async Task<ActionResult> AddMember([FromRoute] int cod) {
            List<Member> t = await DB.Instance.GetMembers();
            Chanchito chanchito = new Chanchito() {
                Members = t
            };
            return View(chanchito);
        }

        [HttpPost]
        //Instance to add members
        [Route("Branch/AddMember/{cod:int}")]
        public IActionResult AddMember([FromRoute] int cod, Chanchito c) {
            DB.Instance.AddMemberToBranch(cod, c.Ced);
            return Redirect(Request.Headers["Referer"].ToString());
        }

        // Instance to assign Leader to Branch
        [HttpGet]
        [Route("Branch/AssignLeader/{cod:int}")]
        public async Task<ActionResult> AssignLeader([FromRoute] int cod) {
            Chanchito chanchito = new Chanchito {
                Members = await DB.Instance.GetMembersOfNode(cod, "Rama")
            };

            return View(chanchito);
        }

        [HttpPost]
        //Instance to asign a Leader
        [Route("Branch/AssignLeader/{cod:int}")]
        public async Task<ActionResult> AssignLeader([FromRoute] int cod, Chanchito c) {
            DB.Instance.MakeMemberNodeLeader(cod, c.Ced, "Rama");
            int i = await DB.Instance.GetParentCode("Zona", "Rama", cod);
            DB.Instance.AddMemberToZone(i, c.Ced);
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
