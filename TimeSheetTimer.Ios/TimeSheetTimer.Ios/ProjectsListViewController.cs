using Foundation;
using System;
using UIKit;
using TimeSheetTimer.Mobile.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheetTimer.Ios
{
    public partial class ProjectsListViewController : UIViewController
    {
		ProjectViewModel _viewModel;
		UIBarButtonItem _clearAllButton;

        public ProjectsListViewController (IntPtr handle) : base (handle)
        {
        }

		private async void ClearAllClicked(object sender, EventArgs args)
		{
			foreach (var project in _viewModel.AllProjects)
			{
				//TODO delete records from db

				if (project.IsRunning())
				{
					project.Stop();
					await _viewModel.SaveTimeRecord(project.RecordStack.Peek());
				}

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

		public override void LoadView ()
		{
			base.LoadView ();

			_clearAllButton = new UIBarButtonItem();

			_clearAllButton.SetTitleTextAttributes(new UITextAttributes { 
				TextColor = UIColor.Red,
				Font = UIFont.BoldSystemFontOfSize(18)
			}, UIControlState.Normal);

			_clearAllButton.Title = "Clear All";
			_clearAllButton.Clicked += ClearAllClicked;

			if (NavigationController?.NavigationBar != null)
			{
				NavigationController.NavigationBar.Translucent = true;
			}

			if (NavigationItem != null)
			{
				NavigationItem.RightBarButtonItem = _clearAllButton;
			}

			_viewModel = AppDelegate.DependencyService.Resolve<ProjectViewModel> ();

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

			if (_viewModel != null)
			{
				try
				{
					await _viewModel.Start ();

					_projectsTableView.Source = new ProjectListTableViewSource (_viewModel, ShowNewProjectDialog);
					_projectsTableView.ReloadData ();
				}
				catch (Exception e)
				{
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

					cell.Label.TextColor = UIColor.FromRGB(101, 46, 0);

					cell.SelectionStyle = UITableViewCellSelectionStyle.None;
					return cell;
				}
				else
				{
					var cell = tableView.DequeueReusableCell ("ProjectTimeTableViewCell") as ProjectTimeTableViewCell;
					var project = _viewModel.AllProjects [indexPath.Row - 1];
					cell.SelectionStyle = UITableViewCellSelectionStyle.None;

					if (project.IsRunning ())
					{
						cell.BackgroundColor = UIColor.FromRGBA (0, 50, 0, 0.2f);
					}
					else
					{
						cell.BackgroundColor = UIColor.FromRGBA (0, 0, 0, 0);
					}

					cell.UpdateCell (project);
					return cell;
				}
			}

			public override nint RowsInSection (UITableView tableview, nint section)
			{
				return 1 + _viewModel.AllProjects.Count;
			}

			public async override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				_lastSelectedRow = indexPath;

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

				tableView.BeginUpdates ();
				tableView.ReloadRows (new NSIndexPath [] { indexPath }, UITableViewRowAnimation.None);
				tableView.EndUpdates ();

				if (indexPath.Row == 0)
				{
					_addNewProjectHandler?.Invoke ();
				}
			}


		}
	}
}