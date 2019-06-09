using ERBus.Entity.Database.Knowledge;
using ERBus.Service.Service;
using System;
using System.Linq.Expressions;

namespace ERBus.Service.Knowledge.DatPhong
{
    public interface IDatPhongService : IDataInfoService<DATPHONG>
    {
    }
    public class DatPhongService : DataInfoServiceBase<DATPHONG>, IDatPhongService
    {
        public DatPhongService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        protected override Expression<Func<DATPHONG, bool>> GetKeyFilter(DATPHONG instance)
        {
            var unitCode = GetCurrentUnitCode();
            return x => x.MA_DATPHONG == instance.MA_DATPHONG && x.UNITCODE.Equals(unitCode);
        }
    }
}
