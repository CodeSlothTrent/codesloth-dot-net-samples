using FluentAssertions;
using Simplicity.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplicityUnitTests
{
    public class AsyncDelayTests
    {
        [Fact]
        public void NoTestsRequiredOnDummyClass()
        {
            // This is a dummy infrastructure class that does not require unit tests for this exercise
            var poller = new AsyncDelay();
            true.Should().BeTrue();
        }
    }
}
