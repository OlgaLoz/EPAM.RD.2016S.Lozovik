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
            var generator = new Fibbonacci();
            generator.MoveNext();
            Assert.AreEqual(1, generator.Current );
        }

        [TestMethod]
        public void Fibbonacci_SecondNumber_ReturnsTwo()
        {
            var generator = new Fibbonacci();
            generator.MoveNext();
            generator.MoveNext();
            Assert.AreEqual(2, generator.Current);
        }

        [TestMethod]
        public void Fibbonacci_ThirdNumber_ReturnsThree()
        {
            var generator = new Fibbonacci();
            generator.MoveNext();
            generator.MoveNext();
            generator.MoveNext();
            var result = generator.Current;
            Assert.AreEqual(3, result);
        }
    }
}

