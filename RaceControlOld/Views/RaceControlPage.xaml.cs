using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using RaceControlOld.ViewModels;

namespace RaceControlOld.Views
{
    public sealed partial class RaceControlPage : Page
    {
        public RaceControlViewModel ViewModel { get; } = new RaceControlViewModel();

        // TODO WTS: Change the grid as appropriate to your app, adjust the column definitions on RaceControlPage.xaml.
        // For more details see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid
        public RaceControlPage()
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
