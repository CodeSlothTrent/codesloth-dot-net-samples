using Simplicity.Infrastructure.DTO;
using Simplicity.WebApi.DTO;

namespace Simplicity.BusinessLogic.Launch
{
    public interface IRocketLauncher
    {
        Task<RocketLaunchResult> LaunchARocket(RocketLaunchMessage rocketLaunchMessage);
    }
}