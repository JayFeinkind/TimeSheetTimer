﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeSheetTimer.Mobile.ClassLibrary;

namespace TimeSheetTimer.Mobile.Interfaces
{
	public interface IProjectService
	{
		Task<List<ProjectDto>> GetProjects ();
		Task<ProjectDto> CreateNewProject (ProjectDto dto);
	}
}
