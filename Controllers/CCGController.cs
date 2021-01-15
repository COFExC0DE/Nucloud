using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
        [Route("CCG/Mail/{id}")]
        public IActionResult Mail([FromRoute] string id) {
            Message t = Mongo.Instance.GetMessage(id.ToString());
            return View(t);
        }

        public IActionResult Report() {
            int pet = Mongo.Instance.GetMessageCount("petition");
            int con = Mongo.Instance.GetMessageCount("congratulate");
            int of = Mongo.Instance.GetMessageCount("offer");
            string text = "";
            string date = DateTime.Now.ToString("dd/MM/yyyy");
            if (of + con + pet > 0) {
                text = String.Format("This month we got {0} contributions of which {1} are petitions, {2} are congratulations and {3} are offers.", (pet + con + of), pet, con, of);          
            } else {
                text = "This month there were no contributions";
            }
            Mongo.Instance.InsertNews(String.Format("{0} report", date), date, text, "Nucloud Team");         
            return RedirectToAction("Inbox", "CCG");
        }

        public IActionResult Clear() {
            Mongo.Instance.ClearInbox();
            return RedirectToAction("Inbox", "CCG");
        }

        [HttpGet]
        public IActionResult Write() {
            return View();
        }

        [HttpPost]
        public IActionResult Write(Message msg) {
            Mongo.Instance.InsertMessage(new Message() {
                Subject = msg.Subject,
                Body = msg.Body,
                Date = DateTime.Now.ToString("dd/MM/yyyy"),
                Type = msg.Type,
                Sender = "None"
            });
            return RedirectToAction("Index", "Home");
        }

    }
}
