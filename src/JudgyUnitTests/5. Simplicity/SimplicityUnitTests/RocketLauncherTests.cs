﻿using FluentAssertions;
using Moq;
using Simplicity.BusinessLogic.Food;
using Simplicity.BusinessLogic.Launch;
using Simplicity.BusinessLogic.Navigation;
using Simplicity.Database;
using Simplicity.Database.DTO;
using Simplicity.Infrastructure.DTO;
using Simplicity.WebApi;
using Simplicity.WebApi.DTO;

namespace SimplicityUnitTests
{
    /// <summary>
    /// After simplification this class has become quite simple (boring) to unit test!
    /// </summary>
    public class RocketLauncherTests
    {
        [Fact]
        public async void LaunchARocket_throwsArgumentNullException_WhenGivenNullRocketNavigation()
        {
            var rocketLaunchingApiMock = new Mock<IRocketLaunchingApi>();
            var foodPreparationMock = new Mock<IFoodPreparation>();

            Func<RocketLauncher> act = () => new RocketLauncher(null, rocketLaunchingApiMock.Object, foodPreparationMock.Object);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task LaunchARocket_throwsArgumentNullException_WhenGivenNullRocketLaunchingApi()
        {
            var rocketNavigationMock = new Mock<IRocketNavigation>();
            var foodPreparationMock = new Mock<IFoodPreparation>();

            Func<RocketLauncher> act = () => new RocketLauncher(rocketNavigationMock.Object, null, foodPreparationMock.Object);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task LaunchARocket_throwsArgumentNullException_WhenGivenNullFoodPreparation()
        {
            var rocketNavigationMock = new Mock<IRocketNavigation>();
            var rocketLaunchingApiMock = new Mock<IRocketLaunchingApi>();

            Func<RocketLauncher> act = () => new RocketLauncher(rocketNavigationMock.Object, rocketLaunchingApiMock.Object, null);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task LaunchARocket_throwsArgumentException_WhenGivenNoMessage()
        {
            var rocketNavigationMock = new Mock<IRocketNavigation>();
            var rocketLaunchingApiMock = new Mock<IRocketLaunchingApi>();
            var foodPreparationMock = new Mock<IFoodPreparation>();
            var rocketLauncher = new RocketLauncher(rocketNavigationMock.Object, rocketLaunchingApiMock.Object, foodPreparationMock.Object);

            Func<Task<RocketLaunchResult>> act = () => rocketLauncher.LaunchARocket(null);
            await act.Should().ThrowAsync<ArgumentException>();
        }

        /// <summary>
        /// This is not a great example of a test, because we are heavily testing the insides of the function
        /// However, in lieu of having more interesting logic to assert, this will give us some confidence that accidental regression
        /// will be flagged by a failing test case
        /// </summary>
        [Fact]
        public async Task LaunchARocket_PassesCalcualtedValues_ToLaunchRequest()
        {
            var rocketModelId = 2;
            var numberOfSlothsToLaunch = 3;

            var foodPreparationMock = new Mock<IFoodPreparation>();
            var foodForJourney = new FoodForJourney(2, new[] { PretendDatabaseClient.FoodSushi, PretendDatabaseClient.FoodSlightlyToxicLeaves });

            foodPreparationMock.Setup(method => method.PrepareFoodForJourney(rocketModelId, numberOfSlothsToLaunch)).ReturnsAsync(foodForJourney);

            var rocketNavigationMock = new Mock<IRocketNavigation>();
            var thrust = 100;
            rocketNavigationMock.Setup(method => method.CalculateThrust(rocketModelId, numberOfSlothsToLaunch)).ReturnsAsync(thrust);
            (int lat, int lon) coordinates = (123, 456);
            rocketNavigationMock.Setup(method => method.CalculateCoordinatesToLand(numberOfSlothsToLaunch, foodForJourney)).Returns(coordinates);

            var rocketLaunchingApiMock = new Mock<IRocketLaunchingApi>();

            var rocketLauncher = new RocketLauncher(rocketNavigationMock.Object, rocketLaunchingApiMock.Object, foodPreparationMock.Object);
            var launchMessage = new RocketLaunchMessage(1, rocketModelId, numberOfSlothsToLaunch);

            var result = await rocketLauncher.LaunchARocket(launchMessage);
            rocketNavigationMock.VerifyAll();
            foodPreparationMock.VerifyAll();

            rocketLaunchingApiMock.Verify(method => method.LaunchRocket(
                It.Is<RocketLaunchRequest>(request =>
                    request.rocketId == rocketModelId &&
                    request.thrust == 100 &&
                    Equals(request.coordinates, coordinates) &&
                    request.numberOfSloths == numberOfSlothsToLaunch &&
                    request.FoodForJourney == foodForJourney
                )
            ));
        }
    }
}
