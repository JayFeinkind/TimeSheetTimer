using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimeSheetTimer.Mobile.ClassLibrary
{
	public class ProjectDto : Entity
	{
		public string Name { get; set; }
		public Stack<ProjectTimeRecordDto> RecordStack { get; set; } = new Stack<ProjectTimeRecordDto> ();

		public bool IsRunning ()
		{
			if (RecordStack.Count == 0)
				return false;
			
			return RecordStack.Peek ().StartUTC != null && RecordStack.Peek ().EndUTC == null;
		}

		public void Start ()
		{
			var currentRecord = new ProjectTimeRecordDto ();
			currentRecord.ProjectId = this.Id;
			currentRecord.StartUTC = DateTime.UtcNow;

			RecordStack.Push (currentRecord);
		}

		public void Stop ()
		{
			// if no records exist or the top of the stack has its end time set return and do nothing.
			//   these indicate Start() was not called first.
			if (IsRunning() == false)
				return;

			var currentRecord = RecordStack.Peek ();
			currentRecord.EndUTC = DateTime.UtcNow;
		}
	}
}
