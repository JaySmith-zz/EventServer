using System;

namespace EventServer.Core
{
    public static class ValidationUtil
    {
        private const string _stringRequiredErrorMessage = "Value cannot be null or empty.";

        public static void ValidateRequiredStringValue(string value, string parameterName)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException(_stringRequiredErrorMessage, parameterName);
        }
    }
}