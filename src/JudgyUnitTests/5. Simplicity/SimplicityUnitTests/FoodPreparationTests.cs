using FluentAssertions;
using Moq;
using Simplicity.BusinessLogic.Food;
using Simplicity.Database;
using Simplicity.Database.DTO;

namespace SimplicityUnitTests
{
    public class FoodPreparationTests
    {
        [Fact]
        public async Task PrepareFoodForJourney_ReturnsFoodFromDatabase_WhenAvailable()
        {
            var foodForJourney = new FoodForJourney(1, new[] { PretendDatabaseClient.FoodSushi });
            var databaseRetrieverMock = new Mock<IRocketDatabaseRetriever>();
            databaseRetrieverMock.Setup(method => method.GetFoodToFeedSlothsOnTheirJourney(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(foodForJourney);

            var foodPreparation = new FoodPreparation(databaseRetrieverMock.Object);
            var food = await foodPreparation.PrepareFoodForJourney(1, 1);
            food.Should().BeEquivalentTo(foodForJourney);
        }

        /// <summary>
        /// The following tests are only available due to refactoring the database retriever to return null when food could not be found
        /// Instead of the ambiguous new object
        /// </summary>
        [Theory]
        [InlineData(9)]
        [InlineData(5)]
        [InlineData(1)]
        public async Task PrepareFoodForJourney_ReturnsDefaultMenu_WhenDatabaseFoodUnavailable_AndLessThan10Sloths(int numberOfSloths)
        {
            var databaseRetrieverMock = new Mock<IRocketDatabaseRetriever>();
            databaseRetrieverMock.Setup(method => method.GetFoodToFeedSlothsOnTheirJourney(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((FoodForJourney?)null);

            var foodPreparation = new FoodPreparation(databaseRetrieverMock.Object);
            var food = await foodPreparation.PrepareFoodForJourney(1, numberOfSloths);

            var foodForJourney = new FoodForJourney(1, new[] { PretendDatabaseClient.FoodSlightlyToxicLeaves });

            food.Should().BeEquivalentTo(foodForJourney);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(11)]
        [InlineData(100)]
        public async Task PrepareFoodForJourney_ThrowsException_WhenDatabaseFoodUnavailable_AndMoreThan9Sloths(int numberOfSloths)
        {
            var databaseRetrieverMock = new Mock<IRocketDatabaseRetriever>();
            databaseRetrieverMock.Setup(method => method.GetFoodToFeedSlothsOnTheirJourney(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((FoodForJourney?)null);

            var foodPreparation = new FoodPreparation(databaseRetrieverMock.Object);
            Func<Task<FoodForJourney>> act = () => foodPreparation.PrepareFoodForJourney(1, numberOfSloths);
            await act.Should().ThrowAsync<Exception>();
        }
    }
}
