using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography.Pkcs;
using System.Windows;
using System.Windows.Controls;
using CoSimulationPlcSimAdv.Commands;
using Hurace.Core.DAL.Domain;
using Hurace.Core.Logic;
using Hurace.Core.Logic.Model;

namespace RaceControl.ViewModels
{
    public class RaceManagementViewModel 
    {
	    private RaceLogic logic;
	    private RaceViewModel selectedRaceViewModel;
	    public ObservableCollection<RaceViewModel> RaceViewModels { get; set; } = new ObservableCollection<RaceViewModel>();

	    public RaceViewModel SelectedRaceViewModel
	    {
		    get => selectedRaceViewModel;
		    set
		    {
			    if (selectedRaceViewModel != value)
				{
					selectedRaceViewModel = value;
					if (selectedRaceViewModel != null)
					{
						OnSelectedItemChanged();
					}
				}
		    }
	    }

	    private bool isSelected;
	    public bool IsSelected {
		    get => isSelected;
		    set {
			    if (isSelected != value)
			    {
				    isSelected = value;
			    }
		    }
	    }

		public ObservableCollection<string> RaceTypes { get; set; } = new ObservableCollection<string>();
		public string SelectedRaceType { get; set; } = "";
		public ObservableCollection<string> RaceStates { get; set; } = new ObservableCollection<string>();
		public string SelectedState { get; set; } = "";
        public ICollection<string> Genders { get; set; } = new List<string>{"m", "f"};

        public CommandBase SaveCommand { get; set; }
        public CommandBase DeleteCommand { get; set; }
        public CommandBase CreateNewRaceCommand { get; set; }
		
		public RaceManagementViewModel()
        {
	        logic = new RaceLogic();

            GetRaces();
            GetRaceTypes();
            GetRaceStates();
            
            SaveCommand = new CommandBase(SaveRace);
            DeleteCommand = new CommandBase(DeleteRace);
            CreateNewRaceCommand = new CommandBase(CreateNewRace);

		}

		private void OnSelectedItemChanged()
		{
			Console.WriteLine("iwas here");
			if (SelectedRaceViewModel != null)
			{
				SelectedRaceType = selectedRaceViewModel.RaceModel.Race.Type.Type;
				SelectedState = selectedRaceViewModel.RaceModel.Race.Status.Name;
			}

		}

		private async void GetRaces()
        {
	        var raceModels = await logic.GetRaces();

	        foreach (var raceModel in raceModels)
	        {
		        var raceViewModel = new RaceViewModel(raceModel);
		        RaceViewModels.Add(raceViewModel);
	        }

        }

        private async void GetRaceTypes()
        {
	        var types = await logic.GetRaceTypes();
	        foreach (var type in types)
	        {
		        RaceTypes.Add(type);
	        }
        }

        private async void GetRaceStates()
        {
	        var states = await logic.GetRaceStates();
	        foreach (var state in states)
	        {
		        RaceStates.Add(state);
	        }
        }
        
        private async void SaveRace(object sender, EventArgs eventArgs)
        {
	        SelectedRaceViewModel.RaceModel.Race.Type.Type = SelectedRaceType;
	        SelectedRaceViewModel.RaceModel.Race.Status.Name = SelectedState;
	        if (SelectedRaceViewModel.NewRace)
	        {
		        await logic.CreateRace(SelectedRaceViewModel.RaceModel);
	        }
	        else
	        {
				await logic.SaveRace(SelectedRaceViewModel.RaceModel);
			}
            
        }
        
        private async void DeleteRace(object sender, EventArgs e)
        {
	        await logic.DeleteRace(SelectedRaceViewModel.RaceModel.Race.Id);
	        RaceViewModels.Remove(SelectedRaceViewModel);
	        SelectedRaceViewModel = RaceViewModels[0];
        }

        private void CreateNewRace(object sender, EventArgs e)
        {
	        SelectedRaceViewModel = new RaceViewModel(
		        new RaceModel{
			        Race = new Race{
	                    Type = new RaceType(),
	                    Status = new Status(),
	                    Name = "New Race",
	                    Date = DateTime.Now,
	                    Location = "",
	                    Sex = "m",
	                    Splittimes = 1,
	                }
		        },
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
