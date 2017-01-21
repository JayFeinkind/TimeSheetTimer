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
		Timer timer = new Timer(250);
		ProjectDto _dto;

        public ProjectTimeTableViewCell (IntPtr handle) : base (handle)
   		{
			timer.AutoReset = true;
			timer.Elapsed += TimerElapsed;
			timer.Start ();
        }

		private void TimerElapsed (object sender, ElapsedEventArgs e)
		{
			try
			{
				if (_dto?.IsRunning() == true)
				{
					// Timer is invoked on background thread, only UI thread can update UI.
					InvokeOnMainThread(() =>
					{
						_timeLabel.Text = AppUtilityService.FormatedTotal(_dto.RecordStack.Peek().Seconds);
					});
				}
			}
			catch (Exception exc)
			{
				
			}
		}

		public void UpdateCell (ProjectDto dto)
		{
			try
			{
				_timeLabel.TextColor = UIColor.White;

				_dto = dto;

				if (dto.IsRunning())
				{
					BackgroundColor = UIColor.FromRGBA(0, 30, 0, 0.2f);
				}
				else
				{
					BackgroundColor = UIColor.FromRGBA(0, 0, 0, 0);
				}

				if (dto?.IsRunning() == false)
				{
					_nameLabel.TextColor = ProjectsListViewController.DefaultTextColor;
					_timeLabel.TextColor = UIColor.White;
					_timeLabel.Text = "Total: " + AppUtilityService.FormatedTotal(_dto.RecordStack.Sum(r => r.Seconds));
				}
				else if (dto != null)
				{
					_nameLabel.TextColor = UIColor.Black;
					_timeLabel.TextColor = UIColor.Black;
					_timeLabel.Text = AppUtilityService.FormatedTotal(_dto.RecordStack.Peek().Seconds);
				}
				else
				{
					_timeLabel.Text = string.Empty;
				}

				_nameLabel.Text = dto.Name;
			}
			catch (Exception e)
			{
			}
		}

    }
}