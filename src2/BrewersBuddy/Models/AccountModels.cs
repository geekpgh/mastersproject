﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace BrewersBuddy.Models
{
    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        public string Email { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string City { get; set; }

        [MinLength(2)]
        [MaxLengthAttribute(2)]
        [StringLength(100, ErrorMessage = "The {0} must be {2} characters long.", MinimumLength = 2)]
        [Display(Name = "State")]
        [RegularExpression(@"[A-Z]{2}", ErrorMessage = "State must be the two character capital abbreviation")]
        public string State { get; set; }

        [MinLength(5)]
        [MaxLengthAttribute(5)]
        [StringLength(100, ErrorMessage = "The {0} must be {2} characters long.", MinimumLength = 5)]
        [Display(Name = "Zip")]
        [RegularExpression(@"^\d{5}(?:[-\s]\d{4})?$", ErrorMessage = "The {0} must be in the format ##### or #####-####")]
        public string Zip { get; set; }

        public virtual ICollection<BatchRating> BatchRatings { get; set; }
        public virtual ICollection<BatchComment> BatchComments { get; set; }
        public virtual ICollection<Friend> Friends { get; set; }
        public virtual ICollection<Batch> CollaboratorBatches { get; set; }
        

        public UserProfile()
        {
            this.Friends = new List<Friend>();
        }

        public virtual string Name
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
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
        [StringLength(100, ErrorMessage = "The {0} must be {2} characters long.", MinimumLength = 2)]
        [Display(Name = "State")]
        [RegularExpression(@"[A-Z]{2}", ErrorMessage = "State must be the two character abbreviation in captial letters. Example: PA")]
        public string State { get; set; }

        [MinLength(5)]
        [MaxLengthAttribute(5)]
        [StringLength(100, ErrorMessage = "The {0} must be {2} characters long.", MinimumLength = 5)]
        [Display(Name = "Zip")]
        [RegularExpression(@"^\d{5}(?:[-\s]\d{4})?$", ErrorMessage = "The {0} must be in the format ##### or #####-####")]
        public string Zip { get; set; }

    }

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }
}
