using System.Web.Mvc;
using TwitterClone.Context;

namespace TwitterClone.Controllers
{
    public class HomeController : Controller
    {
        public Repository userRepo = new Repository();

        public ActionResult NotFound()
        {
            return Json("Page not found!", JsonRequestBehavior.AllowGet); //Replace with return View("NotFound"); and create a page with a layout using _Layout.cshtml
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
            ViewBag.List = "TweetsList";
            var user = userRepo.GetUser(username);
            if (user == null)
                return Redirect("/404");
            return View(user);
        }

        public ActionResult Following()
        {
            ViewBag.List = "Following";
            return View("Show", userRepo.GetCurrentUser());
        }

        public ActionResult Followers()
        {
            ViewBag.List = "Followers";
            return View("Show", userRepo.GetCurrentUser());
        }

    }

}
