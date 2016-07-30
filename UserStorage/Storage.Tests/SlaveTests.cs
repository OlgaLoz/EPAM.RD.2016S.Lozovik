using System;
using System.Net;
using Configurator.Logging;
using FibbonacciGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Storage.Service;
using Storage.Validator;

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
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_NullRepository_ArgumentNullException()
        {
            var slave = new Slave(null);
        }
    }
}
