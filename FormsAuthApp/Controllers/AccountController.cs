﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FormsAuthApp.Models;
using System.Web.Security;

namespace FormsAuthApp.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                // поиск пользователя в бд
                User user = null;
                using (AuthenticationEntities db = new AuthenticationEntities())
                {
                    user = db.User.Where(u => u.Email == model.Name && u.Password == model.Password).FirstOrDefault();

                }
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Name, true);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Пользователя с таким логином и паролем нет");
                }
            }

            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = null;
                using (AuthenticationEntities db = new AuthenticationEntities())
                {
                    user = db.User.Where(u => u.Email == model.Name && u.Password == model.Password).FirstOrDefault();
                }
                if (user == null)
                {
                    // создаем нового пользователя
                    using (AuthenticationEntities db = new AuthenticationEntities())
                    {
                        db.User.Add(new User { Email = model.Name, Password = model.Password, Age = model.Age });
                        db.SaveChanges();

                        user = db.User.Where(u => u.Email == model.Name && u.Password == model.Password).FirstOrDefault();
                    }
                    // если пользователь удачно добавлен в бд
                    if (user != null)
                    {
                        FormsAuthentication.SetAuthCookie(model.Name, true);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь с таким логином уже существует");
                }
            }

            return View(model);
        }
        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
	}
}