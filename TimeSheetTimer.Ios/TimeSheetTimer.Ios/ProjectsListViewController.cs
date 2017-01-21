using Foundation;
using System;
using UIKit;
using TimeSheetTimer.Mobile.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TimeSheetTimer.Ios
{
    public partial class ProjectsListViewController : UIViewController
    {
		ProjectViewModel _viewModel = AppDelegate.DependencyService.Resolve<ProjectViewModel>();

		public static UIColor DefaultTextColor = UIColor.FromRGB(11, 211, 0);

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

			if (NavigationItem != null)
			{
				var clearAllButton = new UIBarButtonItem();
				var stopAll = new UIBarButtonItem();

				clearAllButton.SetTitleTextAttributes(new UITextAttributes
				{
					TextColor = UIColor.Red,
					Font = UIFont.BoldSystemFontOfSize(18)
				}, UIControlState.Normal);

				stopAll.SetTitleTextAttributes(new UITextAttributes
				{
					TextColor = UIColor.Orange,
					Font = UIFont.BoldSystemFontOfSize(18)
				}, UIControlState.Normal);

				stopAll.Title = "Stop All";
				stopAll.Clicked += StopAllClicked;

				clearAllButton.Title = "Clear All";
				clearAllButton.Clicked += ClearAllClicked;

				NavigationItem.RightBarButtonItem = clearAllButton;
				NavigationItem.LeftBarButtonItem = stopAll;
			}

			_viewModel.NewProjectSaved += NewProjectSaved;

			_projectsTableView.TableFooterView = new UIView ();
			_projectsTableView.BackgroundColor = UIColor.FromRGBA (0, 0, 0, 0);
			_projectsTableView.EstimatedRowHeight = 60;
			_projectsTableView.RowHeight = UITableView.AutomaticDimension;
			_projectsTableView.SeparatorInset = new UIEdgeInsets(0, -10, 0, 0);
		}

		public async override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			try
			{
				await _viewModel?.Start ();

				_projectsTableView.Source = new ProjectListTableViewSource (_viewModel, ShowNewProjectDialog);
				_projectsTableView.ReloadData ();
			}
			catch (Exception e)
			{
			}
		}

		private void StopAllClicked(object sender, EventArgs args)
		{
			foreach (var project in _viewModel.AllProjects)
			{
				project.Stop();

				project.RecordStack.Clear();
			}

			_projectsTableView?.ReloadData();
		}

		private async void ClearAllClicked(object sender, EventArgs args)
		{
			foreach (var project in _viewModel.AllProjects)
			{
				await _viewModel.DeleteRecords(project.RecordStack.Where(r => r.Id != 0).ToList());

				project.RecordStack.Clear();
			}

			_projectsTableView?.ReloadData();
		}

		public async Task OnTransitionToBackground()
		{
			foreach (var project in _viewModel.AllProjects)
			{
				if (project.IsRunning())
				{
					project.Stop();
					await _viewModel.SaveTimeRecord(project.RecordStack.Peek());
				}
			}
		}

		private void NewProjectSaved (int index)
		{
			if (index <= _viewModel.AllProjects.Count)
			{
				_projectsTableView.BeginUpdates ();

				// add one to index for the New Project row
				_projectsTableView.InsertRows (new NSIndexPath [] { NSIndexPath.FromRowSection (index + 1, 0) }, UITableViewRowAnimation.Automatic);

				_projectsTableView.EndUpdates ();
			}
		}

		private async Task AddNewProject (string name)
		{
			if (_viewModel.AllProjects.Any (p => p.Name?.ToLower () == name.ToLower ()))
			{
				var alert = UIAlertController.Create ("Oops", "Project by that name already exists", UIAlertControllerStyle.Alert);
				alert.AddAction (UIAlertAction.Create ("Ok", UIAlertActionStyle.Default, null));
				await PresentViewControllerAsync (alert, true);
			}
			else
			{
				await _viewModel.SaveProject (name);
			}
		}

		private void ShowNewProjectDialog ()
		{
			var alert = UIAlertController.Create ("Add New Project", string.Empty, UIAlertControllerStyle.Alert);

			alert.AddTextField ((UITextField obj) => {
				
				obj.Placeholder = "Name";

				obj.ShouldChangeCharacters = (textField, range, replacementString) => {

					if (string.IsNullOrWhiteSpace (replacementString) &&
						range.Location == 0 &&
					    range.Length == textField.Text.Length)
					{
						alert.Actions [0].Enabled = false;
					}
					else
					{
						alert.Actions [0].Enabled = true;
					}

					if (string.IsNullOrWhiteSpace (replacementString) && (range.Length + range.Location > 40))
						return false;

					if (replacementString == " " && textField.Text.Length == 40)
						return false;

					if (string.IsNullOrWhiteSpace (replacementString))
						return true;

					if (textField.Text.Length + replacementString.Length > 40)
						return false;

					return true;
				};
			});

			alert.AddAction (UIAlertAction.Create ("Ok", UIAlertActionStyle.Default, async (UIAlertAction obj) => {
				await AddNewProject (alert.TextFields [0].Text);	
			}));

			alert.AddAction (UIAlertAction.Create ("Cancel", UIAlertActionStyle.Default, null));

			alert.Actions [0].Enabled = false;

			PresentViewController (alert, true, null);
		}

		internal class ProjectListTableViewSource : UITableViewSource
		{
			ProjectViewModel _viewModel;

			NSIndexPath _lastSelectedRow;
			Action _addNewProjectHandler;

			public ProjectListTableViewSource (ProjectViewModel viewModel, Action AddNewProjectHandler)
			{
				_addNewProjectHandler = AddNewProjectHandler;
				_viewModel = viewModel;
			}

			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				if (indexPath.Row == 0)
				{
					var cell = tableView.DequeueReusableCell ("NewProjectTableViewCell") as NewProjectTableViewCell;

					if (indexPath.Row == _lastSelectedRow?.Row)
					{
						cell.BackgroundColor = UIColor.FromRGBA (0, 0, 0, 0.25f);
					}
					else
					{
						cell.BackgroundColor = UIColor.FromRGBA (0, 0, 0, 0);
					}

					cell.Label.TextColor = DefaultTextColor;

					cell.SelectionStyle = UITableViewCellSelectionStyle.None;
					return cell;
				}
				else
				{
					var cell = tableView.DequeueReusableCell ("ProjectTimeTableViewCell") as ProjectTimeTableViewCell;
					var project = _viewModel?.AllProjects [indexPath.Row - 1];
					cell.SelectionStyle = UITableViewCellSelectionStyle.None;

					cell.UpdateCell (project);
					return cell;
				}
			}

			public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
			{
				return indexPath.Row != 0;
			}

			public override UITableViewRowAction[] EditActionsForRow(UITableView tableView, NSIndexPath indexPath)
			{
				var project = _viewModel.AllProjects[indexPath.Row - 1];

				UITableViewRowAction editAction = UITableViewRowAction.Create(
					UITableViewRowActionStyle.Default,
					"Clear",
					async (action, path) => 
					{ 
						await _viewModel.DeleteRecords(project.RecordStack.ToList());
						project.RecordStack.Clear();

						tableView.BeginUpdates();
						tableView.ReloadRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Automatic);
						tableView.EndUpdates();
					}
				);

				editAction.BackgroundColor = UIColor.Orange;

				UITableViewRowAction deleteAction = UITableViewRowAction.Create(
					UITableViewRowActionStyle.Default,
					"Delete",
					async (action, path) =>
					{
						await _viewModel.DeleteProject(project);

						tableView.BeginUpdates();
						tableView.DeleteRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Automatic);
						tableView.EndUpdates();
					}
				);

				deleteAction.BackgroundColor = UIColor.Red;

				return new UITableViewRowAction[] { deleteAction, editAction };
			}

			public override nint RowsInSection (UITableView tableview, nint section)
			{
				return 1 + _viewModel.AllProjects.Count;
			}

			public async override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				if (indexPath.Row != 0)
				{
					var project = _viewModel.AllProjects [indexPath.Row - 1];

					if (project.IsRunning ())
					{
						project.Stop ();
						await _viewModel.SaveTimeRecord(project.RecordStack.Peek());
					}
					else
					{
						project.Start ();
					}
				}

				var paths = new List<NSIndexPath>() { indexPath };

				if (_lastSelectedRow != null && _lastSelectedRow.Row == 0)
				{
					paths.Add(_lastSelectedRow);
				}

				_lastSelectedRow = indexPath;

				tableView.BeginUpdates ();
				tableView.ReloadRows (paths.ToArray(), UITableViewRowAnimation.None);
				tableView.EndUpdates ();

				if (indexPath.Row == 0)
				{
					_addNewProjectHandler?.Invoke ();
				}

			}


		}
	}
}