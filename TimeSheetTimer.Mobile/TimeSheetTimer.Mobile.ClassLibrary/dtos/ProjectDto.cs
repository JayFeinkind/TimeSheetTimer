using System;
using System.Collections.Generic;

namespace TimeSheetTimer.Mobile.ClassLibrary
{
	public class ProjectDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public Stack<ProjectTimeRecordDto> RecordStack { get; set; } = new Stack<ProjectTimeRecordDto> ();

		public void Start ()
		{
			var currentRecord = new ProjectTimeRecordDto ();
			currentRecord.StartUTC = DateTime.UtcNow;

			RecordStack.Push (currentRecord);
		}

		public void Stop ()
		{
			// if no records exist or the top of the stack has its end time set return and do nothing
			if (RecordStack.Count == 0 || RecordStack.Peek()?.EndUTC != null)
				return;

			var currentRecord = RecordStack.Peek ();
			currentRecord.EndUTC = DateTime.UtcNow;
		}
	}
}
