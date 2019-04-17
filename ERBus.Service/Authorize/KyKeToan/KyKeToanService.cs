using ERBus.Entity.Database.Authorize;
using ERBus.Service.Service;
using System;
using System.Linq.Expressions;

namespace ERBus.Service.Authorize.KyKeToan
{
    public interface IKyKeToanService : IDataInfoService<KYKETOAN>
    {
    }
    public class KyKeToanService : DataInfoServiceBase<KYKETOAN>, IKyKeToanService
    {
        public KyKeToanService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        
        protected override Expression<Func<KYKETOAN, bool>> GetKeyFilter(KYKETOAN instance)
        {
            var currentUser = GetCurrentUnitCode();
            return x => x.KY == instance.KY && x.UNITCODE.Equals(currentUser);
        }
    }
}
