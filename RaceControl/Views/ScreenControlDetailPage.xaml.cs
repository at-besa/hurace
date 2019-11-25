using System;

using Microsoft.Toolkit.Uwp.UI.Animations;

using RaceControl.Services;
using RaceControl.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace RaceControl.Views
{
    public sealed partial class ScreenControlDetailPage : Page
    {
        public ScreenControlDetailViewModel ViewModel { get; } = new ScreenControlDetailViewModel();

        public ScreenControlDetailPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is long orderID)
            {
                await ViewModel.InitializeAsync(orderID);
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            if (e.NavigationMode == NavigationMode.Back)
            {
                NavigationService.Frame.SetListDataItemForNextConnectedAnimation(ViewModel.Item);
            }
        }
    }
}
