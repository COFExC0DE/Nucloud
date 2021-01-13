﻿using System;
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
        //Instance that return view 
        [Route("Member/Member/{ced:int}")]
        public async Task<ActionResult> Member([FromRoute] int ced) {
            Member t = await DB.Instance.GetMember(ced.ToString());
            t.Leads = await DB.Instance.GetLeads(ced.ToString());
            t.Coaches = await DB.Instance.GetCoaches(ced.ToString());
            t.MemberOf = await DB.Instance.MemberOf(ced.ToString());
            return View(t);
        }

        [HttpGet]
        //View to add member
        public IActionResult AddMember() {
            return View();
        }

        [HttpPost]
        //Function that create a member
        public IActionResult AddMember(Member member) {
            long num = DB.Instance.CantMemberForCed(member.Ced).Result;
            if (num == 0)
            {
                DB.Instance.CreateMember(new Member()
                {
                    Name = member.Name,
                    LastName = member.LastName,
                    Ced = member.Ced,
                    Phone = member.Phone,
                    Password = member.Password
                });
            }            
            return View();
        }

        [HttpGet]
        //View to login member
        public IActionResult LoginMember()
        {
            return View();
        }

        [HttpPost]
        //cuek
        public IActionResult LoginMember(Member member)
        {
            long num = DB.Instance.CantMemberForCedAndPass(member.Ced, member.Password).Result;
            if(num == 1)
            {
                System.Diagnostics.Debug.WriteLine("Caca");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Caca fallo");
            }
            return View();
        }
    }
}
