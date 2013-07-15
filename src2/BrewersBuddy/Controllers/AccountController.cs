using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Transactions;
using System.Web.Mvc;
using System.Web.Security;
using BrewersBuddy.Models;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;

namespace BrewersBuddy.Controllers
{
	[Authorize]
	public class AccountController : Controller
	{
		#region Properties
		private BrewersBuddyContext db = new BrewersBuddyContext();

		#endregion Properties

		#region Login
		//
		// GET: /Account/Login

		[AllowAnonymous]
		public ActionResult Login(string returnUrl)
		{
			ViewBag.ReturnUrl = returnUrl;
			return View();
		}

		//
		// POST: /Account/Login

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult Login(LoginModel model, string returnUrl)
		{
			if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
			{
				return RedirectToLocal(returnUrl);
			}

			// If we got this far, something failed, redisplay form
			ModelState.AddModelError("", "The user name or password provided is incorrect.");
			return View(model);
		}

		//
		// POST: /Account/ExternalLogin

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult ExternalLogin(string provider, string returnUrl)
		{
			return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
		}

		//
		// GET: /Account/ExternalLoginCallback

		[AllowAnonymous]
		public ActionResult ExternalLoginCallback(string returnUrl)
		{
			AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
			if (!result.IsSuccessful)
			{
				return RedirectToAction("ExternalLoginFailure");
			}

			if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
			{
				return RedirectToLocal(returnUrl);
			}

			if (User.Identity.IsAuthenticated)
			{
				// If the current user is logged in add the new account
				OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
				return RedirectToLocal(returnUrl);
			}
			else
			{
				// User is new, ask for their desired membership name
				string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
				ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
				ViewBag.ReturnUrl = returnUrl;
				return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
			}
		}

		//
		// POST: /Account/ExternalLoginConfirmation

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
		{
			string provider = null;
			string providerUserId = null;

			if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
			{
				return RedirectToAction("Manage");
			}

			if (ModelState.IsValid)
			{
				// Insert a new user into the database
				using (BrewersBuddyContext db = new BrewersBuddyContext())
				{
					UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
					// Check if user already exists
					if (user == null)
					{
						// Insert name into the profile table
						db.UserProfiles.Add(new UserProfile { UserName = model.UserName });
						db.SaveChanges();

						OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
						OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

						return RedirectToLocal(returnUrl);
					}
					else
					{
						ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
					}
				}
			}

			ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
			ViewBag.ReturnUrl = returnUrl;
			return View(model);
		}

		//
		// GET: /Account/ExternalLoginFailure

		[AllowAnonymous]
		public ActionResult ExternalLoginFailure()
		{
			return View();
		}

		[AllowAnonymous]
		[ChildActionOnly]
		public ActionResult ExternalLoginsList(string returnUrl)
		{
			ViewBag.ReturnUrl = returnUrl;
			return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
		}

		[ChildActionOnly]
		public ActionResult RemoveExternalLogins()
		{
			ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
			List<ExternalLogin> externalLogins = new List<ExternalLogin>();
			foreach (OAuthAccount account in accounts)
			{
				AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

				externalLogins.Add(new ExternalLogin
				{
					Provider = account.Provider,
					ProviderDisplayName = clientData.DisplayName,
					ProviderUserId = account.ProviderUserId,
				});
			}

			ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
			return PartialView("_RemoveExternalLoginsPartial", externalLogins);
		}

		#endregion Login

		#region LogOff
		//
		// POST: /Account/LogOff

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult LogOff()
		{
			WebSecurity.Logout();

			return RedirectToAction("Index", "Home");
		}

		//
		// POST: /Account/Disassociate

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Disassociate(string provider, string providerUserId)
		{
			string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
			ManageMessageId? message = null;

			// Only disassociate the account if the currently logged in user is the owner
			if (ownerAccount == User.Identity.Name)
			{
				// Use a transaction to prevent the user from deleting their last login credential
				using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
				{
					bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
					if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
					{
						OAuthWebSecurity.DeleteAccount(provider, providerUserId);
						scope.Complete();
						message = ManageMessageId.RemoveLoginSuccess;
					}
				}
			}

			return RedirectToAction("Manage", new { Message = message });
		}

