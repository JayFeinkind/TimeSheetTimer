using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheetTimer.Mobile.Interfaces
{
    public interface ICreateSqliteService
    {
        Task CreateDb();
    }
}
