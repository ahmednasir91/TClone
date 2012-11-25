using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TwitterClone.Context;
using TwitterClone.Entities;
using TwitterClone.Membership;
using TwitterClone.Models;

namespace TwitterClone.Controllers
{

    [Authorize]
    public class AccountController : Controller
    {
        public Repository Repository = new Repository();

        [HttpPost]
        public ActionResult UpdateDesign(User user)
        {
            ViewBag.Settings = "Design";
            return Json(Repository.UpdateDesign(user) ? "Settings Saved!" : "Error changing settings");
        }

        [HttpPost]
        public ActionResult UploadBackground(HttpPostedFileBase BackgroundImage)
        {
            if (BackgroundImage != null && IsImage(BackgroundImage) && BackgroundImage.ContentLength > 0)
                return Json(Repository.SaveBackground(BackgroundImage));
            return Json("");
        }

        public ActionResult ProfileSettings(string Settings)
        {
            ViewBag.Settings = Settings;
            return View(Repository.GetCurrentUser());
        }
        [HttpPost]
        public ActionResult AccountSettings(User user)
        {
            ViewBag.Settings = "Account";
            if (ModelState.IsValidField("Email") && ModelState.IsValidField("Username"))
            {
                Repository.UpdateUserBasic(user);
                ViewBag.Info = "Settings Saved!";
            }
            if(String.Equals(user.Username, User.Identity.Name))
                return View("ProfileSettings", Repository.GetCurrentUser());
            return RedirectToAction("SignOut");
        }

        [HttpPost]
        public ActionResult ProfileSettings(User user, HttpPostedFileBase Avatar)
        {
            if (Avatar != null && IsImage(Avatar) && Avatar.ContentLength > 0)
                Repository.SaveAvatar(Avatar);
            if (ModelState.IsValidField("FullName") && ModelState.IsValidField("Location") && ModelState.IsValidField("WebsiteURL") && ModelState.IsValidField("Bio"))
                Repository.UpdateUserInfo(user);
            ViewBag.Info = "Settings Saved!";
            ViewBag.Settings = "Profile";
            return View("ProfileSettings", Repository.GetCurrentUser());
        }

        [HttpPost]
        public ActionResult DesignSettings(User user)
        {
            ViewBag.Settings = "Design";
            return View("ProfileSettings", Repository.GetCurrentUser());
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult RegisterHome(RegisterHomeModel model)
        {

            if(model == null)
                model = new RegisterHomeModel();
            var registerModel = new RegisterModel
                                    {
                                        Email = model.Email ?? String.Empty,
                                        UserName = Regex.Replace(model.Name ?? String.Empty, "[^0-9A-Za-z]", ""),
                                        Password = model.PasswordFinal ?? String.Empty,
                                        FullName = model.Name ?? String.Empty
                                    };
            return View("Register", registerModel);
        }


        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View("Login");
        }

        //
        // POST: /Account/Login

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if(ModelState.IsValid)
            {
                if(System.Web.Security.Membership.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/SignOut

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if(ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus;
                System.Web.Security.Membership.CreateUser(model.UserName, model.Password, model.Email, model.FullName, passwordAnswer: null, isApproved: true, providerUserKey: null, status: out createStatus);

                if(createStatus == MembershipCreateStatus.Success)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, createPersistentCookie: false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePassword

        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if(ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = System.Web.Security.Membership.GetUser(User.Identity.Name, userIsOnline: true);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch(Exception)
                {
                    changePasswordSucceeded = false;
                }

                if(changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        private IEnumerable<string> GetErrorsFromModelState()
        {
            return ModelState.SelectMany(x => x.Value.Errors.Select(error => error.ErrorMessage));
        }

        private bool IsImage(HttpPostedFileBase file)
        {
            if (file.ContentType.Contains("image"))
            {
                return true;
            }

            string[] formats = new string[] { ".jpg", ".png", ".gif", ".jpeg" }; // add more if u like...

            // linq from Henrik Stenbæk
            return formats.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch(createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
