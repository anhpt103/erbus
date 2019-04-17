using System.Collections.Generic;

namespace ERBus.Service.BuildQuery
{
    public interface IQueryFilterParam
    {
        int Count { get; set; }
        List<object> Params { get; set; }

        string GetNextParam(object param = null);
    }
}