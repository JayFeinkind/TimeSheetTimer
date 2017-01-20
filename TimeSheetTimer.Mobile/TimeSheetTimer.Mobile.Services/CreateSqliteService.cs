using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSheetTimer.Mobile.Interfaces;

namespace TimeSheetTimer.Mobile.Services
{
    public class CreateSqliteService : ICreateSqliteService
    {
        SQLiteAsyncConnection _context;

        public CreateSqliteService(ISqliteConnectionService connection)
        {
            _context = connection.Instance;
        }

        private async Task ClearDB()
        {
            var getTableNames = await _context.QueryAsync<StringWrapper>("SELECT name as TableName FROM sqlite_master WHERE type='table' AND name <> 'sqlite_sequence'");

            foreach (var table in getTableNames)
            {
                await _context.ExecuteAsync("drop table " + table.TableName);
            }

            getTableNames = await _context.QueryAsync<StringWrapper>("SELECT name as TableName FROM sqlite_master WHERE type='view'");

            foreach (var table in getTableNames)
            {
                await _context.ExecuteAsync("drop view " + table.TableName);
            }
        }

        public async Task CreateDb()
        {
            await ClearDB();


        }
    }

    public class StringWrapper
    {
        public string TableName { get; set; }
    }
}