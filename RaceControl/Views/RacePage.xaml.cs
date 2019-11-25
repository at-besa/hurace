using System;

using RaceControl.ViewModels;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace RaceControl.Views
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
