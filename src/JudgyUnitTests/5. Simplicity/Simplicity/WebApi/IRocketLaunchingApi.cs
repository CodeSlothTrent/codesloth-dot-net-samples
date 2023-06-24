namespace Simplicity.WebApi
{
    public partial interface IRocketLaunchingApi
    {

        Task<RocketLaunchResult> LaunchRocket(RocketLaunchRequest request);
    }
}