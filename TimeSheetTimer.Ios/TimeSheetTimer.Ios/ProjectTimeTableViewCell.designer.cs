// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace TimeSheetTimer.Ios
{
    [Register ("ProjectTimeTableViewCell")]
    partial class ProjectTimeTableViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel _nameLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel _timeLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_nameLabel != null) {
                _nameLabel.Dispose ();
                _nameLabel = null;
            }

            if (_timeLabel != null) {
                _timeLabel.Dispose ();
                _timeLabel = null;
            }
        }
    }
}