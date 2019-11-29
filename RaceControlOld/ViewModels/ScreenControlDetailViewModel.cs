using System;
using System.Linq;
using System.Threading.Tasks;

using RaceControlOld.Core.Models;
using RaceControlOld.Core.Services;
using RaceControlOld.Helpers;

namespace RaceControlOld.ViewModels
{
    public class ScreenControlDetailViewModel : Observable
    {
        private SampleOrder _item;

        public SampleOrder Item
        {
            get { return _item; }
            set { Set(ref _item, value); }
        }

        public ScreenControlDetailViewModel()
        {
        }

        public async Task InitializeAsync(long orderID)
        {
            var data = await SampleDataService.GetContentGridDataAsync();
            Item = data.First(i => i.OrderID == orderID);
        }
    }
}
