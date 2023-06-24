using Simplicity.Database.DTO;

namespace Simplicity.BusinessLogic.Food
{
    public interface IFoodPreparation
    {
        Task<FoodForJourney> PrepareFoodForJourney(int rocketId, int numberOfSlothsToLaunch);
    }
}