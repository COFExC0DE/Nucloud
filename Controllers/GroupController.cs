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
            n.Members = await DB.Instance.GetMembersOfNode(cod, "Grupo");
            return View(n);
        }

        [HttpGet]
        [Route("Group/AddMember/{cod:int}")]
        public async Task<ActionResult> AddMember([FromRoute] int cod) {
            List<Member> t = await DB.Instance.GetMembers();
            Chanchito chanchito = new Chanchito() { 
                Members = t,
                Cod = cod
            };
            return View(chanchito);
        }

        [HttpPost]
        [Route("Group/AddMember/{cod:int}")]
        public IActionResult AddMember([FromRoute] int cod, Chanchito c) {
            DB.Instance.AddMemberToGroup(cod, c.Ced);
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }

    public class Chanchito {
        public int Cod { get; set; }
        public string Ced { get; set; }
        public List<Member> Members { get; set; }
    }
}
