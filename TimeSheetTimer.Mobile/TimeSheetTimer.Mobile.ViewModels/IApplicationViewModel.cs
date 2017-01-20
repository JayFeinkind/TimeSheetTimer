using System;
using System.Threading.Tasks;

namespace TimeSheetTimer.Mobile.ViewModels
{
	public interface IApplicationViewModel
	{
		Task Start ();
	}

	public delegate void ViewModelNavigationRequested(IApplicationViewModel viewModel);
}
