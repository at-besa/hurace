using Hurace.Core.Logic;
using Hurace.Core.Logic.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace RaceControl.ViewModels
{
    public class StartListViewModel 
    {
	    public RaceModel RunningRace { get; set; } = new RaceModel();
        public ICollection<RaceModel> RaceModels { get; set; } = new Collection<RaceModel>();

        public ICollection<StartListModel> RunningRaceStartList { get; set; }
        private RaceLogic raceLogic;
        private StartListLogic startListLogic = new StartListLogic();

        public StartListViewModel()
	    {
            init();
        }

        private async void init()
        {
            raceLogic = new RaceLogic();
            RunningRace = await GetRunningRace();
            RunningRaceStartList = await GetRunningRaceStartList(RunningRace);
        }

        private async Task<ICollection<StartListModel>> GetRunningRaceStartList(RaceModel runningRace)
        {
            if (runningRace == null || runningRace.Race == null)
            {
                return null;
            }
            ICollection<StartListModel> startListModels = await startListLogic.GetStartListForRaceId(runningRace.Race.Id);
            return startListModels;
        }

        private async Task<RaceModel> GetRunningRace()
        {
            var raceModels = await raceLogic.GetRaces();
            var runningRaceModel = raceModels.
                FirstOrDefault(raceModel => raceModel.Race.Status.Name.Equals("running"));

            return runningRaceModel;
        }
    }
}
