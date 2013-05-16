using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BrewersBuddy;
using BrewersBuddy.Controllers;
using BrewersBuddy.Models;

namespace BrewersBuddy.Tests.Controllers
{
	[TestClass]
	public class UserAccountTests
	{
		[TestMethod]
		public void RegisterUser()
		{
			// Arrange
			AccountController controller = new AccountController();

			// Act
			RegisterModel registerModel = new RegisterModel();
			registerModel.Email = "gmblogref@hotmail.com";
			registerModel.UserName = "gmblogref";
			registerModel.Password = "123456";
			registerModel.ConfirmPassword = "123456";
			ViewResult result = controller.Register(registerModel) as ViewResult;

			// Assert
			Assert.AreEqual("Logged in username is gmblogref", result.ViewBag.Message);
			controller.LogOff();
		}

		[TestMethod]
		public void Login()
		{
			// Arrange
			AccountController controller = new AccountController();

			// Act
			LoginModel loginModel = new LoginModel();
			loginModel.UserName = "gmblogref";
			loginModel.Password = "123456";
			ViewResult result = controller.Login(loginModel, "URL") as ViewResult;

			// Assert
			Assert.AreEqual("Logged in username is gmblogref", result.ViewBag.Message);
			controller.LogOff();
		}

		[TestMethod]
		public void LogOff()
		{
			// Arrange
			AccountController controller = new AccountController();

			// Act
			LoginModel loginModel = new LoginModel();
			loginModel.UserName = "gmblogref";
			loginModel.Password = "123456";
			controller.Login(loginModel, "URL");
			ViewResult result = controller.LogOff() as ViewResult;

			// Assert
			Assert.AreEqual("gmblogref is logged off", result.ViewBag.Message);
		}

		[TestMethod]
		public void RecoverPassword()
		{
			// Arrange
			AccountController controller = new AccountController();

			// Act
			ViewResult result = controller.RecoverPassword("gmblogref") as ViewResult;

			// Assert
			Assert.AreEqual("An email with your password has been sent.", result.ViewBag.Message);
		}
	}
}
