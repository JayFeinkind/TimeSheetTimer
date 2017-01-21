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
		readonly IProjectRepository _projectRepository;
		readonly IProjectTimeRecordRepository _projectTimeRepository;
		readonly ProjectMapper _projectMapper = new ProjectMapper ();
		readonly ProjectTimeRecordMapper _projectTimeMapper = new ProjectTimeRecordMapper ();

		public ProjectService (IProjectRepository projectRepository, IProjectTimeRecordRepository projectTimeRepository)
		{
			_projectTimeRepository = projectTimeRepository;
			_projectRepository = projectRepository;
		}

		public async Task<ProjectTimeRecordDto> CreateNewRecord(ProjectTimeRecordDto dto)
		{
			try
			{
				var entity = _projectTimeMapper.MapDtoToNewEntity(dto);

				await _projectTimeRepository.CreateAllEntities(new List<ProjectTimeRecord> { entity }).ConfigureAwait(false);

				return _projectTimeMapper.MapEntityToNewDto(
					await _projectTimeRepository.ReadEntityWhere(e =>
																 e.ProjectId == dto.ProjectId &&
																 e.StartUTC == dto.StartUTC && 
					                                             e.EndUTC == dto.EndUTC).ConfigureAwait(false));
			}
			catch (Exception e)
			{
			}

			return dto;
		}

		public async Task<ProjectDto> CreateNewProject (ProjectDto dto)
		{
			try
			{
				var entity = _projectMapper.MapDtoToNewEntity (dto);

				await _projectRepository.CreateAllEntities (new List<Project> { entity }).ConfigureAwait(false);

				return _projectMapper.MapEntityToNewDto (await _projectRepository.ReadEntityWhere (e => e.Name == dto.Name).ConfigureAwait (false));
			}
			catch (Exception e)
			{
				
			}

			return dto;
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
					var dto = _projectMapper.MapEntityToNewDto (project);

					var projectRecords = allRecords.FindAll (r => r.ProjectId == project.Id).Select (e =>
					{
						return _projectTimeMapper.MapEntityToNewDto (e);
					});

					dto.RecordStack = new Stack<ProjectTimeRecordDto> (projectRecords.OrderBy (r => r.StartUTC));

					projects.Add (dto);
				}
			}
			catch (Exception e)
			{
				
			}

			return projects;
		}
	}
}
