using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FibbonacciGenerator.Tests
{
    [TestClass]
    public class FibbonacciGeneratorTests
    {
        [TestMethod]
        public void Fibbonacci_FirstNumber_ReturnsOne()
        {
            var generator = new FibonacciGenerator();
                      
            Assert.AreEqual(1, generator.GetNextId());
        }

        [TestMethod]
        public void Fibbonacci_SecondNumber_ReturnsTwo()
        {
            var generator = new FibonacciGenerator();
            generator.GetNextId();

            Assert.AreEqual(2, generator.GetNextId());
        }

        [TestMethod]
        public void Fibbonacci_ThirdNumber_ReturnsThree()
        {
            var generator = new FibonacciGenerator();
            generator.GetNextId();
            generator.GetNextId();

            Assert.AreEqual(3, generator.GetNextId());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Fibbonacci_MoveNextOnIntegerOverflow_ReturnsFalse()
        {
            var generator = new FibonacciGenerator();

            for (int i = 0; i < 100; i++)
            {
                generator.GetNextId();
            }
        }

        [TestMethod]
        public void Fibbonacci_LoadCurrentState()
        {
            var generator = new FibonacciGenerator();
            generator.LoadState(8);

            Assert.AreEqual(13, generator.GetNextId());
        }

    }
}

