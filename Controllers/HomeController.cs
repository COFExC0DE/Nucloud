using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NuCloudWeb.Models;

namespace NuCloudWeb.Controllers {
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger) {
            _logger = logger;
        }

        //Instance that call the view
        public IActionResult Index() {
            return View();
        }

        public IActionResult News() {
            return View();
        }

        //Functions that get Clouds
        public async Task<ActionResult> Organization() {
            List<Cloud> t = await DB.Instance.GetClouds();
            return View(t);
        }


        [HttpGet]
        //Interface to Add Cloud
        public IActionResult AddCloud() {
            return View();
        }

        [HttpPost]
        //Intance to call interface of cloud
        public IActionResult AddCloud(Cloud g) {
            DB.Instance.AddCloud(g);
            return RedirectToAction("Organization", "Home");
        }

        //Instance that activate  an error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
