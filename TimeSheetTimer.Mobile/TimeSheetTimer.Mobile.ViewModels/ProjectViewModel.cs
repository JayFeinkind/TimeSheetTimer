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

		public ProjectViewModel (IProjectService projectService)
		{
			_projectService = projectService;
		}

		public async Task Start ()
		{
			AllProjects = await _projectService.GetProjects ();
		}

		public List<ProjectDto> AllProjects { get; set; }
	}
}
