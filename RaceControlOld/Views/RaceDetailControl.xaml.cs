using System;

using RaceControlOld.Core.Models;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace RaceControlOld.Views
{
    public sealed partial class RaceDetailControl : UserControl
    {
        public SampleOrder MasterMenuItem
        {
            get { return GetValue(MasterMenuItemProperty) as SampleOrder; }
            set { SetValue(MasterMenuItemProperty, value); }
        }

        public static readonly DependencyProperty MasterMenuItemProperty = DependencyProperty.Register("MasterMenuItem", typeof(SampleOrder), typeof(RaceDetailControl), new PropertyMetadata(null, OnMasterMenuItemPropertyChanged));

        public RaceDetailControl()
        {
            InitializeComponent();
        }

        private static void OnMasterMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as RaceDetailControl;
            control.ForegroundElement.ChangeView(0, 0, 1);
        }
    }
}
