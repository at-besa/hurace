using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Hurace.Core.Logic;
using Hurace.Core.Logic.Model;
using RaceControl.Helpers;

namespace RaceControl.ViewModels
{
    public class RaceControlViewModel : Observable
    {
        public ObservableCollection<RaceModel> Source { get; } = new ObservableCollection<RaceModel>();
        private RaceLogic raceLogic;
        
        public RaceControlViewModel()
        {
            raceLogic = new RaceLogic();
        }

        public async Task LoadDataAsync()
        {
            Source.Clear();

            // TODO WTS: Replace this with your actual data
            var data = await raceLogic.GetRaces();

            foreach (var item in data)
            {
                Source.Add(item);
            }
        }
    }
}
