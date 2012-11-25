using System.Linq;
using System.Web.Mvc;
using TwitterClone.Context;
using TwitterClone.Entities;

namespace TwitterClone.Controllers
{
    public class HomeController : Controller
    {
        public Repository userRepo = new Repository();

        public ActionResult NotFound()
        {
            return View("NotFound", userRepo.GetCurrentUser()); 
        }

        public ActionResult Index()
        {
            ViewBag.List = "TweetsList";
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

        public ActionResult Following(string username)
        {
            ViewBag.List = "Following";
            return View("Show", userRepo.GetUser(username));
        }

        public ActionResult Followers(string username)
        {
            ViewBag.List = "Followers";
            return View("Show", userRepo.GetUser(username));
        }

        public ActionResult Favorites(string username)
        {
            ViewBag.List = "Favorites";
            return View("Show", userRepo.GetUser(username));
        }

        public ActionResult Lists(string username)
        {
            ViewBag.List = "Lists";
            var user = userRepo.GetUser(username);
            if (user == null)
                return Redirect("/404");
            user.Lists = userRepo.Lists(username);
            return View("Show", user);
        }
        [HttpPost]
        public ActionResult NewList(List list)
        {
            if (ModelState.IsValid)
            {
                userRepo.NewList(list);
                Redirect("/" + User.Identity.Name + "/lists");
            }
            ViewBag.Error = ModelState.Keys.SelectMany(key => ModelState[key].Errors).ToList();
            return Redirect("/" + User.Identity.Name + "/lists");
        }
    }

}
