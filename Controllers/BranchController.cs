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
            DB.Instance.AddGroup(cod, g);
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
