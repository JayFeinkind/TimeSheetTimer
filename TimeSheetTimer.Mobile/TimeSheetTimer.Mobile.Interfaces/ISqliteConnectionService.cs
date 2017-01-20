using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheetTimer.Mobile.Interfaces
{
    public interface ISqliteConnectionService
    {
        SQLiteAsyncConnection Instance { get; }
    }
}
