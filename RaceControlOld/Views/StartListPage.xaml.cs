using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using RaceControlOld.ViewModels;

namespace RaceControlOld.Views
{
    public sealed partial class StartListPage : Page
    {
        public StartListViewModel ViewModel { get; } = new StartListViewModel();

        // TODO WTS: Change the grid as appropriate to your app, adjust the column definitions on StartListPage.xaml.
        // For more details see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid
        public StartListPage()
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
