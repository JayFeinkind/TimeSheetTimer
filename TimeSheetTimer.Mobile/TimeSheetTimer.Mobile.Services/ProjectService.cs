using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeSheetTimer.Mobile.ClassLibrary;
using TimeSheetTimer.Mobile.Interfaces;

namespace TimeSheetTimer.Mobile.Services
{
	public class ProjectService : IProjectService
	{
		IProjectRepository _projectRepository;
		IProjectTimeRecordRepository _projectTimeRepository;
		ProjectMapper _projectMapper = new ProjectMapper ();
		ProjectTimeRecordMapper _projectTimeMapper = new ProjectTimeRecordMapper();

		public ProjectService (IProjectRepository projectRepository, IProjectTimeRecordRepository projectTimeRepository)
		{
			_projectTimeRepository = projectTimeRepository;
			_projectRepository = projectRepository;
		}

		public async Task<List<ProjectDto>> GetProjects ()
		{
			var projects = new List<ProjectDto> ();

			try
			{
				var allProjects = await _projectRepository.ReadAllEntities ().ConfigureAwait (false);
				var allRecords = await _projectTimeRepository.ReadAllEntities ().ConfigureAwait (false);

				foreach (var project in allProjects)
				{
					var dto = new ProjectDto();
					_projectMapper.MapEntityToDto (project, dto);

					var projectRecords = allRecords.FindAll (r => r.ProjectId == project.Id).Select (e =>
					{
						var record = new ProjectTimeRecordDto ();
						_projectTimeMapper.MapEntityToDto (e, record);
						return record;
					});

					dto.RecordStack = new Stack<ProjectTimeRecordDto> (projectRecords.OrderByDescending (r => r.StartUTC));
				}
			}
			catch (Exception e)
			{
				
			}

			return projects;
		}
	}
}
