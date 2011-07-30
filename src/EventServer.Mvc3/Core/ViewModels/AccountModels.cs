using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using EventServer.Core.Domain;

namespace EventServer.Core.ViewModels
{
    public class AccountShowModel
    {
        public UserProfile User { get; set; }
    }

    public class AccountLogOnModel
    {
        public string ReturnUrl { get; set; }

        [Required]
        [DisplayName("Email address")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }

        [DisplayName("Remember me?")]
        public bool RememberMe { get; set; }
    }

    [PropertiesMustMatch("Password", "ConfirmPassword", ErrorMessage = "The password and confirmation password do not match.")]
    public class AccountRegisterModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Invalid Email Address.")]
        [DisplayName("Email address")]
        public string Email { get; set; }

        [Required]
        [ValidatePasswordLength]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Confirm password")]
        public string ConfirmPassword { get; set; }
    }

    [PropertiesMustMatch("NewPassword", "ConfirmPassword", ErrorMessage = "The new password and confirmation password do not match.")]
    public class AccountChangePasswordModel
    {
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("Current password")]
        public string OldPassword { get; set; }

        [Required]
        [ValidatePasswordLength]
        [DataType(DataType.Password)]
        [DisplayName("New password")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Confirm new password")]
        public string ConfirmPassword { get; set; }
    }

    public class AccountChangeNameModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }

    public class AccountChangeEmailModel
    {
        public int Id { get; set; }

        [DisplayName("Current email")]
        public string CurrentEmail { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Invalid or taken email address.")]
        [DisplayName("New email")]
        public string NewEmail { get; set; }
    }
}