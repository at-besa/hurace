using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Hurace.Core.Logic;
using Hurace.Core.Logic.Model;
using RaceControl.Helpers;

namespace RaceControl.ViewModels
{
    public class RaceControlViewModel
    {
        public ObservableCollection<RaceModel> Source { get; } = new ObservableCollection<RaceModel>();
        private RaceManagementLogic raceManagementLogic;
        
        public RaceControlViewModel()
        {
            raceManagementLogic = new RaceManagementLogic();
        }

        public async Task LoadDataAsync()
        {
            Source.Clear();

            // TODO WTS: Replace this with your actual data
            var data = await raceManagementLogic.GetRaces();

            foreach (var item in data)
            {
                Source.Add(item);
            }
        }
    }
}
