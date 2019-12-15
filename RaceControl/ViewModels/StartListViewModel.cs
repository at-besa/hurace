using Hurace.Core.Logic;
using Hurace.Core.Logic.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace RaceControl.ViewModels
{
    public class StartListViewModel : NotifyPropertyChanged
    {
	    public RaceModel RunningRace { get; set; } = new RaceModel();
        public ICollection<SkierModel> PossibleSkiersNotInStartList { get; set; } = new ObservableCollection<SkierModel>();

        private StartListModel runningRaceStartList;
        public StartListModel RunningRaceStartList
        {
            get => runningRaceStartList;
            set => Set(ref runningRaceStartList, value);
        }

        public StartListMemberModel SelectedStartListMember { get; set; }
        public SkierModel SelectedPossibleSkierNotInStartList { get; set; }

        private RaceLogic raceLogic;
        private StartListLogic startListLogic;


        public StartListViewModel()
	    {
            init();
        }

        private async void init()
        {
            raceLogic = new RaceLogic();
            startListLogic = new StartListLogic();
            RunningRace = await GetRunningRace();
            RunningRaceStartList = await GetRunningRaceStartList(RunningRace);
            PossibleSkiersNotInStartList = await GetPossibleSkiersNotInStartList(RunningRace.Race.Sex, RunningRaceStartList.StartListMembers);
        }

        private async Task<ICollection<SkierModel>> GetPossibleSkiersNotInStartList(string sex, ICollection<StartListMemberModel> startListMembers)
        {
            var skiersWithSameSex = await startListLogic.GetAllSkiersWithSameSex(sex);
            foreach (var startListMember in startListMembers)
            {
                skiersWithSameSex.Remove(startListMember.Skier);
            }
            return skiersWithSameSex;
        }

        private async Task<StartListModel> GetRunningRaceStartList(RaceModel runningRace)
        {
            if (runningRace == null || runningRace.Race == null)
            {
                return null;
            }
            StartListModel startListModel = await startListLogic.GetStartListForRaceId(runningRace.Race.Id);
            return startListModel;
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
