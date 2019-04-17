using ERBus.Entity;
using ERBus.Entity.Database.Authorize;
using ERBus.Entity.Database.Catalog;
using ERBus.Service.Service;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace ERBus.Service.Catalog.ThamSoHeThong
{
    public interface IThamSoHeThongService : IDataInfoService<THAMSOHETHONG>
    {
        string BuildCode();
        string SaveCode();
    }
    public class ThamSoHeThongService : DataInfoServiceBase<THAMSOHETHONG>, IThamSoHeThongService
    {
        public ThamSoHeThongService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        protected override Expression<Func<THAMSOHETHONG, bool>> GetKeyFilter(THAMSOHETHONG instance)
        {
            var unitCode = GetCurrentUnitCode();
            return x => x.MA_THAMSO == instance.MA_THAMSO && x.UNITCODE.Equals(unitCode);
        }

        public string BuildCode()
        {
            var unitCode = GetCurrentUnitCode();
            var type = TypeBuildCode.TS.ToString();
            var result = "";
            var idRepo = UnitOfWork.Repository<CAPMA>();
            var config = idRepo.DbSet.FirstOrDefault(x => x.LOAIMA == type && x.UNITCODE == unitCode);
            if (config == null)
            {
                config = new CAPMA
                {
                    ID = Guid.NewGuid().ToString(),
                    LOAIMA = type,
                    NHOMMA = "THAMSOHETHONG",
                    GIATRI = "0",
                    UNITCODE = unitCode
                };
            }
            var newNumber = config.GenerateNumber();
            config.GIATRI = newNumber;
            result = string.Format("{0}{1}", config.LOAIMA, newNumber);
            return result;
        }

        public string SaveCode()
        {
            var unitCode = GetCurrentUnitCode();
            var type = TypeBuildCode.TS.ToString();
            var result = "";
            var idRepo = UnitOfWork.Repository<CAPMA>();
            var config = idRepo.DbSet.FirstOrDefault(x => x.LOAIMA == type && x.UNITCODE == unitCode);
            if (config == null)
            {
                config = new CAPMA
                {
                    ID = Guid.NewGuid().ToString(),
                    LOAIMA = type,
                    NHOMMA = "THAMSOHETHONG",
                    GIATRI = "0",
                    UNITCODE = unitCode
                };
                result = config.GenerateNumber();
                config.GIATRI = result;
                idRepo.Insert(config);
            }
            else
            {
                result = config.GenerateNumber();
                config.GIATRI = result;
                config.ObjectState = ObjectState.Modified;
            }
            result = string.Format("{0}{1}", config.LOAIMA, config.GIATRI);
            return result;
        }
    }
}
