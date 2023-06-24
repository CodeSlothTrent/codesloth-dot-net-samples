using Simplicity.Infrastructure;
using Simplicity.WebApi;

namespace Simplicity.BusinessLogic.Launch
{
    public interface IRocketLauncher
    {
        Task<RocketLaunchResult> LaunchARocket(RocketLaunchMessage rocketLaunchMessage);
    }
}