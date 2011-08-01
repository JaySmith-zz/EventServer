namespace EventServer.Models
{
    using System;

    public static class ValidationUtil
    {
        private const string StringRequiredErrorMessage = "Value cannot be null or empty.";

        public static void ValidateRequiredStringValue(string value, string parameterName)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException(StringRequiredErrorMessage, parameterName);
            }
        }
    }
}