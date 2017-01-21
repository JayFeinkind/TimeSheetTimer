using Foundation;
using System;
using UIKit;
using TimeSheetTimer.Mobile.ClassLibrary;
using System.Timers;
using System.Linq;
using TimeSheetTimer.Mobile.Services;

namespace TimeSheetTimer.Ios
{
    public partial class ProjectTimeTableViewCell : UITableViewCell
    {
		Timer timer = new Timer(1000);
		ProjectDto _dto;

        public ProjectTimeTableViewCell (IntPtr handle) : base (handle)
   		{
			timer.AutoReset = true;
			timer.Elapsed += TimerElapsed;
			timer.Start ();
        }

		private void TimerElapsed (object sender, ElapsedEventArgs e)
		{
			if (_dto?.IsRunning() != true)
			{
				return;
			}

			// Timer is invoked on background thread, only UI thread can update UI.
			InvokeOnMainThread(() =>
			{
				_timeLabel.Text = AppUtilityService.FormatedTotal(_dto.RecordStack.Peek().Seconds);
			});
		}

		public void UpdateCell (ProjectDto dto)
		{
			_dto = dto;

			_nameLabel.TextColor = UIColor.FromRGB(101, 46, 0);

			_nameLabel.Text = dto.Name;

			_timeLabel.Text = "Total: " + AppUtilityService.FormatedTotal(
				_dto.RecordStack.Count > 0 ? 
				_dto.RecordStack.Sum(r => r.Seconds) : 
				0);
		}
    }
}