using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Storage.Interfaces.Entities.UserInfo;

namespace Storage.Tests
{
    [TestClass]
    public class UserTests
    {
        [TestMethod]
        public void User_CompareTheSame_ReturnsTrue()
        {
            var firstUser = new User
            {
                FirstName = "Ann",
                LastName = "Bert"
            };
            var secondUser = new User()
            {
                FirstName = "Ann",
                LastName = "Bert"
            };

            Assert.AreEqual(true, firstUser.Equals(secondUser));
        }

        [TestMethod]
        public void User_CompareTheDifferent_ReturnsFalse()
        {
            var firstUser = new User
            {
                FirstName = "Anna",
                LastName = "Bert"
            };
            var secondUser = new User()
            {
                FirstName = "Ann",
                LastName = "Bert"
            };

            Assert.AreEqual(false, firstUser.Equals(secondUser));
        }

        [TestMethod]
        public void User_HashOfTheSame_MustBeEqual()
        {
            var firstUser = new User
            {
                FirstName = "Ann",
                LastName = "Bert",
                DateOfBirdth = DateTime.Today,
                Gender = Gender.Female
            };

            var secondUser = new User
            {
                FirstName = "Ann",
                LastName = "Bert",
                DateOfBirdth = DateTime.Today,
                Gender = Gender.Female
            };

            Assert.AreEqual(firstUser.GetHashCode(), secondUser.GetHashCode());
        }
    }
}
