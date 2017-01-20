using System.Threading.Tasks;

namespace TimeSheetTimer.Mobile.Interfaces
{
	public interface ISqliteFileReaderRepository
    {
		Task ResetDB ();
		Task CreateDB ();
        string FilePath { get; set; }
    }
}
