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

		[Display(Name = "State")]
		public string State { get; set; }

		[Display(Name = "Zip")]
		public string Zip { get; set; }

		public bool IsValid(ValidType validType)
		{
			using (var cn = new SqlConnection(@"Data Source=(localdb)\v11.0;Initial Catalog=aspnet-BrewersBuddy-20130514214341;" +
			  @"Integrated Security=True;" +
			  @"Connect Timeout=15;Encrypt=False;TrustServerCertificate=False"))
			{
				string _sql = string.Empty;

				if (validType == ValidType.UserName)
				{
					_sql = @"SELECT [Username] FROM [dbo].[UserProfile] " +
						   @"WHERE [Username] = @u";
				}
				else
				{
					_sql = @"SELECT [Email] FROM [dbo].[UserProfile] " +
					   @"WHERE [Email] = @u";
				}

				var cmd = new SqlCommand(_sql, cn);
				if (validType == ValidType.UserName)
				{
					cmd.Parameters.Add(new SqlParameter("@u", SqlDbType.NVarChar)).Value = this.UserName;
				}
				else
				{
					cmd.Parameters.Add(new SqlParameter("@u", SqlDbType.NVarChar)).Value = this.Email;
				}
				cn.Open();
				var reader = cmd.ExecuteReader();
				if (reader.HasRows)
				{
					reader.Dispose();
					cmd.Dispose();
					return false;
				}
				else
				{
					reader.Dispose();
					cmd.Dispose();
					return true;
				}
			}
		}		
    }

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }
}
