using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuCloudWeb.Models;

namespace NuCloudWeb.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
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
        public async Task<IActionResult> LoginMember(Member member)
        {
            long num = DB.Instance.CantMemberForCedAndPass(member.Ced, member.Password).Result;
            if (num == 1)
            {
                var miembro = DB.Instance.GetMember(member.Ced);
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, member.Ced.ToString()));
                identity.AddClaim(new Claim(ClaimTypes.Name, miembro.Result.Name));
                identity.AddClaim(new Claim(ClaimTypes.Surname, miembro.Result.LastName));
                identity.AddClaim(new Claim(ClaimTypes.Email, miembro.Result.Email is null ? "" : miembro.Result.Email));
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                    new AuthenticationProperties { ExpiresUtc = DateTime.Now.AddDays(1), IsPersistent = true });
                return RedirectToAction("Index", "Home");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Caca fallo");
            }
            return View();
        }
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
    }    

}