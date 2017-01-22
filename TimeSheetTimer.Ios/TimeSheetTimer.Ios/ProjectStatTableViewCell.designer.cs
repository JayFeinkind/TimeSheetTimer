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
    [Register ("ProjectStatTableViewCell")]
    partial class ProjectStatTableViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView _barView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel _percentageLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel _projectNameLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint BarViewWidth { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_barView != null) {
                _barView.Dispose ();
                _barView = null;
            }

            if (_percentageLabel != null) {
                _percentageLabel.Dispose ();
                _percentageLabel = null;
            }

            if (_projectNameLabel != null) {
                _projectNameLabel.Dispose ();
                _projectNameLabel = null;
            }

            if (BarViewWidth != null) {
                BarViewWidth.Dispose ();
                BarViewWidth = null;
            }
        }
    }
}