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

        private async void DeleteStartListMember(object sender, EventArgs e)
        {
            var startListMemberToRemove = (StartListMemberModel)sender;
            if (startListMemberToRemove != null)
            {
                RunningRaceStartList.StartListMembers.Remove(startListMemberToRemove);
                RearrangeStartPositionOfStartListMembers(startListMemberToRemove.Startposition);

                PossibleSkiersNotInStartList = await GetPossibleSkiersNotInStartList(RunningRaceStartList.StartListMembers);
            }
        }

        private void RearrangeStartPositionOfStartListMembers(int startposition)
        {
            foreach (var startListMember in RunningRaceStartList.StartListMembers)
            {
                if(startposition < startListMember.Startposition)
                {
                    startListMember.Startposition--;
                }
            }
                
        }

        private async void Init()
        {
            RunningRace = await GetRunningRace();
            RunningRaceStartList = await GetRunningRaceStartList(RunningRace);
            PossibleSkiersNotInStartList = await GetPossibleSkiersNotInStartList(RunningRaceStartList.StartListMembers);

            foreach(var item in RunningRaceStartList.StartListMembers)
            {
                item.DeleteButtonCommand = new CommandBase(DeleteStartListMember);
            }
        }

        private async Task<ICollection<SkierModel>> GetPossibleSkiersNotInStartList(ICollection<StartListMemberModel> startListMembers)
        {
            var possibleSkiersNotInStartList = await startListLogic.GetAllSkiersWithSameSex(RunningRace.Sex);
            foreach (var startListMember in startListMembers)
            {
	            possibleSkiersNotInStartList.Remove(startListMember.Skier);
            }

            return possibleSkiersNotInStartList;
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
