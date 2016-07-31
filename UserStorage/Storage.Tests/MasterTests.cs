using System;
using System.Linq;
using System.Runtime.InteropServices;
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
            Assert.IsTrue(result.Contains(TestInfo.Users[0]));
        }

        [TestMethod]
        public void Delete_UserDeleted()
        {
            var master = new Master(TestInfo.Factory);
            master.Add(TestInfo.Users[0]);
            int id = master.Add(TestInfo.Users[1]);

            master.Delete(id);

            var result = master.GetAll();
            Assert.IsFalse(result.Contains(TestInfo.Users[1]));
            Assert.IsTrue(result.Contains(TestInfo.Users[0]));
        }

        [TestMethod]
        public void Search_MultiplyCriteria_ReturnsOne()
        {
            var master = new Master(TestInfo.Factory);
            int id1 = master.Add(TestInfo.Users[0]);
            int id2 = master.Add(TestInfo.Users[1]);
            int id3 = master.Add(TestInfo.Users[2]);

            var result = master.Search(new MaleCriteria()).ToList();

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Contains(id3));
            Assert.IsFalse(result.Contains(id2));
            Assert.IsTrue(result.Contains(id1));
        }
    }
}