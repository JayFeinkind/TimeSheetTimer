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

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			if (TraitCollection.HorizontalSizeClass == UIUserInterfaceSizeClass.Compact)
			{
				_titleLabel.Font = UIFont.BoldSystemFontOfSize(18);
			}
			else
			{
				_titleLabel.Font = UIFont.BoldSystemFontOfSize(24);
			}
		}

    }
}