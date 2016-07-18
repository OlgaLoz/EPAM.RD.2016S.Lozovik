using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Attributes.Tests
{
    [TestClass]
    public class CreatorTests
    {
        [TestMethod]
        public void CreateUser_CreateListOfUsers()
        {
            var users = new Creator().CreateUsers();
            var arrangeUsers = new []
            {
                new User(1) { FirstName = "Alexander", LastName = "Alexandrov" },
                new User(2) { FirstName = "Semen", LastName = "Semenov" },
                new User(3) { FirstName = "Petr", LastName = "Petrov" }
            };

            CollectionAssert.AreEqual(arrangeUsers, users);
        }

        [TestMethod]
        public void CreateAdvancedUser_CreateListOfAdvancedUser()
        {
            var users = new Creator().CreateAdvanceUsers();
            var arrangeUsers = new []
            {
                new AdvancedUser(4, 3443454)  { FirstName = "Pavel", LastName = "Pavlov" },
                new AdvancedUser(1, 565465) { FirstName = "Pavel", LastName = "Pavlov" },
            };

            CollectionAssert.AreEqual(arrangeUsers, users);
        }
    }
}