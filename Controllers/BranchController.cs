using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NuCloudWeb.Models;

namespace NuCloudWeb.Controllers {
    public class BranchController : Controller {
        public IActionResult Index() {
            return View();
        }

        [HttpGet]
        [Route("Branch/Branch/{cod:int}")]
        public async Task<ActionResult> Branch([FromRoute] int cod) {
            Branch n = await DB.Instance.GetBranch(cod);
            n.Children = await DB.Instance.BranchGroups(cod);
            n.Members = await DB.Instance.GetMembersOfNode(cod, "Rama");
            return View(n);
        }

        [HttpGet]
        [Route("Branch/AddGroup/{cod:int}")]
        public IActionResult AddGroup([FromRoute] int cod) {            
            return View();
        }

        [HttpPost]
        [Route("Branch/AddGroup/{cod:int}")]
        public IActionResult AddGroup([FromRoute] int cod, Group g) {
            long res = DB.Instance.CantGroupForCod(cod).Result;
            if(res == 0)
            {
                DB.Instance.AddGroup(cod, g);
            }            
            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpGet]
        [Route("Branch/AddMember/{cod:int}")]
        public async Task<ActionResult> AddMember([FromRoute] int cod) {
            List<Member> t = await DB.Instance.GetMembers();
            Chanchito chanchito = new Chanchito() {
                Members = t
            };
            return View(chanchito);
        }

        [HttpPost]
        [Route("Branch/AddMember/{cod:int}")]
        public IActionResult AddMember([FromRoute] int cod, Chanchito c) {
            DB.Instance.AddMemberToBranch(cod, c.Id);
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
