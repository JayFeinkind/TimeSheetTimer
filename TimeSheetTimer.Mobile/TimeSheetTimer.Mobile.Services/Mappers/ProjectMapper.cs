using System;
using TimeSheetTimer.Mobile.ClassLibrary;

namespace TimeSheetTimer.Mobile.Services
{
	public class ProjectMapper
	{
		public void MapEntityToDto (Project entity, ProjectDto dto)
		{
			dto.IsDeleted = entity.IsDeleted;
			dto.Id = entity.Id;
			dto.Name = dto.Name;
		}

		public void MapDtoToEntity (Project entity, ProjectDto dto)
		{
			entity.IsDeleted = dto.IsDeleted;
			entity.Id = dto.Id;
			entity.Name = dto.Name;
		}

		public ProjectDto MapEntityToNewDto (Project entity)
		{
			var dto = new ProjectDto();

			dto.IsDeleted = entity.IsDeleted;
			dto.Id = entity.Id;
			dto.Name = entity.Name;

			return dto;
		}

		public Project MapDtoToNewEntity (ProjectDto dto)
		{
			var entity = new Project ();

			entity.IsDeleted = dto.IsDeleted;
			entity.Id = dto.Id;
			entity.Name = dto.Name;

			return entity;
		}
	}
}
