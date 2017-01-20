using System;
using System.Diagnostics;

namespace TimeSheetTimer.Mobile.ClassLibrary
{
	public class ProjectTimeRecordDto
	{
		public int Id { get; set; }

		public DateTime? StartUTC { get; set; }

		public DateTime? EndUTC { get; set; }

		public long Seconds
		{
			get
			{
				if (StartUTC == null)
				{
					return 0;
				}

				if (EndUTC != null)
				{
					return EndUTC.Value.Subtract (StartUTC.Value).Ticks / 10000000;
				}
				else
				{
					return DateTime.UtcNow.Subtract (StartUTC.Value).Ticks / 10000000;
				}
			}
		}
	}
}
