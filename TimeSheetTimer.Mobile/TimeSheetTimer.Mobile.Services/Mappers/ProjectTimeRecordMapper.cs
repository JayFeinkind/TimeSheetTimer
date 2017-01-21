using System;
using TimeSheetTimer.Mobile.ClassLibrary;

namespace TimeSheetTimer.Mobile.Services
{
	public class ProjectTimeRecordMapper
	{
		public void MapEntityToDto (ProjectTimeRecord entity, ProjectTimeRecordDto dto)
		{
			dto.Id = entity.Id;
			dto.IsDeleted = entity.IsDeleted;
			dto.ProjectId = entity.ProjectId;
			dto.StartUTC = entity.StartUTC;
			dto.EndUTC = entity.EndUTC;
		}

		public void MapDtoToEntity (ProjectTimeRecord entity, ProjectTimeRecordDto dto)
		{
			entity.Id = dto.Id;
			entity.IsDeleted = dto.IsDeleted;
			entity.ProjectId = dto.ProjectId;
			entity.StartUTC = dto.StartUTC;
			entity.EndUTC = dto.EndUTC;
		}

		public ProjectTimeRecordDto MapEntityToNewDto (ProjectTimeRecord entity)
		{
			var dto = new ProjectTimeRecordDto ();

			dto.IsDeleted = entity.IsDeleted;
			dto.ProjectId = entity.ProjectId;
			dto.Id = entity.Id;
			dto.StartUTC = entity.StartUTC;
			dto.EndUTC = entity.EndUTC;

			return dto;
		}

		public ProjectTimeRecord MapDtoToNewEntity (ProjectTimeRecordDto dto)
		{
			var entity = new ProjectTimeRecord ();

			entity.IsDeleted = dto.IsDeleted;
			entity.ProjectId = dto.ProjectId;
			entity.Id = dto.Id;
			entity.StartUTC = dto.StartUTC;
			entity.EndUTC = dto.EndUTC;

			return entity;
		}
	}
}
