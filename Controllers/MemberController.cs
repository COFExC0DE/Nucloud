using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Neo4j.Driver;
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

        [HttpGet]
        public IActionResult AddMember() {
            return View();
        }

        [HttpPost]
        public IActionResult AddMember(Member member) {
            long num = DB.Instance.CantMemberForCed(member.Ced).Result;
            if (num == 0)
            {
                DB.Instance.CreateMember(new Member()
                {
                    Name = member.Name,
                    LastName = member.LastName,
                    Ced = member.Ced,
                    Phone = member.Phone
                });
            }            
            return View();
        }
    }
}
