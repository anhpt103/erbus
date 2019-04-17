using ERBus.Service.BuildQuery;
using System.Collections.Generic;
namespace ERBus.Service.Service
{
    public interface IDataSearch
    {
        string DefaultOrder { get; }
        void LoadGeneralParam(string summary);
        List<IQueryFilter> GetFilters();
        
        List<IQueryFilter> GetQuickFilters();
    }
}