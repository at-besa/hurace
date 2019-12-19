using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Logic;
using Hurace.Core.Logic.Interface;
using Hurace.Core.Logic.Model;
using RaceControl.Helpers;
using Swack.UI.ViewModels;

namespace RaceControl.ViewModels
{
    public class StartListViewModel : NotifyPropertyChanged
    {
        private readonly IRaceManagementLogic raceManagementLogic = RaceManagementLogic.Instance;
        private readonly IStartListLogic startListLogic = StartListLogic.Instance;
	    public RaceModel RunningRace { get; set; } = new RaceModel();

	    private ICollection<SkierModel> possibleSkiersNotInStartList;
        public ICollection<SkierModel> PossibleSkiersNotInStartList
	    {
		    get => possibleSkiersNotInStartList;
		    set => Set(ref possibleSkiersNotInStartList ,value);
	    }

	    private StartListModel runningRaceStartList;
        public StartListModel RunningRaceStartList
        {
            get => runningRaceStartList;
            set => Set(ref runningRaceStartList, value);
        }

        public StartListMemberModel SelectedStartListMember { get; set; }
        public SkierModel SelectedPossibleSkierNotInStartList { get; set; }

        public CommandBase DeleteStartListMemberCommand { get; set; }



        public StartListViewModel()
	    {
            Init();
            DeleteStartListMemberCommand = new CommandBase(DeleteStartListMember);
        }

        private void DeleteStartListMember(object sender, EventArgs e)
        {
            String senderType = sender.GetType().ToString();
            
            throw new NotImplementedException();
        }

        private async void Init()
        {
            RunningRace = await GetRunningRace();
            RunningRaceStartList = await GetRunningRaceStartList(RunningRace);
            PossibleSkiersNotInStartList = await startListLogic.GetAllSkiersWithSameSex(RunningRace.Sex);
            GetPossibleSkiersNotInStartList(RunningRaceStartList.StartListMembers);

            foreach(var item in RunningRaceStartList.StartListMembers)
            {
                item.DeleteButtonCommand = new CommandBase(DeleteStartListMember);
            }
        }

        private void GetPossibleSkiersNotInStartList(ICollection<StartListMemberModel> startListMembers)
        {
	        foreach (var startListMember in startListMembers)
            {
	            PossibleSkiersNotInStartList.Remove(startListMember.Skier);
            }
        }

        private async Task<StartListModel> GetRunningRaceStartList(RaceModel runningRace)
        {
            if (runningRace == null)
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
