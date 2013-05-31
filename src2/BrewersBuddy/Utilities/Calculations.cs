
namespace BrewersBuddy.Utilities
{
    public class Calculations
    {
        public static double calculateABV(double originalGravity, double finalGravity)
        {
            return (originalGravity - finalGravity) / .75;
        }

        public static double calculateABVPercentage(double originalGravity, double finalGravity)
        {
            return calculateABV(originalGravity, finalGravity) * 100;
        }

    }
}