using BE_U2_W3_Pizzeria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BE_U2_W3_Pizzeria.Controllers
{
    public class HomeController : Controller
    {
        private ModelDbContext db = new ModelDbContext();
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        [HttpGet]
        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signup(Utenti nuovoUtente)
        {
            using (var context = new ModelDbContext())
            {
                context.Utenti.Add(nuovoUtente);
                context.SaveChanges();
            }
          
            return RedirectToAction("Login");
        }

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            using (var context = new ModelDbContext())
            {
                var user = context.Utenti.FirstOrDefault(u => u.Username == username && u.Password == password);
                if (user != null)
                {
                    System.Diagnostics.Debug.WriteLine("Errore: ");
                    FormsAuthentication.SetAuthCookie(username, false);
                    ViewBag.AuthSuccess = "Login effettuato con successo";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.AuthError = "Username o password errati";
                    return View();
                }
            }
        }

        public ActionResult Benvenuto()
        {
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            TempData["LoginMess"] = "Sei stato disconesso";
            return RedirectToAction("Index", "Home");
        }

    }
}
