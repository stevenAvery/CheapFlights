using System;
using System.ComponentModel.DataAnnotations;

namespace CheapFlights.Models
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class NotEqualToAttribute : ValidationAttribute {
        private const string DefaultErrorMessage = "{0} cannot be the same as {1}.";
        public string OtherProperty { get; private set; }

        public NotEqualToAttribute(string otherProperty, string errorMessage = DefaultErrorMessage) 
            : base(errorMessage) {
            if (string.IsNullOrEmpty(otherProperty))
                throw new ArgumentNullException("otherProperty");
            OtherProperty = otherProperty;
        }

        public override string FormatErrorMessage(string name) =>
            string.Format(ErrorMessageString, name, OtherProperty);

        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            if (value != null) {
                var otherProperty = validationContext.ObjectInstance.GetType().GetProperty(OtherProperty);

                var otherPropertyValue = otherProperty.GetValue(validationContext.ObjectInstance, null);

                if (value.Equals(otherPropertyValue))
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }

            return ValidationResult.Success;
        }        
    }
}