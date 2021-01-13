using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace NuCloudWeb.Controllers {
    public class CCGController : Controller {
        public IActionResult Index() {
            return View();
        }

        public async Task<ActionResult> Inbox() {
            // get messages to show
            // return View(t); 
            return View();
        }

        [HttpGet]
        [Route("CCG/Message/{cod:int}")]
        //Interface for the Add Group view
        public IActionResult Message([FromRoute] int cod) {
            return View();
        }

        public void Download() {
          
        }

        public void Report() {

        }

    }
}
