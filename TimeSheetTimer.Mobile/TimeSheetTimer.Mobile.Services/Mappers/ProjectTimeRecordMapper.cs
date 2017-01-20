using System;
using TimeSheetTimer.Mobile.ClassLibrary;

namespace TimeSheetTimer.Mobile.Services
{
	public class ProjectTimeRecordMapper
	{
		public void MapEntityToDto (ProjectTimeRecord entity, ProjectTimeRecordDto dto)
		{
			dto.Id = entity.Id;
			dto.StartUTC = entity.StartUTC;
			dto.EndUTC = entity.EndUTC;
		}
	}
}
