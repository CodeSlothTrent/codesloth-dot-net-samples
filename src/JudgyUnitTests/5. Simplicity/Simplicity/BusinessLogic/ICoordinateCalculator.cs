using Simplicity.Database;

namespace Simplicity.BusinessLogic
{
    public interface ICoordinateCalculator
    {
        (int latitude, int longitude) CalculateCoordinatesToLand(int numberOfSloths, FoodForJourney foodForJourney);
    }
}