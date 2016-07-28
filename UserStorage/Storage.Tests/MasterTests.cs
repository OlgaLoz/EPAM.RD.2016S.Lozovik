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
    public class MasterTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_NullValidator_ArgumentNullException()
        {
            var master = new Master(null, new Repository.Repository(), new FibonacciGenerator(), new IPEndPoint[] { }, new DefaultLogger());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_NullRepository_ArgumentNullException()
        {
            var master = new Master(new UserValidator(), null, new FibonacciGenerator(), new IPEndPoint[] { }, new DefaultLogger());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_NullGenerator_ArgumentNullException()
        {
            var master = new Master(new UserValidator(), new Repository.Repository(), null, new IPEndPoint[] { }, new DefaultLogger());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_NullConnectionInfo_ArgumentNullException()
        {
            var master = new Master(new UserValidator(), new Repository.Repository(), new FibonacciGenerator(), null, new DefaultLogger());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_NullLogger_ArgumentNullException()
        {
            var master = new Master(new UserValidator(), new Repository.Repository(), new FibonacciGenerator(), new IPEndPoint[] { }, null);
        }
    }
}