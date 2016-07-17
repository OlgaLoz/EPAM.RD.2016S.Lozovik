using System;
using System.ComponentModel.DataAnnotations;

namespace Attributes
{
    // Should be applied to properties and fields.
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class IntValidatorAttribute : ValidationAttribute
    {
        private readonly int min;
        private readonly int max;
        
        public IntValidatorAttribute(int min, int max)
        {
            this.min = min;
            this.max = max;
        }

        public override bool IsValid(object value)
        {
            if (!(value is int))
            {
                throw new ArgumentException($"{nameof(value)} must be int");
            }

            int id = (int)value;

            return (id >= min && id <= max);
        }
    }
}
