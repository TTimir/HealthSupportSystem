using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HealthSupportSystem.Controllers
{
    public class PasswordRecoveryController : Controller
    {
        private HealthSupportSysdbEntities db = new HealthSupportSysdbEntities();

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ViewBag.Message = "Email is required.";
                return View();
            }

            var user = db.UserTables.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                // Generate a new random password
                string newPassword = GenerateRandomPassword(); // Implement this method

                // Update user's password in the database
                user.Password = newPassword; // Note: You should hash or encrypt this password before saving it.

                // Save changes to the database
                db.SaveChanges();

                // Pass the new password to the view
                ViewBag.NewPassword = newPassword;

                ViewBag.Message = "Your new password has been generated. Please make sure to note it down.";
            }
            else
            {
                ViewBag.Message = "Email not found.";
            }

            return View();
        }

        // Helper method to generate a random password
        private string GenerateRandomPassword()
        {
            // Implement your logic to generate a random password here
            // Example implementation:
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 8)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}