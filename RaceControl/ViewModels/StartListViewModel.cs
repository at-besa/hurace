using Hurace.Core.Logic;
using Hurace.Core.Logic.Model;
using Swack.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.DAL.Domain;
using Swack.UI.ViewModels;

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

        private RaceManagementLogic raceManagementLogic;
        private StartListLogic startListLogic;


        public StartListViewModel()
	    {
            Init();
        }

        private async void Init()
        {
            raceManagementLogic = new RaceManagementLogic();
            startListLogic = new StartListLogic();
            RunningRace = await GetRunningRace();
            RunningRaceStartList = await GetRunningRaceStartList(RunningRace);
            PossibleSkiersNotInStartList = await GetPossibleSkiersNotInStartList(RunningRace.Sex, RunningRaceStartList.StartListMembers);
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
            if (runningRace == null || runningRace == null)
            {
                return null;
            }
            StartListModel startListModel = await startListLogic.GetStartListForRaceId(runningRace.Id);
            return startListModel;
        }

        private async Task<RaceModel> GetRunningRace()
        {
            var raceModels = await raceManagementLogic.GetRaces();
            var runningRaceModel = raceModels.
                FirstOrDefault(raceModel => raceModel.Status.Name.Equals("running"));

            return runningRaceModel;
        }
    }
}
