using Simplicity.Database;

namespace Simplicity.WebApi
{
    public record RocketLaunchRequest(
        int rocketId, 
        int numberOfSloths, 
        int thrust, 
        FoodForJourney FoodForJourney, 
        (int lat, int lon) coordinates
    );
}