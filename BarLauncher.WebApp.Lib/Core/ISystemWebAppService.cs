using BarLauncher.EasyHelper.Core.Service;
using FluentDataAccess;

namespace BarLauncher.WebApp.Lib.Core.Service
{
    public interface ISystemWebAppService : ISystemService, IDataAccessConfigurationByPath
    {
        string GetExportPath();

        string GetUID();
    }
}