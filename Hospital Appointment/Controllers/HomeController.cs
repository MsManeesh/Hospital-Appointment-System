using Hospital_Appointment.DAL;
using Hospital_Appointment.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Hospital_Appointment.Controllers
{
    public class HomeController : Controller
    {
        Login login= new Login();
        UserDbHandler userDb= new UserDbHandler();
        public ActionResult Index()
        {
            if(HttpContext.User.Identity.IsAuthenticated)
                return RedirectToAction("Dasboard");
            return View();
        }
        [Authorize]
        public ActionResult Dasboard()
        {
            try
            {
                User user = new User();
                user = userDb.GetSingleUserDetails(HttpContext.User.Identity.Name);
                return View(user);
            }catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message + " " + ex.StackTrace);
                return View();
            }
        }
        [HttpPost]
        public ActionResult Index(Login login)
        {
            try {
                FormsAuthentication.SignOut();
                if (ModelState.IsValid)
                {
                    User user = userDb.GetSingleUserDetails(login.Email);
                    if (user != null)
                    {
                        if (login.Email == user.Email && login.Password == user.Password)
                        {
                            FormsAuthentication.SetAuthCookie(login.Email, true);
                            return RedirectToAction("Dasboard", "Home");
                        }
                        ModelState.AddModelError("", "Invalid Password ");
                        return View();
                    }
                    ModelState.AddModelError("", "Not a Registered User");
                    return View();
                }
                ModelState.AddModelError("", "Please provide the Email and Password");
                return View();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View();
            }

            ;
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(Login login)
        {
            try
            {
                
                //var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

                var user = userDb.GetSingleUserDetails(login.Email);
                if (user != null)
                {
                    //Send email for reset password
                    string resetCode = Guid.NewGuid().ToString();
                    SendVerificationLinkEmail(user.Email, resetCode, "ResetPassword");
                    if (userDb.AddResetPasswordCode(resetCode,user.Id))
                    {
                        return RedirectToAction("Index");
                    }
                    //account.ResetPasswordCode = resetCode;
                    //This line I have added here to avoid confirm password not match issue , as we had added a confirm password property 
                    //in our model class in part 1
                    //dc.Configuration.ValidateOnSaveEnabled = false;
                    //dc.SaveChanges();
                    //message = "Reset password link has been sent to your email id.";
                    ModelState.AddModelError("", "Server Error");
                    return View(login);
                }
                else
                {
                    ModelState.AddModelError("", "Account doesnot exist");
                    return View(login);
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(login);
            }
                
            
            
        }


        public void SendVerificationLinkEmail(string emailID, string activationCode, string emailFor = "VerifyAccount")
        {
            var verifyUrl = "/Home/" + emailFor + "/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress(ConfigurationManager.AppSettings.Get("FromEmail"), "Dotnet Awesome");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = ConfigurationManager.AppSettings.Get("Password"); // Replace with actual password

            string subject = "";
            string body = "";
            if (emailFor == "VerifyAccount")
            {
                subject = "Your account is successfully created!";
                body = "<br/><br/>We are excited to tell you that your Dotnet Awesome account is" +
                    " successfully created. Please click on the below link to verify your account" +
                    " <br/><br/><a href='" + link + "'>" + link + "</a> ";
            }
            else if (emailFor == "ResetPassword")
            {
                subject = "Reset Password";
                body = "Hi,<br/>br/>We got request for reset your account password. Please click on the below link to reset your password" +
                    "<br/><br/><a href=" + link + ">Reset Password link</a>";
            }


            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword),
                EnableSsl = true,
            //UseDefaultCredentials = true,
            //Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
        };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }

        public ActionResult ResetPassword(string id)
        {
            //Verify the reset password link
            //Find account associated with this link
            //redirect to reset password page
            if (string.IsNullOrWhiteSpace(id))
            {
                return HttpNotFound();
            }


            var user = userDb.GetUsersByResetPasswordCode(id);
                if (user != null)
                {
                    ResetPasswordModel model = new ResetPasswordModel();
                    model.ResetCode = id;
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            var message = "";
            if (ModelState.IsValid)
            {
               
                    var user = userDb.GetUsersByResetPasswordCode(model.ResetCode);
                    if (user != null)
                    {
                        if (userDb.UpdatePassword(model.NewPassword, user.Id))
                        {
                            if (userDb.AddResetPasswordCode("", user.Id))
                            {
                            message = "New password updated successfully";
                            }
                        }
                      
                    }
                
            }
            else
            {
                message = "Something invalid";
            }
            ViewBag.Message = message;
            return View(model);
        }
    }
}