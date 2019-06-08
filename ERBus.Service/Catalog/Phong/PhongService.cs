using ERBus.Entity;
using ERBus.Entity.Database.Catalog;
using ERBus.Service.Service;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace ERBus.Service.Catalog.Phong
{
    public interface IPhongService : IDataInfoService<PHONG>
    {
        string BuildCode(string maTang);
        string SaveCode(string maTang);
    }
    public class PhongService : DataInfoServiceBase<PHONG>, IPhongService
    {
        public PhongService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        protected override Expression<Func<PHONG, bool>> GetKeyFilter(PHONG instance)
        {
            var unitCode = GetCurrentUnitCode();
            return x => x.MAPHONG == instance.MAPHONG && x.UNITCODE.Equals(unitCode);
        }

        public string BuildCode(string maTang)
        {
            var unitCode = GetCurrentUnitCode();
            var type = TypeBuildCode.PHONG.ToString();
            var result = "";
            var idRepo = UnitOfWork.Repository<CAPMA>();
            var config = idRepo.DbSet.FirstOrDefault(x => x.LOAIMA == type && x.NHOMMA == maTang && x.UNITCODE == unitCode);
            if (config == null)
            {
                config = new CAPMA
                {
                    ID = Guid.NewGuid().ToString(),
                    LOAIMA = type,
                    NHOMMA = maTang,
                    GIATRI = "0",
                    UNITCODE = unitCode
                };
            }
            var newNumber = config.GenerateNumber();
            config.GIATRI = newNumber;
            result = string.Format("{0}_{1}_{2}_{3}", "TANG", config.NHOMMA, config.LOAIMA, newNumber);
            return result;
        }

        public string SaveCode(string maTang)
        {
            var unitCode = GetCurrentUnitCode();
            var type = TypeBuildCode.PHONG.ToString();
            var result = "";
            var idRepo = UnitOfWork.Repository<CAPMA>();
            var config = idRepo.DbSet.FirstOrDefault(x => x.LOAIMA == type && x.NHOMMA == maTang && x.UNITCODE == unitCode);
            if (config == null)
            {
                config = new CAPMA
                {
                    ID = Guid.NewGuid().ToString(),
                    LOAIMA = type,
                    NHOMMA = maTang,
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
            result = string.Format("{0}_{1}_{2}_{3}", "TANG", config.NHOMMA, config.LOAIMA, config.GIATRI);
            return result;
        }

    }
}
