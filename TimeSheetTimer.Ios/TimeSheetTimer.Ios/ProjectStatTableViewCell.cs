using Foundation;
using System;
using UIKit;
using TimeSheetTimer.Mobile.ClassLibrary;
using System.Globalization;
using System.Diagnostics;
using System.Drawing;
using CoreAnimation;

namespace TimeSheetTimer.Ios
{
    public partial class ProjectStatTableViewCell : UITableViewCell
    {
		int MinBarWidth = 40;
		public double Percentage { get; set; } = 0;
		public ProjectDto Project { get; set; }

        public ProjectStatTableViewCell (IntPtr handle) : base (handle)
        {
        }

		private void SetFonts()
		{
			if (TraitCollection.HorizontalSizeClass == UIUserInterfaceSizeClass.Compact)
			{
				_projectNameLabel.Font = UIFont.BoldSystemFontOfSize(18);
			}
			else
			{
				_projectNameLabel.Font = UIFont.BoldSystemFontOfSize(24);
			}
		}

		private void SetBarWidth()
		{
			var maxBarWidth = ContentView.Frame.Width - _barView.Frame.X - 10;
			nfloat newWidth = MinBarWidth;

			float perc = (float)Percentage / 100f;
			newWidth = maxBarWidth * perc;
			BarViewWidth.Constant = newWidth > MinBarWidth ? newWidth : MinBarWidth;
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			_percentageLabel.Text = Percentage + CultureInfo.CurrentCulture.NumberFormat.PercentSymbol;
			_barView.Layer.CornerRadius = 10;

			_projectNameLabel.Text = Project?.Name;
			_projectNameLabel.TextColor = ProjectsListViewController.DefaultTextColor;

			SetFonts();

			SetBarWidth();
		}


    }
}