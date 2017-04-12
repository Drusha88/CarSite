using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Models;
using Domain.Entities;
using Domain.Abstract;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;

namespace WebUI.Controllers
{
    public class UserController : Controller
    {
        IUserRepository repository;
        IEmailProcessor emailProcessor;
        public UserController(IUserRepository repository, IEmailProcessor emailProcessor)
        {
            this.repository = repository;
            this.emailProcessor = emailProcessor;
        }
        public ViewResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel userInfo)
        {

            if (ModelState.IsValid)
            {
                //&& u.Password == userInfo.Password
                User user = repository.Users.FirstOrDefault(u => u.Email == userInfo.Email);

                if (user != null)
                {
                    if (user.Confirm)
                    {
                        if (user.Password == userInfo.Password)
                        {

                            MD5CryptoServiceProvider md5Provider = new MD5CryptoServiceProvider();
                            byte[] hashCode = md5Provider.ComputeHash(Encoding.Default.GetBytes(user.Login + ConfigurationManager.AppSettings["Key"]));
                            string sign = BitConverter.ToString(hashCode).ToLower().Replace("-", "");

                            Session["Login"] = user.Login;
                            Session["Sign"] = sign;


                            return RedirectToAction("List", "Cars");
                        }
                        else
                            ModelState.AddModelError("Password", "Неправильный Password");
                    }
                    else
                        ModelState.AddModelError("", "Аккаунт не подтвержден. Ссылка с активацией была отправленна на Ваш Email.");
                }
                else
                    ModelState.AddModelError("Email", "Email не найден.");
            }
            return View(new LoginViewModel());
        }

        public ViewResult Registration()
        {
            return View(new User());
        }

        [HttpPost]
        public ViewResult Registration(User user)
        {


            if (ModelState.IsValid)
            {
                bool login = repository.Users.FirstOrDefault(u => u.Login == user.Login) == null ? false : true;

                if (!login)
                {
                    bool email = repository.Users.FirstOrDefault(u => u.Email == user.Email) == null ? false : true;

                    if (!email)
                    {
                        User savedUser = repository.SaveUser(user);
                        emailProcessor.SendEmail(savedUser, Request.Url.Authority);
                        ViewBag.Text = "Для завершения регистрации перейдите по ссылке отправленной вам на Ваш email.";
                        return View("Completed");
                    }
                    else
                    {
                        ModelState.AddModelError("Email", "Данный Email уже зарегестрирован.");
   
                    }

                }
                else
                {
                    ModelState.AddModelError("Login", "Login занят.");

                }

            }
            return View(new User());
        }

        public ActionResult Confirm(string ticket)
        {
            if (repository.FindTicket(ticket))
            {
                ViewBag.Text = "Email подтвержден.";
            }
            else
            {
                ViewBag.Text = "Email не подтвержден, либо данная ссылка устарела.";
            }
            return View("Completed");
        }

        public RedirectResult Logout()
        {
            Session.Clear();
            
            return Redirect("/User/Login");
        
        }

        public PartialViewResult Info()
        {
            string login = (string)Session["Login"];

            ViewBag.login = login;

            return PartialView();
        }


    }
}