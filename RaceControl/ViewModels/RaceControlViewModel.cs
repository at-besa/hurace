using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Hurace.Core.Logic;
using Hurace.Core.Logic.Model;
using RaceControl.Helpers;
using Swack.UI.ViewModels;

namespace RaceControl.ViewModels
{
    public class RaceControlViewModel : NotifyPropertyChanged
    {
        private readonly RaceControlLogic raceControlLogic = RaceControlLogic.Instance;
        private readonly RaceManagementLogic raceManagementLogic = RaceManagementLogic.Instance;
        private RaceControlModel raceControlModel;
        public ObservableCollection<RaceModel> Source { get; } = new ObservableCollection<RaceModel>();

        public RaceControlModel RaceControlModel
        {
	        get => raceControlModel;
	        set => Set(ref raceControlModel, value);
        }

        public RaceControlViewModel()
        {
            Init();
        }

        private async void Init()
        {
            await LoadDataAsync();
        }


        private async Task LoadDataAsync()
        {
            Source.Clear();
            var race = await raceManagementLogic.GetRunningRace();
            RaceControlModel = await raceControlLogic.GetRaceControlForRaceId(race.Id);
        }

    }
}
