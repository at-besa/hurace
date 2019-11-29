using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using RaceControlOld.ViewModels;

namespace RaceControlOld.Views
{
    public sealed partial class RacePage : Page
    {
        public RaceViewModel ViewModel { get; } = new RaceViewModel();

        public RacePage()
        {
            InitializeComponent();
            Loaded += RacePage_Loaded;
        }

        private async void RacePage_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadDataAsync(MasterDetailsViewControl.ViewState);
        }
    }
}
