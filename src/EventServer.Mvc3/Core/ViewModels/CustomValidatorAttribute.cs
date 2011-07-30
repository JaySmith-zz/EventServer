using System;
using System.ComponentModel.DataAnnotations;

namespace EventServer.Core.ViewModels
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class CustomValidatorAttribute : ValidationAttribute
    {
        public CustomValidatorAttribute(string method, string errorMessage) : base(errorMessage)
        {
            _method = method;
        }

        private readonly string _method;
        private readonly object _typeId = new object();

        public override object TypeId
        {
           get { return _typeId; }
        }

        public override bool IsValid(object value)
        {
            return value
                .GetType()
                .GetMethod(_method)
                .Invoke(value, null)
                .As<bool>();
        }
    }
}