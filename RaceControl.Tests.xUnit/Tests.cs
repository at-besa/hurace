using System;

using RaceControl.ViewModels;

using Xunit;

namespace RaceControl.Tests.XUnit
{
    // TODO WTS: Add appropriate tests
    public class Tests
    {
        [Fact]
        public void TestMethod1()
        {
        }

        // TODO WTS: Add tests for functionality you add to RaceControlViewModel.
        [Fact]
        public void TestRaceControlViewModelCreation()
        {
            // This test is trivial. Add your own tests for the logic you add to the ViewModel.
            var vm = new RaceControlViewModel();
            Assert.NotNull(vm);
        }

        // TODO WTS: Add tests for functionality you add to RaceViewModel.
        [Fact]
        public void TestRaceViewModelCreation()
        {
            // This test is trivial. Add your own tests for the logic you add to the ViewModel.
            var vm = new RaceViewModel();
            Assert.NotNull(vm);
        }

        // TODO WTS: Add tests for functionality you add to ScreenControlViewModel.
        [Fact]
        public void TestScreenControlViewModelCreation()
        {
            // This test is trivial. Add your own tests for the logic you add to the ViewModel.
            var vm = new ScreenControlViewModel();
            Assert.NotNull(vm);
        }

        // TODO WTS: Add tests for functionality you add to StartListViewModel.
        [Fact]
        public void TestStartListViewModelCreation()
        {
            // This test is trivial. Add your own tests for the logic you add to the ViewModel.
            var vm = new StartListViewModel();
            Assert.NotNull(vm);
        }
    }
}
