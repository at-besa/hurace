using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using RaceControl.Core.Models;
using RaceControl.Core.Services;
using RaceControl.Helpers;

namespace RaceControl.ViewModels
{
    public class RaceControlViewModel : Observable
    {
        public ObservableCollection<SampleOrder> Source { get; } = new ObservableCollection<SampleOrder>();

        public RaceControlViewModel()
        {
        }

        public async Task LoadDataAsync()
        {
            Source.Clear();

            // TODO WTS: Replace this with your actual data
            var data = await SampleDataService.GetGridDataAsync();

            foreach (var item in data)
            {
                Source.Add(item);
            }
        }
    }
}
