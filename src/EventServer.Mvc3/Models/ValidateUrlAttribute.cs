namespace EventServer.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ValidateUrlAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            var possibleUrl = value.ToString();
            return string.IsNullOrEmpty(possibleUrl) || Uri.IsWellFormedUriString(possibleUrl, UriKind.Absolute);
        }
    }
}