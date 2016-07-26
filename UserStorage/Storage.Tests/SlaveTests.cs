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
        public void Ctor_NullConnectionInfo_ArgumentNullException()
        {
            var slave = new Slave(null, new Repository.Repository(), 
                 new DefaultLogger());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_NullRepository_ArgumentNullException()
        {
            var slave = new Slave(new IPEndPoint(0, 0), null,
                 new DefaultLogger());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_NullLogger_ArgumentNullException()
        {
            var slave = new Slave(new IPEndPoint(0, 0), new Repository.Repository(),
                 null);
        }
    }
    }
