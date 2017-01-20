using System;
using TimeSheetTimer.Mobile.ClassLibrary;
using TimeSheetTimer.Mobile.Interfaces;

namespace TimeSheetTimer.Mobile.DataAccess
{
	public class ProjectTimeRecordRepository : BaseRepository<ProjectTimeRecord>, IProjectTimeRecordRepository
	{
		public ProjectTimeRecordRepository (ISqliteConnectionService context) : base(context)
		{
		}
	}
}