		#endregion LogOff

		#region Register
		//
		// GET: /Account/Register

		[AllowAnonymous]
		public ActionResult Register()
		{
			return View();
		}

		//
		// POST: /Account/Register

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult Register(RegisterModel model)
		{
			GetBrokenRulesFor(model);
			if (ModelState.IsValid)
			{
				// Attempt to register the user
				try
				{
					WebSecurity.CreateUserAndAccount(model.UserName, model.Password, propertyValues: new { Email = model.Email, FirstName = model.FirstName, LastName = model.LastName, City = model.City, State = model.State, Zip = model.Zip });
					WebSecurity.Login(model.UserName, model.Password);
					return RedirectToAction("Index", "Home");
				}
				catch (MembershipCreateUserException e)
				{
					ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
				}
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		#endregion Register

		#region Edit Account
		//
		// GET: /Account/Edit/

		public ActionResult Edit()
		{
			TempData["Success"] = string.Empty;
			foreach (UserProfile UP in db.UserProfiles)
			{
				if (UP.UserName == User.Identity.Name)
				{
					return View(UP);
				}
			}

			return HttpNotFound();
		}

		//
		// POST: /Account/Edit/5

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(UserProfile userProfile)
		{
			if (ModelState.IsValid)
			{
				db.Entry(userProfile).State = System.Data.EntityState.Modified;
				db.SaveChanges();

				TempData["Success"] = "Save Successful";
				return View(userProfile);
			}

			ModelState.AddModelError("", "Error saving changes to user account.");
			return View(userProfile);
		}

		#endregion Edit Account

		#region Delete Account
		//
		// GET: /Account/Delete/


		public ActionResult Delete()
		{
			BrewersBuddyContext db = new BrewersBuddyContext();

			var currentUser = User.Identity.Name;
			UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == currentUser.ToLower());

			if (user == null)
			{
				// Some problem occured
				return HttpNotFound();
			}
			return View(user);
		}


		//
		// POST: /Account/Delete/

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(UserProfile user)
		{
			WebSecurity.Logout();
			OAuthWebSecurity.DeleteAccount(user.UserName, user.UserId.ToString());

			UserProfile user1 = db.UserProfiles.Find(user.UserId);

			// Remove all user batches
			var ownedBatches = from batch in db.Batches
							   where (batch.OwnerId == user.UserId)
							   select batch;
			if (ownedBatches != null)
			{
				var list = ownedBatches.ToList();
				for (int i = 0; i < list.Count; i++)
				{
					db.Batches.Remove(list[i]);
				}
			}

			// Remove user                            
			db.UserProfiles.Remove(user1);
			db.SaveChanges();

			return RedirectToAction("..");
		}

		#endregion Delete Account

		#region Search Accounts
		public ActionResult SearchIndex(string username,
		string firstname, string lastname, string zip)
		{
			IEnumerable<UserProfile> users = from u in db.UserProfiles select u;
			UserProfile currentUser = users.First(s => s.UserName == User.Identity.Name);

			TempData["FirstLoad"] = false;
			if (String.IsNullOrWhiteSpace(username) && String.IsNullOrWhiteSpace(firstname)
			&& String.IsNullOrWhiteSpace(lastname) && String.IsNullOrWhiteSpace(zip))
			{
				users = users.Where(s => s.Zip == "-1");
			}
			else
			{
				if (!String.IsNullOrEmpty(username))
				{
					users = users.Where(s => s.UserName == username);
				}
				if (!String.IsNullOrEmpty(firstname))
				{
					users = users.Where(s => s.FirstName == firstname);
				}
				if (!String.IsNullOrEmpty(lastname))
				{
					users = users.Where(s => s.LastName == lastname);
				}
				if (!String.IsNullOrEmpty(zip))
				{
					users = users.Where(s => s.Zip == zip);
				}
			}

			users = users.Where(s => s.UserName != currentUser.UserName);
			foreach (UserProfile UP in users)
			{
				if (currentUser.Friends.Contains(UP))
				{
					users = users.Where(s => s.UserId != UP.UserId);
				}
			}
			return View(users.ToList());
		}

		#endregion Search Accounts

		#region Friends List Actions
		public ActionResult FriendsList(int ID = 0)
		{
			UserProfile main = (UserProfile)db.UserProfiles.First(q => q.UserName == User.Identity.Name);
			if (ID > 0)
			{
				if (!main.Friends.Any(item => item.UserId == ID))
				{
					main.Friends.Add((UserProfile)db.UserProfiles.First(p => p.UserId == ID));
					db.Entry(main).State = System.Data.EntityState.Modified;
					db.SaveChanges();

					return View(main.Friends);
				}
			}

			return View(main.Friends);
		}

		public ActionResult RemoveFriend(int ID = 0)
		{
			UserProfile main = (UserProfile)db.UserProfiles.First(q => q.UserName == User.Identity.Name);

			if (main.Friends.Any(item => item.UserId == ID))
			{
				main.Friends.Remove((UserProfile)db.UserProfiles.First(p => p.UserId == ID));
				db.Entry(main).State = System.Data.EntityState.Modified;
				db.SaveChanges();

				return RedirectToAction("FriendsList");
			}

			return RedirectToAction("FriendsList");
		}

		#endregion Friends List Actions

		#region Manage
		//
		// GET: /Account/Manage

		public ActionResult Manage(ManageMessageId? message)
		{
			ViewBag.StatusMessage =
				message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
				: message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
				: message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
				: "";
			ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
			ViewBag.ReturnUrl = Url.Action("Manage");
			return View();
		}

		//
		// POST: /Account/Manage

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Manage(LocalPasswordModel model)
		{
			bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
			ViewBag.HasLocalPassword = hasLocalAccount;
			ViewBag.ReturnUrl = Url.Action("Manage");
			if (hasLocalAccount)
			{
				if (ModelState.IsValid)
				{
					// ChangePassword will throw an exception rather than return false in certain failure scenarios.
					bool changePasswordSucceeded;
					try
					{
						changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
					}
					catch (Exception)
					{
						changePasswordSucceeded = false;
					}

					if (changePasswordSucceeded)
					{
						return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
					}
					else
					{
						ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
					}
				}
			}
			else
			{
				// User does not have a local password so remove any validation errors caused by a missing
				// OldPassword field
				ModelState state = ModelState["OldPassword"];
				if (state != null)
				{
					state.Errors.Clear();
				}

				if (ModelState.IsValid)
				{
					try
					{
						WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
						return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
					}
					catch (Exception)
					{
						ModelState.AddModelError("", String.Format("Unable to create local account. An account with the name \"{0}\" may already exist.", User.Identity.Name));
					}
				}
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		//
		// GET: Account/ForgotPassword
		//

		[AllowAnonymous]
		public ActionResult ForgotPassword()
		{
			return View();
		}

		//
		// POST: Account/ForgotPassword
		//

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult ForgotPassword(string UserName)
		{
			//check user existance
			var user = Membership.GetUser(UserName);
			if (user == null)
			{
				TempData["Message"] = "User Not exist.";
			}
			else
			{
				//generate password token
				var token = WebSecurity.GeneratePasswordResetToken(UserName);
				//create url with above token
				var resetLink = "<a href='" + Url.Action("ResetPassword", "Account", new { un = UserName, rt = token }, "http") + "'>Reset Password</a>";
				//get user emailid
				BrewersBuddyContext db = new BrewersBuddyContext();
				var emailid = (from i in db.UserProfiles
							   where i.UserName == UserName
							   select i.Email).FirstOrDefault();
				//send mail
				string subject = "Password Reset Token";
				string body = "<b>Please find the Password Reset Token</b><br/>" + resetLink; //edit it
				try
				{
					SendEMail(emailid, subject, body);
					TempData["Message"] = "Mail Sent.";
				}
				catch (Exception ex)
				{
					TempData["Message"] = "Error occured while sending email." + ex.Message;
				}
			}

			return View();
		}

		//
		// GET: Account/Login
		//
		[AllowAnonymous]
		public ActionResult ResetPassword(string un, string rt)
		{
			BrewersBuddyContext db = new BrewersBuddyContext();
			//TODO: Check the un and rt matching and then perform following
			//get userid of received username
			var userid = (from i in db.UserProfiles
						  where i.UserName == un
						  select i.UserId).FirstOrDefault();
			//check userid and token matches
			bool any = (from j in db.webpages_Memberships
						where (j.UserId == userid)
						&& (j.PasswordVerificationToken == rt)
						//&& (j.PasswordVerificationTokenExpirationDate < DateTime.Now)
						select j).Any();

			if (any == true)
			{
				//generate random password
				string newpassword = GenerateRandomPassword(8);
				//reset password
				bool response = WebSecurity.ResetPassword(rt, newpassword);
				if (response == true)
				{
					//get user emailid to send password
					var emailid = (from i in db.UserProfiles
								   where i.UserName == un
								   select i.Email).FirstOrDefault();
					//send email
					string subject = "New Password";
					string body = "<b>Please find the New Password</b><br/>" + newpassword; //edit it
					try
					{
						SendEMail(emailid, subject, body);
						TempData["Message"] = "Success! Check email we sent for new password.";
					}
					catch (Exception ex)
					{
						TempData["Message"] = "Error occured while sending email." + ex.Message;
					}
				}
				else
				{
					TempData["Message"] = "Hey, avoid random request on this page.";
				}
			}
			else
			{
				TempData["Message"] = "Username and token not maching.";
			}

			return View();
		}

		private string GenerateRandomPassword(int length)
		{
			string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-*&#+";
			char[] chars = new char[length];
			Random rd = new Random();
			for (int i = 0; i < length; i++)
			{
				chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
			}
			return new string(chars);
		}

		private void SendEMail(string emailid, string subject, string body)
		{
			var client = new SmtpClient("smtp.gmail.com", 587);
			client.Credentials = new NetworkCredential("brewersbuddy@gmail.com", "sweng500");
			client.EnableSsl = true;

			MailMessage msg = new MailMessage("Noreply@BrewersBuddy.com", emailid);

			msg.Subject = subject;
			msg.IsBodyHtml = true;
			msg.Body = body + "<br/><br/>*** DO NOT REPLY TO THIS EMAIL.  THIS EMAIL IS NOT CHECKED. ***";

			client.Send(msg);
		}

		#endregion Manage

		#region Helpers
		private void GetBrokenRulesFor(RegisterModel model)
		{
			if (!IsValid(model, ValidType.UserName))
			{
				ModelState.AddModelError("", ErrorCodeToString(MembershipCreateStatus.DuplicateUserName));
			}
			if (!IsValid(model, ValidType.Email))
			{
				ModelState.AddModelError("", ErrorCodeToString(MembershipCreateStatus.DuplicateEmail));
			}
		}

		public bool IsValid(RegisterModel model, ValidType validType)
		{
			foreach (UserProfile UP in db.UserProfiles.ToList())
			{
				if (validType == ValidType.UserName)
				{
					if (UP.UserName == model.UserName)
					{
						return false;
					}
				}
				if (validType == ValidType.Email)
				{
					if (UP.Email == model.Email)
					{
						return false;
					}
				}
			}

			return true;
		}

		private ActionResult RedirectToLocal(string returnUrl)
		{
			if (Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}
			else
			{
				return RedirectToAction("Index", "Home");
			}
		}

		public enum ManageMessageId
		{
			ChangePasswordSuccess,
			SetPasswordSuccess,
			RemoveLoginSuccess,
		}

		internal class ExternalLoginResult : ActionResult
		{
			public ExternalLoginResult(string provider, string returnUrl)
			{
				Provider = provider;
				ReturnUrl = returnUrl;
			}

			public string Provider { get; private set; }
			public string ReturnUrl { get; private set; }

			public override void ExecuteResult(ControllerContext context)
			{
				OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
			}
		}

		private static string ErrorCodeToString(MembershipCreateStatus createStatus)
		{
			// See http://go.microsoft.com/fwlink/?LinkID=177550 for
			// a full list of status codes.
			switch (createStatus)
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
