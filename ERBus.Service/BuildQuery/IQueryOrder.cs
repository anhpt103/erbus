using System.Reflection;
using ERBus.Service.BuildQuery.Query.Types;

namespace ERBus.Service.BuildQuery
{
    public interface IQueryOrder
    {
        string Field { get; set; }

        PropertyInfo Property { get; set; }

        OrderMethod Method { get; set; }

        string Build();

        T ConvertTo<T>() where T : IQueryOrder;
        void MapTo<T>(T order) where T : IQueryOrder;
    }
}