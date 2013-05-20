using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BrewersBuddy.Models;

namespace BrewersBuddy.Tests.Calculators
{
    [TestClass]
    public class TestCalculations
    {
        [TestMethod]
        public void TestABV()
        {
            double originalGravity = 1.05;
            double finalGravity = 1.01;

            double actualResult = Calculations.calculateABV(originalGravity, finalGravity);
            Assert.AreEqual(.0533, actualResult, .001);
        }

        [TestMethod]
        public void TestABVPercentage()
        {
            double originalGravity = 1.05;
            double finalGravity = 1.01;

            double actualResult = Calculations.calculateABVPercentage(originalGravity, finalGravity);
            Assert.AreEqual(5.33, actualResult, .01);
        }
    }
}
