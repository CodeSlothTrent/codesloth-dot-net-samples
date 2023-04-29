﻿using Microsoft.Extensions.Hosting;

namespace BadCodeToBeJudged
{
    public class MiniRocketLauncher : BackgroundService
    {
        public int LaunchCount { get; set; } = 0;
        public MiniRocketLauncher()
        {
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                LaunchCount++;
            }
        }
    }
}
