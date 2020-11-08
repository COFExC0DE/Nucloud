using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NuCloudWeb.Models;

namespace NuCloudWeb.Controllers {
    public class MemberController : Controller {
        public IActionResult Index() {
            return View();
        }

        [HttpGet]
        [Route("Member/Member/{ced:int}")]
        public async Task<ActionResult> Member([FromRoute] int ced) {
            Member t = await DB.Instance.GetMember(ced.ToString());
            return View(t);
        }
    }
}
