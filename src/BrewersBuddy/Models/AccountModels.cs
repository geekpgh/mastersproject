using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace BrewersBuddy.Models
{
	////////////////////CONTEXTS/////////////////////////////////////
	public class UsersContext : DbContext
	{
        public DbSet<UserProfile> UserProfiles { get; set; }
		public DbSet<webpages_Membership> webpages_Memberships { get; set; }
    }

    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
		public string UserName { get; set; }
		public string Email { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Zip { get; set; }
	}

	[Table("webpages_Membership")]
	public class webpages_Membership
	{
		[Key]
		public int UserId { get; set; }
		public DateTime CreateDate { get; set; }
		public string ConfirmationToken { get; set; }
		public bool IsConfirmed { get; set; }
		public DateTime LastPasswordFailureDate { get; set; }
		public int PasswordFailuresSinceLastSuccess { get; set; }
		public string Password { get; set; }
		public DateTime PasswordChangeDate { get; set; }
		public string PasswordSalt { get; set; }
		public string PasswordVerificationToken { get; set; }
		public DateTime PasswordVerificationTokenExpirationDate { get; set; }
	}

    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

	public enum ValidType
	{
		UserName,
		Email
	}

	public class RegisterModel
    {
		[Required]
		[Display(Name = "Email")]
		public string Email { get; set; }

        [Required]
		[Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

		[Display(Name = "First name")]
		public string FirstName { get; set; }

		[Display(Name = "Last name")]
		public string LastName { get; set; }

		[Display(Name = "City")]
		public string City { get; set; }

		[MinLength(2)]
		[MaxLengthAttribute(2)]
		[Display(Name = "State")]
		public string State { get; set; }

		[MinLength(5)]
		[MaxLengthAttribute(5)]
		[Display(Name = "Zip")]
		public string Zip { get; set; }
		
    }

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }
}
