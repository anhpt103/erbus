using ERBus.Entity;
using ERBus.Entity.Database.Catalog;
using ERBus.Entity.Database.Knowledge;
using ERBus.Service.Service;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace ERBus.Service.Knowledge.DatPhong
{
    public interface IDatPhongService : IDataInfoService<DATPHONG>
    {
        string BuildCode();
        string SaveCode();
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

        public string BuildCode()
        {
            var unitCode = GetCurrentUnitCode();
            var type = TypeBuildCode.DATPHONG.ToString();
            var result = "";
            var idRepo = UnitOfWork.Repository<CAPMA_THEONGAY>();
            DateTime nowDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            DateTime tomorrowDay = DateTime.Now.AddDays(1);
            var config = idRepo.DbSet.FirstOrDefault(x => x.LOAIMA == type && x.NHOMMA == type && x.NGAY_SINHMA >= nowDay && x.NGAY_SINHMA < tomorrowDay && x.UNITCODE == unitCode);
            if (config == null)
            {
                config = new CAPMA_THEONGAY
                {
                    ID = Guid.NewGuid().ToString(),
                    LOAIMA = type,
                    NHOMMA = type,
                    NGAY_SINHMA = DateTime.Now,
                    GIATRI = "0",
                    UNITCODE = unitCode
                };
            }
            var newNumber = config.GenerateNumber();
            config.GIATRI = newNumber;
            result = string.Format("{0}{1}{2}_{3}{4}", DateTime.Now.Day.ToString("D2"), DateTime.Now.Month.ToString("D2"), DateTime.Now.Year, config.LOAIMA, newNumber);
            return result;
        }

        public string SaveCode()
        {
            var unitCode = GetCurrentUnitCode();
            var type = TypeBuildCode.DATPHONG.ToString();
            var result = "";
            var idRepo = UnitOfWork.Repository<CAPMA_THEONGAY>();
            DateTime nowDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            DateTime tomorrowDay = DateTime.Now.AddDays(1);
            var config = idRepo.DbSet.FirstOrDefault(x => x.LOAIMA == type && x.NHOMMA == type && x.NGAY_SINHMA >= nowDay && x.NGAY_SINHMA < tomorrowDay && x.UNITCODE == unitCode);
            if (config == null)
            {
                config = new CAPMA_THEONGAY
                {
                    ID = Guid.NewGuid().ToString(),
                    LOAIMA = type,
                    NHOMMA = type,
                    NGAY_SINHMA = DateTime.Now,
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
            result = string.Format("{0}{1}{2}_{3}{4}", DateTime.Now.Day.ToString("D2"), DateTime.Now.Month.ToString("D2"), DateTime.Now.Year, config.LOAIMA, config.GIATRI);
            return result;
        }
    }
}
