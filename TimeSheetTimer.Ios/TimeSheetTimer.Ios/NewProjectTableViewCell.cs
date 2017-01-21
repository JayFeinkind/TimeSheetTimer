using Foundation;
using System;
using UIKit;

namespace TimeSheetTimer.Ios
{
    public partial class NewProjectTableViewCell : UITableViewCell
    {
		public UILabel Label { get { return _titleLabel; } }

        public NewProjectTableViewCell (IntPtr handle) : base (handle)
        {
        }

    }
}