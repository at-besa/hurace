﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MaterialDesignThemes.Wpf;
using RaceControl.Helpers;
using RaceControl.ViewModels;

namespace RaceControl
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			ApplyBase(true);
			DataContext = new RaceManagementViewModel();
		}

		private void RaceManagement_Clicked(object sender, RoutedEventArgs e)
		{
			DataContext = new RaceManagementViewModel();
		}

		private void RaceControlButton_Clicked(object sender, RoutedEventArgs e)
		{
			DataContext = new RaceControlViewModel();
		}

		private void ScreenControlButton_Clicked(object sender, RoutedEventArgs e)
		{
			DataContext = new ScreenControlViewModel();
		}

		private void StartListButton_Clicked(object sender, RoutedEventArgs e)
		{
			DataContext = new StartListViewModel();
		}
		
		private static void ApplyBase(bool isDark)
		{
			ModifyTheme(theme => theme.SetBaseTheme(isDark ? Theme.Dark : Theme.Light));
		}

		private static void ModifyTheme(Action<ITheme> modificationAction)
		{
			PaletteHelper paletteHelper = new PaletteHelper();
			ITheme theme = paletteHelper.GetTheme();

			modificationAction?.Invoke(theme);

			paletteHelper.SetTheme(theme);
		}
	}
}
