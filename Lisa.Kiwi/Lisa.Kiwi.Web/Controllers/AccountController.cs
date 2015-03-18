﻿using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Lisa.Kiwi.Web.ViewModels;
using Lisa.Kiwi.WebApi.Access;

namespace Lisa.Kiwi.Web.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var authProxy = new AuthenticationProxy("http://localhost:20151", "/api/oauth");

            var response = await authProxy.Login(model.UserName, model.Password);

            if (response.Status != LoginStatus.Success)
            {
                ModelState.AddModelError("password", "Wachtwoord of gebruikersnaam is onjuist");

                return View();
            }

            var tokenCookie = new HttpCookie("token", String.Join(" ", response.TokenType, response.Token))
            {
                Expires = DateTime.Now.AddMinutes(response.TokenExpiresIn),
                HttpOnly = true
            };

            Response.Cookies.Add(tokenCookie);

            // dont store isAdmin value in cookies as it is unsafe

            return RedirectToAction("Index", "Dashboard");
        }
    }
}