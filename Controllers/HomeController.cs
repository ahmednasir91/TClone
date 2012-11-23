using System.Web.Mvc;
using TwitterClone.Context;

namespace TwitterClone.Controllers
{
    public class HomeController : Controller
    {
        public Repository userRepo = new Repository();
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
            return Json(username, JsonRequestBehavior.AllowGet);
        }

    }

}
