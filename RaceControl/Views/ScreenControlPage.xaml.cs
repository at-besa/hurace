using System;

using RaceControl.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace RaceControl.Views
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
