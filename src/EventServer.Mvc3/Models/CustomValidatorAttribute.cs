namespace EventServer.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class CustomValidatorAttribute : ValidationAttribute
    {
        private readonly string method;
        private readonly object typeId = new object();

        public CustomValidatorAttribute(string method, string errorMessage) : base(errorMessage)
        {
            this.method = method;
        }

        public override object TypeId
        {
           get { return this.typeId; }
        }

        public override bool IsValid(object value)
        {
            return value
                .GetType()
                .GetMethod(this.method)
                .Invoke(value, null)
                .As<bool>();
        }
    }
}