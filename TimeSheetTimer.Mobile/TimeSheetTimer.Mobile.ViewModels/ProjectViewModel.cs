using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeSheetTimer.Mobile.ClassLibrary;
using TimeSheetTimer.Mobile.Interfaces;

namespace TimeSheetTimer.Mobile.ViewModels
{
	public class ProjectViewModel : IApplicationViewModel
	{
		readonly IProjectService _projectService;

		public ViewModelNavigationRequested ViewModelNavigationRequestedHandler;

		public Action<int> NewProjectSaved;

		public List<ProjectDto> AllProjects { get; set; } = new List<ProjectDto>();

		public ProjectViewModel (IProjectService projectService)
		{
			_projectService = projectService;
		}

		public async Task Start()
		{
			await ReadProjects();
		}

		#region crud

		public async Task ReadProjects()
		{
			AllProjects = await _projectService.GetActiveProjects();
		}

		public async Task SaveTimeRecord(ProjectTimeRecordDto record)
		{
			var newRecord = await _projectService.CreateNewRecord(record);

			var project = AllProjects.Find(p => p.Id == record.ProjectId);

			project.RecordStack.Pop();
			project.RecordStack.Push(newRecord);
		}

		public async Task DeleteRecords(List<ProjectTimeRecordDto> records)
		{
			await _projectService.DeleteRecords(records);
			AllProjects.Find(p => p.Id == records.FirstOrDefault()?.ProjectId)?.RecordStack.Clear();
		}

		public async Task DeleteProject(ProjectDto dto)
		{
			await _projectService.DeleteProject(dto);
			AllProjects.Remove(dto);
		}

		public async Task SaveProject (string name)
		{
			var project = new ProjectDto ();
			project.Name = name;

			var newProject = await _projectService.CreateNewProject (project);

			AllProjects.Add (newProject);

			NewProjectSaved?.Invoke (AllProjects.IndexOf (newProject));
		}

		#endregion
	}
}
