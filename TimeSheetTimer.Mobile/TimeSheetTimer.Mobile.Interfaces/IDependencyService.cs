using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheetTimer.Mobile.Interfaces
{
    public interface IDependencyService
    {
        T Resolve<T>();
        void RegisterInstance<T>(T implementation);
        void RegisterType<T, T1>() where T1 : T;
    }
}
