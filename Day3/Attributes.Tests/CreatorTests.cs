using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Attributes.Tests
{
    [TestClass]
    public class CreatorTests
    {
        [TestMethod]
        public void Sfsg()
        {
           var users = new Creator().CreateAdvanceUsers();
            int t = 5;
            Assert.AreEqual(4, t);
            //users.
        }
    }
}