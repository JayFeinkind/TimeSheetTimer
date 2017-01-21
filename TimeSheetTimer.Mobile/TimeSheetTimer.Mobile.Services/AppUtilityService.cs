using System;
using System.Text;

namespace TimeSheetTimer.Mobile.Services
{
	public static class AppUtilityService
	{
		public static string FormatedTotal(long seconds)
		{
			if (seconds == 0)
			{
				return "0s ";
			}

			long hours = seconds / 3600;

			seconds -= hours * 3600;

			long minutes = seconds / 60;

			seconds -= minutes * 60;

			var sb = new StringBuilder();

			if (hours > 0)
			{
				sb.Append(hours + "h ");
			}

			if (minutes > 0)
			{
				sb.Append(minutes + "m ");
			}

			if (seconds > 0)
			{
				sb.Append(seconds + "s");
			}

			return sb.ToString();
		}
	}
}
