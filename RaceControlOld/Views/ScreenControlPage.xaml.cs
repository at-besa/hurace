using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using RaceControlOld.ViewModels;

namespace RaceControlOld.Views
{
    public sealed partial class ScreenControlPage : Page
    {
        public ScreenControlViewModel ViewModel { get; } = new ScreenControlViewModel();

        public ScreenControlPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            await ViewModel.LoadDataAsync();
        }
    }
}
