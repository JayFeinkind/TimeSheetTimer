using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeSheetTimer.Mobile.ClassLibrary;
using TimeSheetTimer.Mobile.Interfaces;

namespace TimeSheetTimer.Mobile.ViewModels
{
	public class ProjectViewModel : IApplicationViewModel
	{
		IProjectService _projectService;

		public ViewModelNavigationRequested ViewModelNavigationRequestedHandler;

		public Action<int> NewProjectSaved;

		public ProjectViewModel (IProjectService projectService)
		{
			_projectService = projectService;
		}

		public async Task Start ()
		{
			AllProjects = await _projectService.GetProjects ();
		}

		public async Task SaveTimeRecord(ProjectTimeRecordDto record)
		{
			await _projectService.CreateNewRecord(record);
		}

		public async Task SaveProject (string name)
		{
			var project = new ProjectDto ();
			project.Name = name;

			var newProject = await _projectService.CreateNewProject (project);

			AllProjects.Add (newProject);

			NewProjectSaved?.Invoke (AllProjects.IndexOf (newProject));
		}

		public List<ProjectDto> AllProjects { get; set; } = new List<ProjectDto> ();
	}
}
