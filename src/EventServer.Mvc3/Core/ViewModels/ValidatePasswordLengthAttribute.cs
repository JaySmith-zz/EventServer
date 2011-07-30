using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Security;

namespace EventServer.Core.ViewModels
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ValidatePasswordLengthAttribute : ValidationAttribute
    {
        public ValidatePasswordLengthAttribute() : base(_defaultErrorMessage)
        {
        }

        private const string _defaultErrorMessage = "The {0} field must be at least {1} characters long.";
        private readonly int _minCharacters = Membership.Provider.MinRequiredPasswordLength;

        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentUICulture, ErrorMessageString, name, _minCharacters);
        }

        public override bool IsValid(object value)
        {
            string valueAsString = value as string;
            return (valueAsString != null && valueAsString.Length >= _minCharacters);
        }
    }
}