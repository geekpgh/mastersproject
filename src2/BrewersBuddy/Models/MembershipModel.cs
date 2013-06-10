using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrewersBuddy.Models
{
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

}