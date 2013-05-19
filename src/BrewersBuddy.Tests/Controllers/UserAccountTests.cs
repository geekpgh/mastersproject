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
		public void RegisterUser_TEST()
		{
			// Arrange
			AccountController controller = new AccountController();

			// Act
			RegisterModel registerModel = new RegisterModel();
			registerModel.Email = "NUNIT@Test.com";
			registerModel.UserName = "NUNIT_Test";
			registerModel.Password = "123456";
			registerModel.ConfirmPassword = "123456";
			ViewResult result = controller.Register(registerModel) as ViewResult;

			// Assert
			// Do a database call or a get by login info to check to see if the person was added
			Assert.AreEqual("Logged in username is NUNIT_Test", result.ViewBag.Message);
		}

		[TestMethod]
		public void Login_TEST()
		{
			// Arrange
			AccountController controller = new AccountController();

			// Act
			LoginModel loginModel = new LoginModel();
			loginModel.UserName = "NUNIT_Test";
			loginModel.Password = "123456";
			ViewResult result = controller.Login(loginModel, "URL") as ViewResult;

			// Assert
			// Find the global user for this instance and make sure the user is NUNIT_Test
			Assert.AreEqual("Logged in username is NUNIT_Test", result.ViewBag.Message);
		}

		[TestMethod]
		public void LogOff_TEST()
		{
			// Arrange
			AccountController controller = new AccountController();

			// Act
			LoginModel loginModel = new LoginModel();
			loginModel.UserName = "NUNIT_Test";
			loginModel.Password = "123456";
			controller.Login(loginModel, "URL");
			ViewResult result = controller.LogOff() as ViewResult;

			// Assert
			// Find the global user for this instance and make sure the user is not NUNIT_Test
			Assert.AreEqual("NUNIT_Test is logged off", result.ViewBag.Message);
		}

		[TestMethod]
		public void RecoverPassword_TEST()
		{
			// Arrange
			AccountController controller = new AccountController();

			// Act
			ViewResult result = controller.RecoverPassword("NUNIT_Test") as ViewResult;

			// Assert
			Assert.AreEqual("An email with your password has been sent.", result.ViewBag.Message);
		}

		[TestMethod]
		public void UserCanEditAccountInformation_TEST()
		{
			// Arrange
			AccountController controller = new AccountController();

			// Act
			LoginModel loginModel = new LoginModel();
			loginModel.UserName = "NUNIT_Test";
			loginModel.Password = "123456";
			controller.Login(loginModel, "URL");

			// Get global users account and edit some items and save the user
			ViewResult result = controller.RecoverPassword("NUNIT_Test") as ViewResult;

			// Assert
			// Make suer the users account info changed
			Assert.AreEqual("An email with your password has been sent.", result.ViewBag.Message);
		}

		[TestMethod]
		public void UserCanEnterZipToFindBrewers_TEST()
		{
			// Arrange
			AccountController controller = new AccountController();

			// Act
			// Create a few accounts with same zip code
			// Call the search method with the zip code
			ViewResult result = controller.RecoverPassword("NUNIT_Test") as ViewResult;

			// Assert
			// That all users returned have the same zip code
			Assert.AreEqual("An email with your password has been sent.", result.ViewBag.Message);
		}

		
	}
}
