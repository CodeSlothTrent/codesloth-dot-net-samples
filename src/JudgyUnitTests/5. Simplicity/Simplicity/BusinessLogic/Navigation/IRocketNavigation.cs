using Simplicity.Database.DTO;

namespace Simplicity.BusinessLogic.Navigation
{
    public interface IRocketNavigation
    {
        (int latitude, int longitude) CalculateCoordinatesToLand(int numberOfSloths, FoodForJourney foodForJourney);
        Task<int> CalculateThrust(int rocketModelId, int numberOfSloths);
    }
}