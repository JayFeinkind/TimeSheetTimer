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
    [Register ("ProjectsListViewController")]
    partial class ProjectsListViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView _projectsTableView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_projectsTableView != null) {
                _projectsTableView.Dispose ();
                _projectsTableView = null;
            }
        }
    }
}