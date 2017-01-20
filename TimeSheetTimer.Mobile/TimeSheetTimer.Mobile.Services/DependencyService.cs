using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSheetTimer.Mobile.DataAccess;
using TimeSheetTimer.Mobile.Interfaces;

namespace TimeSheetTimer.Mobile.Services
{
    public class DependencyService : IDependencyService
    {
        private static IUnityContainer _dependencyContainer;

        public DependencyService()
        {
            _dependencyContainer = new UnityContainer();
            RegisterDependencies();
        }
        
        public T Resolve<T>()
        {
            return _dependencyContainer.Resolve<T>();
        }

        public void RegisterDependencies()
        {
            _dependencyContainer.RegisterType<ICreateSqliteService, CreateSqliteService>();
        }

        public void RegisterInstance<T>(T implementation)
        {
            _dependencyContainer.RegisterInstance<T>(implementation);
        }

        public void RegisterType<T, T1>() where T1 : T
        {
            _dependencyContainer.RegisterType<T, T1>();
        }
    }
}
