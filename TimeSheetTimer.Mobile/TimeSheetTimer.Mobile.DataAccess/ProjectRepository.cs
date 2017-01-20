using System;
using TimeSheetTimer.Mobile.ClassLibrary;
using TimeSheetTimer.Mobile.Interfaces;

namespace TimeSheetTimer.Mobile.DataAccess
{
	public class ProjectRepository : BaseRepository<Project>, IProjectRepository
	{
		public ProjectRepository (ISqliteConnectionService context) : base(context)
		{
		}
	}
}
