using Foundation;
using System;
using UIKit;

namespace TimeSheetTimer.Ios
{
    public partial class ProjectsListViewController : UIViewController
    {
        public ProjectsListViewController (IntPtr handle) : base (handle)
        {
        }

		public override void LoadView ()
		{
			base.LoadView ();

			if (NavigationController?.NavigationBar != null)
			{
				NavigationController.NavigationBar.Translucent = true;
			}

			_projectsTableView.BackgroundColor = UIColor.FromRGBA (0, 0, 0, 0);
		}
    }
}