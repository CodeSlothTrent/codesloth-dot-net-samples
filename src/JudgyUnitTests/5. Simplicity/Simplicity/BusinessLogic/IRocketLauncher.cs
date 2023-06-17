using Simplicity.Infrastructure;
using Simplicity.WebApi;

namespace Simplicity.BusinessLogic
{
    public interface IRocketLauncher
    {
        Task<RocketLaunchResult> LaunchARocket(RocketLaunchMessage rocketLaunchMessage);
    }
}