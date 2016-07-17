using System;
using System.ComponentModel.DataAnnotations;

namespace Attributes
{
    // Should be applied to properties and fields.
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class StringValidatorAttribute : ValidationAttribute
    {
        private readonly int maxLength;

        public StringValidatorAttribute(int maxLength)
        {
            this.maxLength = maxLength;
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            string val =  value as string;

            if (val == null)
            {
                throw new ArgumentException($"{nameof(value)} must be string");
            }

            return val.Length <= maxLength;
        }
    }
}
