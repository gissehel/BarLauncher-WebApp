using BarLauncher.EasyHelper.Core.Service;
using FluentDataAccess;

namespace BarLauncher.WebApp.Lib.Core.Service
{
    public interface IDataAccessWebAppService : IDataAccessConfigurationByPath
    {
        string GetExportPath();

        string GetUID();
    }
}