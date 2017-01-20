using System;
using SQLite;
using TimeSheetTimer.Mobile.Interfaces;

namespace TimeSheetTimer.Mobile.Services
{
	public class SqliteConnectionService : ISqliteConnectionService
	{
		private SQLiteAsyncConnection _instance;

		public SqliteConnectionService (ISqliteFileReaderRepository fileRepository)
		{
			_instance = new SQLiteAsyncConnection (fileRepository.FilePath);
		}

		public SQLiteAsyncConnection Instance { get { return _instance; } }
	}
}
