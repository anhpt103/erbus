using AutoMapper;
using ERBus.Entity;
using ERBus.Entity.Database.Authorize;
using ERBus.Entity.Database.Catalog;
using ERBus.Service.Service;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace ERBus.Service.Authorize.NguoiDung
{
    public interface INguoiDungService : IDataInfoService<NGUOIDUNG>
    {
        NguoiDungViewModel.Dto Login(NguoiDungViewModel.Dto model);
        NguoiDungViewModel.Dto FindUser(string username, string password);
        string BuildCode();
        string SaveCode();
    }
    public class NguoiDungService : DataInfoServiceBase<NGUOIDUNG>, INguoiDungService
    {
        public NguoiDungService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        protected override Expression<Func<NGUOIDUNG, bool>> GetKeyFilter(NGUOIDUNG instance)
        {
            return x => x.USERNAME == instance.USERNAME && x.UNITCODE.StartsWith("");
        }

        public string BuildCode()
        {
            var unitCode = GetCurrentUnitCode();
            var type = TypeBuildCode.NGUOIDUNG.ToString();
            var result = "";
            var idRepo = UnitOfWork.Repository<CAPMA>();
            var config = idRepo.DbSet.FirstOrDefault(x => x.LOAIMA == type && x.UNITCODE == unitCode);
            if (config == null)
            {
                config = new CAPMA
                {
                    ID = Guid.NewGuid().ToString(),
                    LOAIMA = type,
                    NHOMMA = "NGUOIDUNG",
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
            var type = TypeBuildCode.NGUOIDUNG.ToString();
            var result = "";
            var idRepo = UnitOfWork.Repository<CAPMA>();
            var config = idRepo.DbSet.FirstOrDefault(x => x.LOAIMA == type && x.UNITCODE == unitCode);
            if (config == null)
            {
                config = new CAPMA
                {
                    ID = Guid.NewGuid().ToString(),
                    LOAIMA = type,
                    NHOMMA = "NGUOIDUNG",
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

        public NguoiDungViewModel.Dto Login(NguoiDungViewModel.Dto model)
        {
            NguoiDungViewModel.Dto result = null;
            var user = Repository.DbSet.FirstOrDefault(x => x.USERNAME == model.USERNAME);
            if (user != null)
            {
                if (user.PASSWORD == MD5Encrypt.Encrypt(model.PASSWORD))
                {
                    result = Mapper.Map<NGUOIDUNG, NguoiDungViewModel.Dto>(user);
                }
            }
            return result;
        }
        public NguoiDungViewModel.Dto FindUser(string username, string password)
        {
            var result = new NguoiDungViewModel.Dto();
            using (var ctx = new ERBusContext())
            {
                var user = ctx.NGUOIDUNGs.FirstOrDefault(x => x.USERNAME == username && x.TRANGTHAI == 10);
                if (user != null)
                {
                    if (user.PASSWORD == MD5Encrypt.Encrypt(password))
                    {
                        result = Mapper.Map<NGUOIDUNG, NguoiDungViewModel.Dto>(user);
                        return result;
                    }
                }
                else
                {
                    result = null;
                }
            }
            return result;
        }
    }
}
