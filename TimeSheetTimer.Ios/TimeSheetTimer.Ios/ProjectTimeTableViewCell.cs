using Foundation;
using System;
using UIKit;
using TimeSheetTimer.Mobile.ClassLibrary;
using System.Timers;

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
			if (_dto.IsRunning () == false)
				return;

			InvokeOnMainThread (() =>
			 {
				 _timeLabel.Text = _dto.RecordStack.Peek ().Seconds.ToString ();
			});
		}

		public void UpdateCell (ProjectDto dto)
		{
			_dto = dto;
			_nameLabel.Text = dto.Name;

			if (dto.IsRunning ())
			{
				BackgroundColor = UIColor.FromRGBA (76, 217, 100, 0.25f);
			}
			else
			{
				BackgroundColor = UIColor.FromRGBA (0, 0, 0, 0);
			}
		}
    }
}