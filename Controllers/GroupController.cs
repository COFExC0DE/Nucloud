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



        [HttpPost]
        //Instance to add member to a group
        [Route("Group/Observer/{cod:int}")]
        public IActionResult Observer([FromRoute] int cod, Chanchito c)
        {
            DB.Instance.SendObserver(cod, "Rama", c.Ced);
            return RedirectToAction("Group", "Group", new { cod = cod });
        }

        [HttpGet]
        //Instance to call the view
        [Route("Group/Observer/{cod:int}")]
        public async Task<ActionResult> Observer([FromRoute] int cod)
        {
            List<Member> t = await DB.Instance.GetObserver(cod, "Rama");
            Chanchito chanchito = new Chanchito()
            {
                Members = t
            };
            return View(chanchito);
        }

        

        [HttpGet]
        //Instance to call the view
        [Route("Group/Group/{cod:int}")]
        public async Task<ActionResult> Group([FromRoute] int cod) {
            Group n = await DB.Instance.GetGroup(cod);
            n.Members = await DB.Instance.GetMembersOfNode(cod, "Grupo");
            n.Leaders = await DB.Instance.GetLeaders("Grupo", cod);
            n.Coach = await DB.Instance.GetCoach("Grupo", cod);
            return View(n);
        }

        [HttpGet]
        //Instance to get member
        [Route("Group/AddMember/{cod:int}")]
        public async Task<ActionResult> AddMember([FromRoute] int cod) {
            List<Member> t = await DB.Instance.GetMembers();
            Chanchito chanchito = new Chanchito() { 
                Members = t
            };
            return View(chanchito);
        }

        [HttpPost]
        //Instance to add member to a group
        [Route("Group/AddMember/{cod:int}")]
        public IActionResult AddMember([FromRoute] int cod, Chanchito c) {
            DB.Instance.AddMemberToGroup(cod, c.Ced);
            return RedirectToAction("Group", "Group", new { cod = cod });
        }

        [HttpGet]
        //Instance to Get Monitors
        [Route("Group/AssignMonitor/{cod:int}")]
        public async Task<ActionResult> AssignMonitor([FromRoute] int cod) {
            List<Member> t = await DB.Instance.GetMonitors();
            Chanchito chanchito = new Chanchito() {
                Members = t
            };
            return View(chanchito);
        }

        [HttpPost]
        //Instance to assign monitor
        [Route("Group/AssignMonitor/{cod:int}")]
        public IActionResult AssignMonitor([FromRoute] int cod, Chanchito c) {
            DB.Instance.MakeMemberMonitor(cod, c.Ced);
            return RedirectToAction("Group", "Group", new { cod = cod });
        }

        [HttpGet]
        //Instance to asign leader
        [Route("Group/AssignLeader/{cod:int}")]
        public async Task<ActionResult> AssignLeader([FromRoute] int cod) {
            Chanchito chanchito = new Chanchito {
                Members = await DB.Instance.GetMembersOfNode(cod, "Grupo")
            };

            return View(chanchito);
        }

        //Instance to make a member a group Leader
        [HttpPost]
        [Route("Group/AssignLeader/{cod:int}")]
        public async Task<ActionResult> AssignLeader([FromRoute] int cod, Chanchito c) {
            DB.Instance.MakeMemberNodeLeader(cod, c.Ced, "Grupo");
            int i = await DB.Instance.GetParentCode("Rama", "Grupo", cod);
            DB.Instance.AddMemberToBranch(i, c.Ced);
            return RedirectToAction("Group", "Group", new { cod = cod });
        }

        [HttpGet]
        [Route("Group/MoveMember/{cod:int}")]
        public async Task<ActionResult> MoveMember([FromRoute] int cod) {
            int c = await DB.Instance.GetParentCode("Rama", "Grupo", cod);
            Chanchito chanchito = new Chanchito {
                Members = await DB.Instance.GetMembersOfNode(cod, "Grupo"),
                Groups = await DB.Instance.BranchGroups(c)
            };

            return View(chanchito);
        }

        [HttpPost]
        [Route("Group/MoveMember/{cod:int}")]
        public async Task<ActionResult> MoveMember([FromRoute] int cod, Chanchito c) {
            DB.Instance.RemoveMember("Grupo", cod, c.Ced);
            DB.Instance.AddMemberToGroup(c.Cod, c.Ced);
            return RedirectToAction("Group", "Group", new { cod = cod });
        }
    }

    public class Chanchito {
        public string Ced { get; set; }
        public int Cod { get; set; }
        public List<Member> Members { get; set; }
        public List<Group> Groups { get; set; }
    }
}
