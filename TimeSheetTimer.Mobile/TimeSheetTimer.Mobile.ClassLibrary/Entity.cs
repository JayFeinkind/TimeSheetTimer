using System;
using SQLite;

namespace TimeSheetTimer.Mobile.ClassLibrary
{
	public class Entity
	{
		[PrimaryKey, AutoIncrement, Column ("Id")]
		public int Id { get; set; }
	}
}
