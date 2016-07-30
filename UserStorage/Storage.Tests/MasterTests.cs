using System;
using System.Linq;
using Configurator.Factory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Storage.Interfaces.Entities.UserInfo;
using Storage.Service;

namespace Storage.Tests
{
    [TestClass]
    public class MasterTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_NullFactory_ArgumentNullException()
        {
            var master = new Master(null);
        }

        [TestMethod]
        public void Add_UserAdded()
        {
            var master = new Master(TestInfo.Factory);
            int id = master.Add(TestInfo.Users[0]);
            var result = master.GetAll();
            Assert.AreEqual(1, id);
            Assert.IsTrue(TestInfo.Users[0].Equals(result.SingleOrDefault(u => u.PersonalId == id)));
        }
    }
}