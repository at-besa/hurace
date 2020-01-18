using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Logic;
using Hurace.Core.Logic.Interface;
using Hurace.Core.Logic.Model;
using RaceControl.Helpers;
using Swack.UI.ViewModels;
using NotifyPropertyChanged = RaceControl.Helpers.NotifyPropertyChanged;

namespace RaceControl.ViewModels
{
    public class StartListViewModel : NotifyPropertyChanged
    {
        private readonly IRaceManagementLogic raceManagementLogic = RaceManagementLogic.Instance;
        private readonly IStartListLogic startListLogic = StartListLogic.Instance;
	    public RaceModel RunningRace { get; set; } = new RaceModel();

	    private ICollection<SkierModel> _possibleSkiersNotInStartList;
        public ICollection<SkierModel> PossibleSkiersNotInStartList
	    {
		    get => _possibleSkiersNotInStartList;
		    set => Set(ref _possibleSkiersNotInStartList ,value);
	    }

	    private StartListModel _runningRaceStartList;
        public StartListModel RunningRaceStartList
        {
            get => _runningRaceStartList;
            set => Set(ref _runningRaceStartList, value);
        }

        public StartListMemberModel SelectedStartListMember { get; set; }
        public SkierModel SelectedPossibleSkierNotInStartList { get; set; }

        public CommandBase DeleteStartListMemberCommand { get; set; }
        public CommandBase AddStartListMemberCommand { get; set; }



        public StartListViewModel()
	    {
            Init();
            DeleteStartListMemberCommand = new CommandBase(DeleteStartListMember);
            AddStartListMemberCommand = new CommandBase(AddStartListMember);
        }

        private void AddStartListMember(object sender, EventArgs e)
        {
            var skierToAdd = (SkierModel)sender;
            if(skierToAdd != null)
            {
                PossibleSkiersNotInStartList.Remove(skierToAdd);
                //startListLogic.
                //var sizeOfStartList = RunningRaceStartList.StartListMembers.Count();
                //RunningRaceStartList.StartListMembers.Add(new StartListMemberModel
                //{
                //    Skier = skierToAdd,
                //    Blocked = true,
                //    Finished = false,
                //    Running = false,
                //    Disqualified = false,
                //    Startposition = sizeOfStartList++,
                //    DeleteButtonCommand = new CommandBase(DeleteStartListMember)
                //});
            }
        }

        private async void DeleteStartListMember(object sender, EventArgs e)
        {
            var startListMemberToRemove = (StartListMemberModel)sender;
            if (startListMemberToRemove != null)
            {
                var raceId = RunningRace.Id;
                var skierId = startListMemberToRemove.Skier.Id;
                var startposition = startListMemberToRemove.Startposition;
                var runNo = startListMemberToRemove.RunNo;
                await startListLogic.DeleteStartListMember(skierId, raceId, runNo ,startposition);
                if(!await startListLogic.IsStartListMemberInStartList(raceId , skierId, runNo))
                {
                    await RearrangeStartPositionOfStartListMembers(startposition);
                }
                RunningRaceStartList = await GetRunningRaceStartList(RunningRace);
                PossibleSkiersNotInStartList = await GetPossibleSkiersNotInStartList(RunningRaceStartList.StartListMembers);
            }
        }

        private async Task RearrangeStartPositionOfStartListMembers(int startposition)
        {
            foreach (var startListMember in RunningRaceStartList.StartListMembers)
            {
                if(startposition < startListMember.Startposition)
                {
                    var updateded = await startListLogic.UpdateStartListMemberStartPosition(RunningRace.Id, startListMember.Skier.Id, startListMember.RunNo,startListMember.Startposition - 1);
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
