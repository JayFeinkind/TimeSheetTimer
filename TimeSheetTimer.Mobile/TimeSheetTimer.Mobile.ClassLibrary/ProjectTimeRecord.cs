using System;
namespace TimeSheetTimer.Mobile.ClassLibrary
{
	public class ProjectTimeRecord : Entity
	{
		public DateTime StartUTC { get; set; }
		public DateTime EndUTC { get; set; }
		public int ProjectId { get; set; }
	}
}
