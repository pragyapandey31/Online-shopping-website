using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using loginsession.Models;
using System.Net;
using System.Data.Entity;
using System.Collections.Generic;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;

namespace loginsession.Controllers
{[RequireHttps]
    public class AccountController : Controller
    {
        private AppDbContext DB = new AppDbContext();
        // private PostgreSQLDatabase _database;
        private readonly UserManager<AppUser> userManager;


        public AccountController()
        : this(Startup.UserManagerFactory.Invoke())
        {
        }

        public AccountController(UserManager<AppUser> userManager)
        {
            var provider = new DpapiDataProtectionProvider("Sample");
            this.userManager = userManager;
            userManager.UserTokenProvider = new DataProtectorTokenProvider<AppUser>(provider.Create("EmailConfirmation"));
        }


        private IAuthenticationManager GetAuthenticationManager()
        {
            var ctx = Request.GetOwinContext();
            return ctx.Authentication;
        }
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await userManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await userManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
           // AddErrors(result);
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
       
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model,string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                 string code = await userManager.GeneratePasswordResetTokenAsync(user.Id);
                 //var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                 //await userManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
              //  return Redirect(returnUrl);




                System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
                new System.Net.Mail.MailAddress("pragya.pandey15@gmail.com", "Web Registration"),
                new System.Net.Mail.MailAddress(user.Email));
                m.Subject = "Email confirmation";
                m.Body = string.Format("Dear {0} < BR /> Please reset your password by clicking< a href =\"{1}\" title =\"Reset Password\">{1}</a>",
                   user.UserName, Url.Action("ResetPassword", "Account",
                   new { userId = user.Id, code = code }, Request.Url.Scheme))
                    ;
                m.IsBodyHtml = true;
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com");

                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("pragya.pandey15@gmail.com", "pranavpandey");
                smtp.Send(m);
                return Redirect(returnUrl);

                //return View();
            }

            // If we got this far, something failed, redisplay form
            return View();
        }

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await userManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }



        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await GetAuthenticationManager().GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Index","StoreHome");

            }

           // if (DB.UserLogins.Single(x => x.ProviderKey == loginInfo.Login.ProviderKey) != null)
            //{

                var ul = DB.UserLogins.SingleOrDefault(x => x.ProviderKey == loginInfo.Login.ProviderKey);



            //var i=await userManager.FindBy
            if (ul != null)
            {
                var i = ul.UserId;
                if (i != null)
                {
                    var u = DB.Users.Single(y => y.Id == i);
                    var identity = await userManager.CreateIdentityAsync(
                        u, DefaultAuthenticationTypes.ApplicationCookie);

                    GetAuthenticationManager().SignIn(identity);


                    return Redirect(returnUrl);
                    // return View("~/Home/Index");
                    // return Url.Action("Index", "Home");
                    //return RedirectToAction("Index", "StoreHome");

                }

            }

            // Sign in the user with this external login provider if the user already has a login
            //var result = await Microsoft.AspNet.Identity.Owin.SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            //switch (result)
            //{
            //  case SignInStatus.Success:
            //    return RedirectToLocal(returnUrl);
            // case SignInStatus.LockedOut:
            //   return View("Lockout");
            //case SignInStatus.RequiresVerification:
            //  return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
            //case SignInStatus.Failure:
            //default:
            //  // If the user does not have an account, then prompt the user to create an account
            ViewBag.ReturnUrl = returnUrl;
            string returnUrl1 = returnUrl;
            ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
            return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await GetAuthenticationManager().GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new AppUser { UserName = model.Email, Email = model.Email };
                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await userManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        //  return RedirectToLocal(returnUrl);

                        await SignIn(user);
                         return Redirect(returnUrl);
                        // return View("Index");
                        //return RedirectToAction("Index", "StoreHome");
                    }
                }
                // AddErrors(result);
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult LogIn(string returnUrl)
        {
            var model = new LogInModel
            {
                ReturnUrl = returnUrl

            };
            ViewBag.returnUrl = returnUrl;
            return View(model);
        }



        // [AllowAnonymous]


        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> LogIn(LogInModel model, string returnUrl)
        {
            //string s = "ss";
            ViewBag.error = "ne";
            if (!ModelState.IsValid)
            {
                ViewBag.error = "error";
                return View(returnUrl);
            }

            var user = await userManager.FindAsync(model.Email, model.Password);

            if (user != null)
            {
                var identity = await userManager.CreateIdentityAsync(
                    user, DefaultAuthenticationTypes.ApplicationCookie);

                GetAuthenticationManager().SignIn(identity);

                // return Redirect(GetRedirectUrl(model.ReturnUrl));


                return Redirect(returnUrl);
            }

            // user authN failed
            ViewBag.error = "error";
            ModelState.AddModelError("", "Invalid email or password");
            //return Redirect(returnUrl);
           //return View("error");
            return RedirectToAction("error", "StoreHome");
        }


        private string GetRedirectUrl(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                return Url.Action("Index", "StoreHome");
            }


            return returnUrl;
        }


        public ActionResult LogOut(string returnUrl)
        {
            string url = this.Request.UrlReferrer.AbsolutePath;
            GetAuthenticationManager().SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return Redirect(returnUrl);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing && userManager != null)
            {
                userManager.Dispose();
            }
            base.Dispose(disposing);
        }


        [HttpGet]
        public ActionResult Register()
        {
            var model = new RegisterModel();



            return View(model);

        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return Redirect(returnUrl);
            }

            var user = new AppUser
            {
                Email = model.Email,
                UserName = model.Email,
                //Country = model.Country
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await SignIn(user);
                return Redirect(returnUrl);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }

            return View();
        }

        private async Task SignIn(AppUser user)
        {
            var identity = await userManager.CreateIdentityAsync(
                  user, DefaultAuthenticationTypes.ApplicationCookie);

          //  identity.AddClaim(new Claim(ClaimTypes.Country, user.Country));
            GetAuthenticationManager().SignIn(identity);

        }

        #region Helpers
        private const string XsrfKey = "XsrfId";


        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        #endregion
    }


}

    //
    // POST: /Account/ExternalLoginConfirmation
   
      

    
