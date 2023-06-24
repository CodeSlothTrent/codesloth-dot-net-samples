using Simplicity.Database;

namespace Simplicity.BusinessLogic.Food
{
    public class FoodPreparation : IFoodPreparation
    {
        private IRocketDatabaseRetriever rocketDatabaseRetriever;

        public FoodPreparation(IRocketDatabaseRetriever rocketDatabaseRetriever)
        {
            this.rocketDatabaseRetriever = rocketDatabaseRetriever;
        }

        public async Task<FoodForJourney> PrepareFoodForJourney(int rocketId, int numberOfSlothsToLaunch)
        {
            var foodForJourney = await rocketDatabaseRetriever.GetFoodToFeedSlothsOnTheirJourney(rocketId, numberOfSlothsToLaunch);
            if (foodForJourney == null)
            {
                if (numberOfSlothsToLaunch < 10)
                {
                    foodForJourney = new FoodForJourney { NumberOfCourses = 1, Foods = new[] { PretendDatabaseClient.FoodSlightlyToxicLeaves } };
                }
                else
                {
                    throw new Exception("We can't cater a trip for 10 or more sloths without carefully planned food for space travel!");
                }
            }

            return foodForJourney;
        }
    }
}