using ERBus.Entity.Database.Catalog;
using ERBus.Service.Service;
using System;
using System.Linq.Expressions;
namespace ERBus.Service.Catalog.CauHinhLoaiPhong
{
    public interface ICauHinhLoaiPhongService : IDataInfoService<CAUHINH_LOAIPHONG>
    {
    }
    public class CauHinhLoaiPhongService : DataInfoServiceBase<CAUHINH_LOAIPHONG>, ICauHinhLoaiPhongService
    {
        public CauHinhLoaiPhongService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        protected override Expression<Func<CAUHINH_LOAIPHONG, bool>> GetKeyFilter(CAUHINH_LOAIPHONG instance)
        {
            var unitCode = GetCurrentUnitCode();
            return x => x.MALOAIPHONG == instance.MALOAIPHONG && x.UNITCODE.Equals(unitCode);
        }
    }
}
