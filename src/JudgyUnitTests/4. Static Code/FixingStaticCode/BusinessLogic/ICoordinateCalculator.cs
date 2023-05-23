using FixingStaticCode.Database;

namespace FixingStaticCode.BusinessLogic
{
    public interface ICoordinateCalculator
    {
        (int latitude, int longitude) CalculateCoordinatesToLand(int numberOfSloths, FoodForJourney foodForJourney);
    }
}