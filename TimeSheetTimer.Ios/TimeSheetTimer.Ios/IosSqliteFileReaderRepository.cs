using System;
using System.IO;
using SQLite;
using Foundation;
using System.Threading.Tasks;
using TimeSheetTimer.Mobile.Interfaces;

namespace TimeSheetTimer.Ios
{
	public class IosSqliteFileReaderRepository : ISqliteFileReaderRepository
	{
		private string _filePath;

		public IosSqliteFileReaderRepository ()
		{
			try
			{
				string fileName = "Database.sqlite";
				string docFolder = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
				string libFolder = Path.Combine (docFolder, "..", "Library", "Databases");
				_filePath = Path.Combine (libFolder, fileName);

				if (!Directory.Exists (libFolder))
				{
					Directory.CreateDirectory (libFolder);
				}

				Console.WriteLine (_filePath);
			}
			catch (Exception e)
			{
				
			}
		}

		#region ISqliteFileReaderRepository implementation

		public async Task CreateDB ()
		{ 
			if (!File.Exists (_filePath))
			{
				await AppDelegate.DependencyService.Resolve<ICreateSqliteService> ().CreateDb ();
			}
		}

		public async Task ResetDB ()
		{
			await AppDelegate.DependencyService.Resolve<ICreateSqliteService> ().CreateDb ();
		}


		public string FilePath
		{
			get
			{
				return _filePath;
			}
			set
			{
				this._filePath = value;
			}
		}

		#endregion
	}
}

