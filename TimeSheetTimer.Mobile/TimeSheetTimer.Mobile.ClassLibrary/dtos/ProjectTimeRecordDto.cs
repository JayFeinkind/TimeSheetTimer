using System;
namespace TimeSheetTimer.Mobile.ClassLibrary
{
	public class ProjectTimeRecordDto
	{
		public int Id { get; set; }

		public DateTime? StartUTC { get; set; }

		public DateTime? EndUTC { get; set; }

		public long Minutes
		{
			get
			{
				if (StartUTC == null || EndUTC == null)
				{
					return 0;
				}

				return EndUTC.Value.Subtract (StartUTC.Value).Ticks;
			}
		}
	}
}
