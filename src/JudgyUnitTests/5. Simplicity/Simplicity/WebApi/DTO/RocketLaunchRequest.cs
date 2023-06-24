using Simplicity.Database.DTO;

namespace Simplicity.WebApi.DTO
{
    public record RocketLaunchRequest(
        int rocketId,
        int numberOfSloths,
        int thrust,
        FoodForJourney FoodForJourney,
        (int lat, int lon) coordinates
    );
}