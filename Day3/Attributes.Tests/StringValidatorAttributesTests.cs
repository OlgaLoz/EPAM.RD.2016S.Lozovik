using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Attributes.Tests
{
    [TestClass]
    public class StringValidatorAttributesTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void StringValidatorAttribute_NullValue_ReturnsArgumentNullException()
        {
            var validator = new StringValidatorAttribute(5);
            validator.IsValid(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void StringValidatorAttribute_NotStringValue_ReturnsArgumentException()
        {
            var validator = new StringValidatorAttribute(5);
            validator.IsValid(5);
        }

        [TestMethod]
        public void StringValidatorAttribute_CorrectValue_ReturnsTrue()
        {
            var validator = new StringValidatorAttribute(5);
            Assert.AreEqual(validator.IsValid("12345"), true);
        }

        [TestMethod]
        public void StringValidatorAttribute_IncorrectValue_ReturnsFalse()
        {
            var validator = new StringValidatorAttribute(5);
            Assert.AreEqual(validator.IsValid("123456"), false);
        }
    }
}
