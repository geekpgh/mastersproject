using BrewersBuddy.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BrewersBuddy.Tests.Utilities
{
    [TestClass]
    public class CalculationsTest
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
