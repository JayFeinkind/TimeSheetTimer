using System;
using TimeSheetTimer.Mobile.ClassLibrary;

namespace TimeSheetTimer.Mobile.Services
{
	public class ProjectMapper
	{
		public void MapEntityToDto (Project entity, ProjectDto dto)
		{
			dto.Id = entity.Id;
			dto.Name = dto.Name;
		}
	}
}
