using ERBus.Entity;
using ERBus.Entity.Database.Authorize;
using ERBus.Entity.Database.Catalog;
using ERBus.Service.Service;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace ERBus.Service.Authorize.CuaHang
{
    public interface ICuaHangService : IDataInfoService<CUAHANG>
    {
        string BuildCode();
        string SaveCode();
    }
    public class CuaHangService : DataInfoServiceBase<CUAHANG>, ICuaHangService
    {
        public CuaHangService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        public string BuildCode()
        {
            var unitCode = GetCurrentUnitCode();
            var type = TypeBuildCode.CUAHANG.ToString();
            var result = "";
            var idRepo = UnitOfWork.Repository<CAPMA>();
            var config = idRepo.DbSet.FirstOrDefault(x => x.LOAIMA == type && x.UNITCODE == unitCode);
            if (config == null)
            {
                config = new CAPMA
                {
                    ID = Guid.NewGuid().ToString(),
                    LOAIMA = type,
                    NHOMMA = "CUAHANG",
                    GIATRI = "0",
                    UNITCODE = unitCode
                };
            }
            var newNumber = config.GenerateNumber();
            config.GIATRI = newNumber;
            result = string.Format("{0}{1}", "0", newNumber);
            return result;
        }

        public string SaveCode()
        {
            var unitCode = GetCurrentUnitCode();
            var type = TypeBuildCode.CUAHANG.ToString();
            var result = "";
            var idRepo = UnitOfWork.Repository<CAPMA>();
            var config = idRepo.DbSet.FirstOrDefault(x => x.LOAIMA == type && x.UNITCODE == unitCode);
            if (config == null)
            {
                config = new CAPMA
                {
                    ID = Guid.NewGuid().ToString(),
                    LOAIMA = type,
                    NHOMMA = "CUAHANG",
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
            result = string.Format("{0}{1}", "0", config.GIATRI);
            return result;
        }
        protected override Expression<Func<CUAHANG, bool>> GetKeyFilter(CUAHANG instance)
        {
            string unitCode = GetCurrentUnitCode();
            return x => x.MA_CUAHANG == instance.MA_CUAHANG && x.UNITCODE.Equals(unitCode);
        }
    }
}
