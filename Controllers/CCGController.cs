using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NuCloudWeb.Models;

namespace NuCloudWeb.Controllers {
    public class CCGController : Controller {
        public IActionResult Index() {
            return View();
        }

        public IActionResult Inbox() {
            List<Message> t = Mongo.Instance.GetMessages();
            return View(t);
        }

        [HttpGet]
        //Instance that return view 
        [Route("CCG/Message/{id:int}")]
        public IActionResult Message([FromRoute] int id) {
            Message t = Mongo.Instance.getMessage(id.ToString());
            return View(t);
        }

        public void Download() {
          
        }

        public void Report() {

        }

    }
}
