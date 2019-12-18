using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace RaceControl.Helpers
{
	class BoolToVisConverterInverted : BooleanConverter<Visibility>
	{

		public BoolToVisConverterInverted() :
			base(Visibility.Visible, Visibility.Collapsed)
		{ }
	}
}
