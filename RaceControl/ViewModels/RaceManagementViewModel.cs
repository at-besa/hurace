using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Hurace.Core.DAL.Domain;
using Hurace.Core.Logic;
using Hurace.Core.Logic.Interface;
using Hurace.Core.Logic.Model;
using RaceControl.Helpers;
using RaceControl.ViewModels.RaceManagementHelpers;
using Swack.UI.ViewModels;
using NotifyPropertyChanged = RaceControl.Helpers.NotifyPropertyChanged;

namespace RaceControl.ViewModels
{
    public class RaceManagementViewModel : NotifyPropertyChanged
	{
	    private readonly IRaceManagementLogic managementManagementLogic = RaceManagementLogic.Instance;
	    private RaceViewModel selectedRaceViewModel;
	    public ObservableCollection<RaceViewModel> RaceViewModels { get; } = new ObservableCollection<RaceViewModel>();

	    public RaceViewModel SelectedRaceViewModel
	    {
		    get => selectedRaceViewModel;
		    set
		    {
			    Set(ref selectedRaceViewModel, value);
				if (selectedRaceViewModel != null)
				{
					OnSelectedItemChanged();
				}
		    }
	    }

	    private bool isSelected;
	    private string selectedRaceType = "";
	    private string selectedState = "";

	    public bool IsSelected {
		    get => isSelected;
		    set => Set(ref isSelected, value);
	    }

		public ICollection<string> RaceTypes { get; set; } = new List<string>();

		public string SelectedRaceType
		{
			get => selectedRaceType;
			set => Set(ref selectedRaceType, value);
		}

		private string errorText;

		public string ErrorText
		{
			get => errorText;
			set => Set(ref errorText, value);
		}

		public ICollection<string> RaceStates { get; set; } = new List<string>();

		public string SelectedState
		{
			get => selectedState;
			set => Set(ref selectedState, value);
		}

		public ICollection<string> Genders { get; set; } = new List<string>{"m", "f"};

        public CommandBase SaveCommand { get; set; }
        public CommandBase DeleteCommand { get; set; }
        public CommandBase CreateNewRaceCommand { get; set; }
		
		public RaceManagementViewModel()
		{
			GetRaces();
            GetRaceTypes();
            GetRaceStates();
            
            SaveCommand = new CommandBase(SaveRace);
            DeleteCommand = new CommandBase(DeleteRace);
            CreateNewRaceCommand = new CommandBase(CreateNewRace);

		}

		private void OnSelectedItemChanged()
		{
			if (SelectedRaceViewModel != null)
			{
				SelectedRaceType = selectedRaceViewModel.RaceModel.Type.Type;
				SelectedState = selectedRaceViewModel.RaceModel.Status.Name;
			}

		}

		private async void GetRaces()
        {
	        var raceModels = await managementManagementLogic.GetRaces();

	        foreach (var raceModel in raceModels)
	        {
		        var raceViewModel = new RaceViewModel(raceModel);
		        RaceViewModels.Add(raceViewModel);
	        }

	        if (raceModels.Count >= 1)
	        {
		        SelectedRaceViewModel = new RaceViewModel(raceModels.FirstOrDefault(raceModel => raceModel.Id != -1));
	        }

        }

        private async void GetRaceTypes()
        {
	        var types = await managementManagementLogic.GetRaceTypes();
	        foreach (var type in types)
	        {
		        RaceTypes.Add(type);
	        }
        }

        private async void GetRaceStates()
        {
	        var states = await managementManagementLogic.GetRaceStates();
	        foreach (var state in states)
	        {
		        RaceStates.Add(state);
		        
	        }
        }
        
        private async void SaveRace(object sender, EventArgs eventArgs)
        {
	        SelectedRaceViewModel.RaceModel.Type.Type = SelectedRaceType;
	        SelectedRaceViewModel.RaceModel.Status.Name = SelectedState;
	        if (SelectedState == "running" && await RunningRaceAlreadyExisits())
	        {
		        ErrorText = "There is only one running race possible! Change status!";
		        return;
	        }

	        ErrorText = "";
	        if (SelectedRaceViewModel.NewRace)
	        {
		        await managementManagementLogic.CreateRace(SelectedRaceViewModel.RaceModel);
		        SelectedRaceViewModel.NewRace = false;
	        }
	        else
	        {
				await managementManagementLogic.SaveRace(SelectedRaceViewModel.RaceModel);
			}
            
        }

        private async Task<bool> RunningRaceAlreadyExisits()
        {
	        var raceModels = await managementManagementLogic.GetRaces();
	        var runningRaceModel = raceModels.
		        FirstOrDefault(raceModel => raceModel.Status.Name.Equals("running"));
	        if (runningRaceModel == null)
	        {
		        return false;
	        }

	        return true;
        }

        private async void DeleteRace(object sender, EventArgs e)
        {
	        await managementManagementLogic.DeleteRace(SelectedRaceViewModel.RaceModel.Id);
	        RaceViewModels.Remove(SelectedRaceViewModel);
	        SelectedRaceViewModel = RaceViewModels[0];
        }

        private void CreateNewRace(object sender, EventArgs e)
        {
	        SelectedRaceViewModel = new RaceViewModel(
		        new RaceModel(new Race{
	                    Type = new RaceType(),
	                    Status = new Status(),
	                    Name = "New Race",
	                    Date = DateTime.Now,
	                    Location = "",
	                    Sex = "m",
	                    Splittimes = 1,
		        }),
		        true
		        );
            RaceViewModels.Add(SelectedRaceViewModel);
        }



        //public event PropertyChangedEventHandler PropertyChanged;

        //[NotifyPropertyChangedInvocator]
        //protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        //{
        // PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}
    }

}
