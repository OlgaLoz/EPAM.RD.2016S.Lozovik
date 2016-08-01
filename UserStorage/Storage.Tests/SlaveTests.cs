using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Storage.Interfaces.Entities.UserInfo;
using Storage.Service;

namespace Storage.Tests
{
    [TestClass]
    public class SlaveTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_NullFactory_ArgumentNullException()
        {
            var slave = new Slave(null);
        }
        
        [TestMethod]
        [ExpectedException(typeof(AccessViolationException))]
        public void Add_UserAdded()
        {
            var slave = new Slave(TestInfo.Factory);
            
            slave.Add(TestInfo.Users[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(AccessViolationException))]
        public void Delete_UserDeleted()
        {
            var slave = new Slave(TestInfo.Factory);

            slave.Delete(1);
        }

        [TestMethod]
        public void Search_ReturnsThree()
        {
            var slave = new Slave(TestInfo.Factory);
 
            var criteria = new Predicate<User>[] { user => user.Visas?.Length > 1 };
            var result = slave.Search(criteria).ToList();
 
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains(TestInfo.Users[2].PersonalId));
            Assert.IsFalse(result.Contains(TestInfo.Users[1].PersonalId));
            Assert.IsFalse(result.Contains(TestInfo.Users[0].PersonalId));
        }
 
        [TestMethod]
        public void Search_MultiplyCriteria_ReturnsOne()
        {
            var slave = new Slave(TestInfo.Factory);
 
            var criteria = new Predicate<User>[] { user => user.Visas?.Length > 0, user => user.Gender == Gender.Male };
            var result = slave.Search(criteria).ToList();
 
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Contains(TestInfo.Users[2].PersonalId));
            Assert.IsFalse(result.Contains(TestInfo.Users[1].PersonalId));
            Assert.IsTrue(result.Contains(TestInfo.Users[0].PersonalId));
         }
    }
}
