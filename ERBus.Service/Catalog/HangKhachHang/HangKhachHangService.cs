using ERBus.Entity;
using ERBus.Entity.Database.Catalog;
using ERBus.Service.Service;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace ERBus.Service.Catalog.HangKhachHang
{
    public interface IHangKhachHangService : IDataInfoService<HANGKHACHHANG>
    {
        string BuildCode();
        string SaveCode();
        bool CheckExistsFirstRate();
    }
    public class HangKhachHangService : DataInfoServiceBase<HANGKHACHHANG>, IHangKhachHangService
    {
        public HangKhachHangService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        protected override Expression<Func<HANGKHACHHANG, bool>> GetKeyFilter(HANGKHACHHANG instance)
        {
            var unitCode = GetCurrentUnitCode();
            return x => x.MAHANG == instance.MAHANG && x.UNITCODE.Equals(unitCode);
        }

        public string BuildCode()
        {
            var unitCode = GetCurrentUnitCode();
            var type = TypeBuildCode.H.ToString();
            var result = "";
            var idRepo = UnitOfWork.Repository<CAPMA>();
            var config = idRepo.DbSet.FirstOrDefault(x => x.LOAIMA == type && x.UNITCODE == unitCode);
            if (config == null)
            {
                config = new CAPMA
                {
                    ID = Guid.NewGuid().ToString(),
                    LOAIMA = type,
                    NHOMMA = "RATE",
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
            var type = TypeBuildCode.H.ToString();
            var result = "";
            var idRepo = UnitOfWork.Repository<CAPMA>();
            var config = idRepo.DbSet.FirstOrDefault(x => x.LOAIMA == type && x.UNITCODE == unitCode);
            if (config == null)
            {
                config = new CAPMA
                {
                    ID = Guid.NewGuid().ToString(),
                    LOAIMA = type,
                    NHOMMA = "RATE",
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

        public bool CheckExistsFirstRate()
        {
            var unitCode = GetCurrentUnitCode();
            var result = false;
            var exists = UnitOfWork.Repository<HANGKHACHHANG>().DbSet.FirstOrDefault(x=>x.HANG_KHOIDAU == 1 && x.UNITCODE.Equals(unitCode));
            if (exists != null) result = true;
            return result;
        }

    }
}
