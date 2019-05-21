﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RealTimeTasks.Data;

namespace RealTimeTasks.Web.Controllers
{
    public class AccountController : Controller
    {
        private string _connectionString;

        public AccountController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        public IActionResult Login(string returnUrl)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var db = new UserRepository(_connectionString);
            var user = db.Login(email, password);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            //this code is conceptually the same as FormsAuthentication.SetAuthCookie()
            var claims = new List<Claim>
            {
                new Claim("user", email)
            };
            HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(claims, "Cookies", "user", "role"))).Wait();
            return Redirect("/");
        }

        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Signup(User user, string password)
        {
            var db = new UserRepository(_connectionString);
            db.AddUser(user, password);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();
            return Redirect("/");
        }
    }
}