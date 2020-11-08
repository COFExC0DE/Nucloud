using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NuCloudWeb.Models;

namespace NuCloudWeb.Controllers {
    public class GroupController : Controller {
        public IActionResult Index() {
            return View();
        }

        [HttpGet]
        [Route("Group/Group/{cod:int}")]
        public async Task<ActionResult> Group([FromRoute] int cod) {
            Group n = await DB.Instance.GetGroup(cod);
            n.Members = await DB.Instance.GetMembersOfGroup(cod);
            return View(n);
        }
    }
}
