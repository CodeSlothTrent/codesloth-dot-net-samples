namespace FixingConcreteDependencies.Infrastructure
{
    /// <summary>
    /// A message that signals a rocket is ready to be launched by a Code Sloth user
    /// </summary>
    public record RocketLaunchMessage(int messageId, int RocketModelId, int numberOfSlothsToLaunch);
}
