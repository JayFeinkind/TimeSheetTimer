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

		public static UIColor DefaultTextColor = UIColor.FromRGB(138, 222, 69); //UIColor.FromRGB(11, 211, 0);

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
				NavigationItem.Title = "Project Timers";

				var clearAllButton = new UIBarButtonItem();
				var actions = new UIBarButtonItem (UIBarButtonSystemItem.Action);

				actions.TintColor = UIColor.Black;

				clearAllButton.SetTitleTextAttributes(new UITextAttributes
				{
					TextColor = UIColor.Red,
					Font = UIFont.BoldSystemFontOfSize(18)
				}, UIControlState.Normal);

				clearAllButton.Title = "Clear All";

				clearAllButton.Clicked += ClearAllClicked;
				actions.Clicked += ShowActionSheet;

				NavigationItem.RightBarButtonItem = actions;
				NavigationItem.LeftBarButtonItem = clearAllButton;
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

				SetTableType(ProjectListDisplayType.Timer);
			}
			catch (Exception e)
			{
			}
		}

		private void SetTableType(ProjectListDisplayType displayType)
		{
			if (NavigationItem != null)
			{
				switch (displayType)
				{
					case ProjectListDisplayType.Timer:
						NavigationItem.Title = "Project Timers";
						break;
					case ProjectListDisplayType.Stat:
						NavigationItem.Title = "Project Statistics";
						break;
				}
			}

			_projectsTableView.AllowsSelection = displayType != ProjectListDisplayType.Stat;

			_projectsTableView.Source = new ProjectListTableViewSource(_viewModel, ShowNewProjectDialog, displayType);
			_projectsTableView.ReloadData();
		}

		private async void ShowActionSheet(object sender, EventArgs args)
		{
			var popUp = UIAlertController.Create("Page Display", "Change current page display type", UIAlertControllerStyle.ActionSheet);

			// on iPhone PopoverPresentationController will be null as action sheets show from the bottom
			//   of the screen on iPhones. On iPad you can specify where the action sheet shows.
			if (popUp.PopoverPresentationController != null)
			{
				popUp.PopoverPresentationController.BarButtonItem = sender as UIBarButtonItem;
				popUp.PopoverPresentationController.PermittedArrowDirections = UIPopoverArrowDirection.Up;
			}

			var source = _projectsTableView?.Source as ProjectListTableViewSource;

			if (source?.CurrentDisplayType != ProjectListDisplayType.Timer)
			{
				popUp.AddAction(UIAlertAction.Create("Timer", UIAlertActionStyle.Default, (action) => {
					SetTableType(ProjectListDisplayType.Timer);
				}));
			}

			if (source?.CurrentDisplayType != ProjectListDisplayType.Stat)
			{
				popUp.AddAction(UIAlertAction.Create("Statistics", UIAlertActionStyle.Default, (action) => {
					SetTableType(ProjectListDisplayType.Stat);
				}));
			}

			if (UIDevice.CurrentDevice.UserInterfaceIdiom != UIUserInterfaceIdiom.Pad)
			{
				popUp.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, async (action) => {
					await popUp.DismissViewControllerAsync(true);
				}));
			}

			await PresentViewControllerAsync(popUp, true);
		}

		private async void ClearAllClicked(object sender, EventArgs args)
		{
			foreach (var project in _viewModel.AllProjects)
			{
				await _viewModel.DeleteRecords(project.RecordStack.Where(r => r.Id != 0).ToList());
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

		internal enum ProjectListDisplayType
		{
			Timer,
			Stat
		}

		internal class ProjectListTableViewSource : UITableViewSource
		{
			ProjectViewModel _viewModel;
			Action _addNewProjectHandler;
			ProjectListDisplayType _displayType;
			NSIndexPath _lastSelectedIndex;

			public ProjectListDisplayType CurrentDisplayType
			{
				get
				{
					return _displayType; 
				}
			}

			public ProjectListTableViewSource (ProjectViewModel viewModel, Action AddNewProjectHandler, ProjectListDisplayType type)
			{
				_displayType = type;
				_addNewProjectHandler = AddNewProjectHandler;
				_viewModel = viewModel;
			}

			private UITableViewCell GetTimerCell(UITableView tableView, NSIndexPath indexPath)
			{
				if (indexPath.Row == 0)
				{
					var cell = tableView.DequeueReusableCell("NewProjectTableViewCell") as NewProjectTableViewCell;

					if (indexPath.Row == _lastSelectedIndex?.Row)
					{
						cell.BackgroundColor = UIColor.FromRGBA(0, 0, 0, 0.25f);
					}
					else
					{
						cell.BackgroundColor = UIColor.FromRGBA(0, 0, 0, 0);
					}

					cell.Label.TextColor = DefaultTextColor;

					cell.SelectionStyle = UITableViewCellSelectionStyle.None;
					return cell;
				}
				else
				{
					var cell = tableView.DequeueReusableCell("ProjectTimeTableViewCell") as ProjectTimeTableViewCell;
					var project = _viewModel?.AllProjects[indexPath.Row - 1];
					cell.SelectionStyle = UITableViewCellSelectionStyle.None;

					cell.UpdateCell(project);
					return cell;
				}
			}

			private UITableViewCell GetStatCell(UITableView tableView, NSIndexPath indexPath)
			{
				var project = _viewModel?.AllProjects[indexPath.Row];

				var cell = tableView.DequeueReusableCell("ProjectStatTableViewCell") as ProjectStatTableViewCell;

				cell.BackgroundColor = UIColor.FromRGBA(0, 0, 0, 0);

				double projectSum = project.RecordStack.Sum(r => r.Seconds);
				double totalSum = _viewModel.AllProjects.Sum(p => p.RecordStack.Sum(r => r.Seconds));

				cell.Project = project;
				cell.Percentage = Math.Round(((projectSum / totalSum) * 100), 1, MidpointRounding.AwayFromZero);
				cell.SelectionStyle = UITableViewCellSelectionStyle.None;

				return cell;
			}

			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				switch (_displayType)
				{
					case ProjectListDisplayType.Stat:
						return GetStatCell(tableView, indexPath);
					default:
						return GetTimerCell(tableView, indexPath);
				}
			}

			public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
			{
				switch (_displayType)
				{
					case ProjectListDisplayType.Timer:
						return indexPath.Row != 0;
					default:
						return false;
				}

			}

			public override UITableViewRowAction[] EditActionsForRow(UITableView tableView, NSIndexPath indexPath)
			{
				var project = _viewModel.AllProjects[indexPath.Row - 1];

				UITableViewRowAction editAction = UITableViewRowAction.Create(
					UITableViewRowActionStyle.Default,
					"Clear Time",
					async (action, path) => 
					{ 
						await _viewModel.DeleteRecords(project.RecordStack.ToList());

						tableView.BeginUpdates();
						tableView.ReloadRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Automatic);
						tableView.EndUpdates();
					}
				);

				editAction.BackgroundColor = UIColor.Orange;

				UITableViewRowAction deleteAction = UITableViewRowAction.Create(
					UITableViewRowActionStyle.Default,
					"Delete Project",
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
				switch (_displayType)
				{
					case ProjectListDisplayType.Timer:
						return 1 + _viewModel.AllProjects.Count;
					default:
						return _viewModel.AllProjects.Count;
				}

			}

			/// <summary>
			/// Handles the timer row pressed.
			/// </summary>
			/// <returns>List of IndexPath's indicating which rows have changed.</returns>
			/// <param name="atIndex">At index.</param>
			private async Task<List<NSIndexPath>> HandleTimerPressed(NSIndexPath atIndex)
			{
				var indexPaths = new List<NSIndexPath>() { atIndex };

				if (atIndex.Row != 0)
				{
					var project = _viewModel.AllProjects[atIndex.Row - 1];

					if (project.IsRunning())
					{
						project.Stop();
						await _viewModel.SaveTimeRecord(project.RecordStack.Peek());
					}
					else
					{
						project.Start();

						var runningProjects = _viewModel.AllProjects.FindAll(p => p.IsRunning() && p.Id != project.Id);

						foreach (var proj in runningProjects)
						{
							proj.Stop();
							await _viewModel.SaveTimeRecord(proj.RecordStack.Peek());

							indexPaths.Add(NSIndexPath.FromRowSection(_viewModel.AllProjects.IndexOf(proj) + 1, 0));
						}
					}
				}

				return indexPaths;
			}

			private async Task TimerRowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				var affectedPaths = await HandleTimerPressed(indexPath);

				var paths = new List<NSIndexPath> { indexPath };
				paths.AddRange(affectedPaths);

				if (_lastSelectedIndex != null && _lastSelectedIndex.Row == 0 && indexPath.Row != 0)
				{
					paths.Add(NSIndexPath.FromRowSection(0, 0));
				}

				_lastSelectedIndex = indexPath;

				tableView.BeginUpdates();
				tableView.ReloadRows(paths.ToArray(), UITableViewRowAnimation.None);
				tableView.EndUpdates();

				// shows the new project pop up
				if (indexPath.Row == 0)
				{
					_addNewProjectHandler?.Invoke();
				}
			}

			public async override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				switch (_displayType)
				{
					case ProjectListDisplayType.Timer:
						await TimerRowSelected(tableView, indexPath);
						break;
				}

				//if ((indexPath.Row == 0 || _lastSelectedIndex?.Row == 0) && _displayType == ProjectListDisplayType.Timer)
				//{
				//	_lastSelectedIndex = indexPath;

				//	tableView.BeginUpdates();
				//	tableView.ReloadRows(new NSIndexPath[] { NSIndexPath.FromRowSection(0, 0) }, UITableViewRowAnimation.None);
				//	tableView.EndUpdates();
				//}
				//else
				//{
				//	_lastSelectedIndex = indexPath;
				//}
			}


		}
	}
}