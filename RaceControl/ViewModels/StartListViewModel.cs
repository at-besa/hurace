using Hurace.Core.Logic;
using Hurace.Core.Logic.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace RaceControl.ViewModels
{
    public class StartListViewModel 
    {
	    public RaceModel RunningRace { get; set; }
        private RaceLogic raceLogic;

	    public StartListViewModel()
	    {
            raceLogic = new RaceLogic();
            GetRunningRace();
        }

        private void GetStartListToRace()
        {
            throw new NotImplementedException();
        }

        private void GetSkiersToRace()
        {
            throw new NotImplementedException();
        }

        private async void GetRunningRace()
        {
            var raceModels = await raceLogic.GetRaces();
            var runningRaceModel = raceModels.
                Where(raceModel => raceModel.Race.Status.Name.Equals("running"))
                .FirstOrDefault();

            if (runningRaceModel == null)
            {
                return;
            }

            RunningRace = new RaceModel(); 
            RunningRace = runningRaceModel;
        }
	}
}
