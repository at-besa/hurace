using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Logic;
using Hurace.Core.Logic.Interface;
using Hurace.Core.Logic.Model;
using RaceControl.Helpers;
using SQLitePCL;
using Swack.UI.ViewModels;
using NotifyPropertyChanged = RaceControl.Helpers.NotifyPropertyChanged;

namespace RaceControl.ViewModels
{
    public class StartListViewModel : NotifyPropertyChanged
    {
        private readonly IRaceManagementLogic _raceManagementLogic = RaceManagementLogic.Instance;
        private readonly IStartListLogic _startListLogic = StartListLogic.Instance;
        private readonly int _runNo = 1;
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
        }

        private async void AddStartListMember(object sender, EventArgs e)
        {
            var skierToAdd = (SkierModel)sender;
            if(skierToAdd != null)
            {
                var raceId = RunningRace.Id;
                var startPosition = RunningRaceStartList.StartListMembers.Count() + 1;
                var inserted = await _startListLogic.InsertStartListMember(raceId, skierToAdd.Id, _runNo, startPosition);
                if (inserted)
                {
                    Init();
                }
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
                await _startListLogic.DeleteStartListMember(raceId, skierId, runNo);
                if(!await _startListLogic.IsStartListMemberInStartList(raceId , skierId, runNo))
                {
                    await RearrangeStartPositionOfStartListMembers(startposition);
                }
                Init();
            }
        }

        private async Task RearrangeStartPositionOfStartListMembers(int startposition)
        {
            foreach (var startListMember in RunningRaceStartList.StartListMembers)
            {
                if(startposition < startListMember.Startposition)
                {
                    var updateded = await _startListLogic.UpdateStartListMemberStartPosition(RunningRace.Id, startListMember.Skier.Id, startListMember.RunNo,startListMember.Startposition - 1);
                }
            }       
        }

        private async void Init()
        {
            RunningRace = await GetRunningRace();
            RunningRaceStartList = await GetRunningRaceStartList(RunningRace);
            PossibleSkiersNotInStartList = await GetPossibleSkiersNotInStartList(RunningRaceStartList.StartListMembers);

            foreach(var startListMember in RunningRaceStartList.StartListMembers)
            {
                startListMember.DeleteButtonCommand = new CommandBase(DeleteStartListMember);
                startListMember.UpButtonCommand = new CommandBase(MoveUpStartListMember);
                startListMember.DownButtonCommand = new CommandBase(MoveDownStartListMember);
            }

            foreach (var possibleSkierNotInStartList in PossibleSkiersNotInStartList)
            {
                possibleSkierNotInStartList.AddButtonCommand = new CommandBase(AddStartListMember);
            }
        }

        private async void MoveDownStartListMember(object? sender, EventArgs e)
        {
            if (sender == null)
            {
                return;
            }
            var startListMemberToMoveDown = (StartListMemberModel) sender;
            if (startListMemberToMoveDown.Startposition == RunningRaceStartList.StartListMembers.Count)
            {
                return;
            }

            int oldStartPosition = startListMemberToMoveDown.Startposition;
            var startListMemberToMoveUp = RunningRaceStartList.StartListMembers.FirstOrDefault(startListMember => startListMember.Startposition == (oldStartPosition + 1));
            if (startListMemberToMoveUp == null)
            {
                return;
            }

            int newStartPosition = startListMemberToMoveUp.Startposition;
            bool movedToOldStartPosition =  await _startListLogic.UpdateStartListMemberStartPosition(RunningRace.Id, startListMemberToMoveUp.Skier.Id,
                startListMemberToMoveUp.RunNo, oldStartPosition);
            if (movedToOldStartPosition)
            {
                bool movedToNewStartPosition = await _startListLogic.UpdateStartListMemberStartPosition(RunningRace.Id, startListMemberToMoveDown.Skier.Id,
                    startListMemberToMoveDown.RunNo, newStartPosition);   
            }
            Init();
        }

        private async void MoveUpStartListMember(object? sender, EventArgs e)
        {
            if (sender == null)
            {
                return;
            }
            var startListMemberToMoveUp = (StartListMemberModel) sender;
            if (startListMemberToMoveUp.Startposition == 1)
            {
                return;
            }

            int oldStartPosition = startListMemberToMoveUp.Startposition;
            var startListMemberToMoveDown = RunningRaceStartList.StartListMembers.FirstOrDefault(startListMember => startListMember.Startposition == (oldStartPosition - 1));
            if (startListMemberToMoveDown == null)
            {
                return;
            }

            int newStartPosition = startListMemberToMoveDown.Startposition;
            bool movedToOldStartPosition =  await _startListLogic.UpdateStartListMemberStartPosition(RunningRace.Id, startListMemberToMoveDown.Skier.Id,
                startListMemberToMoveDown.RunNo, oldStartPosition);
            if (movedToOldStartPosition)
            {
                bool movedToNewStartPosition = await _startListLogic.UpdateStartListMemberStartPosition(RunningRace.Id, startListMemberToMoveUp.Skier.Id,
                    startListMemberToMoveUp.RunNo, newStartPosition);   
            }
            Init();
        }

        private async Task<ICollection<SkierModel>> GetPossibleSkiersNotInStartList(ICollection<StartListMemberModel> startListMembers)
        {
            var possibleSkiersNotInStartList = await _startListLogic.GetAllSkiersWithSameSex(RunningRace.Sex);
            foreach (var startListMember in startListMembers)
            {
                SkierModel skierToRemove = possibleSkiersNotInStartList.FirstOrDefault(possibleSkierNotInStartList =>
                    possibleSkierNotInStartList.Id == startListMember.Skier.Id);
                var removed = possibleSkiersNotInStartList.Remove(skierToRemove);
            }

            return possibleSkiersNotInStartList;
        }

        private async Task<StartListModel> GetRunningRaceStartList(RaceModel runningRace)
        {
            if (runningRace == null)
            {
                return null;
            }
            StartListModel startListModel = await _startListLogic.GetStartListForRaceId(runningRace.Id);
            return startListModel;
        }

        private async Task<RaceModel> GetRunningRace()
        {
            var raceModels = await _raceManagementLogic.GetRaces();
            var runningRaceModel = raceModels.
                FirstOrDefault(raceModel => raceModel.Status.Name.Equals("running"));

            return runningRaceModel;
        }
    }
}
