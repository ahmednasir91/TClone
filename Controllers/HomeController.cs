﻿using System.Web.Mvc;
using TwitterClone.Context;

namespace TwitterClone.Controllers
{
    public class HomeController : Controller
    {
        public Repository userRepo = new Repository();

        public ActionResult NotFound()
        {

            return View("NotFound", userRepo.GetCurrentUser());
            //ViewBag.Message = "Sorry Page not found!";
           // return View();
            //Replace with return View("NotFound"); and create a page with a layout using _Layout.cshtml
        }

        public ActionResult Index()
        {
            var model = userRepo.GetUser(User.Identity.Name);
            return User.Identity.IsAuthenticated ? View("LoggedIn", model) : View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Show(string username)
        {
            var user = userRepo.GetUser(username);
            if (user == null)
                return Redirect("/404");
            return Json(username, JsonRequestBehavior.AllowGet);
        }

    }

}
