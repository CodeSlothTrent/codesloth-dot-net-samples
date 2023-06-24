using Simplicity.BusinessLogic.Food;
using Simplicity.BusinessLogic.Navigation;
using Simplicity.Infrastructure;
using Simplicity.WebApi;

namespace Simplicity.BusinessLogic.Launch
{
    public class RocketLauncher : IRocketLauncher
    {
        private IRocketNavigation rocketNavigation;
        private IRocketLaunchingApi rocketLaunchingApi;
        private IFoodPreparation foodPreparation;

        public RocketLauncher(IRocketNavigation rocketNavigation, IRocketLaunchingApi rocketLaunchingApi, IFoodPreparation foodPreparation)
        {
            this.rocketNavigation = rocketNavigation ?? throw new ArgumentNullException(nameof(rocketNavigation));
            this.rocketLaunchingApi = rocketLaunchingApi ?? throw new ArgumentNullException(nameof(rocketLaunchingApi));
            this.foodPreparation = foodPreparation ?? throw new ArgumentNullException(nameof(foodPreparation));
        }

        public async Task<RocketLaunchResult> LaunchARocket(RocketLaunchMessage rocketLaunchMessage)
        {
            if(rocketLaunchMessage == null)
            {
                throw new ArgumentNullException(nameof(rocketLaunchMessage));
            }

            var requiredThrust = await rocketNavigation.CalculateThrust(rocketLaunchMessage.RocketModelId, rocketLaunchMessage.numberOfSlothsToLaunch);
            var foodForJourney = await foodPreparation.PrepareFoodForJourney(rocketLaunchMessage.RocketModelId, rocketLaunchMessage.numberOfSlothsToLaunch);
            var coordinatesToLandOnMoon = rocketNavigation.CalculateCoordinatesToLand(rocketLaunchMessage.numberOfSlothsToLaunch, foodForJourney);

            var rocketLaunchRequest = new RocketLaunchRequest(
                rocketLaunchMessage.RocketModelId,
                rocketLaunchMessage.numberOfSlothsToLaunch,
                requiredThrust,
                foodForJourney,
                coordinatesToLandOnMoon
            );

            var rocketLaunchResult = await rocketLaunchingApi.LaunchRocket(rocketLaunchRequest);
            return rocketLaunchResult;
        }


    }
}
