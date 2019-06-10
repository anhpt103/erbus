using AutoMapper;
using ERBus.Entity;
using ERBus.Entity.Database.Catalog;
using ERBus.Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ERBus.Service.Catalog.BoHang
{
    public interface IBoHangService : IDataInfoService<BOHANG>
    {
        string BuildCode();
        string SaveCode();
        BOHANG InsertBohang(BoHangViewModel.Dto instance);
        BOHANG UpdateBohang(BoHangViewModel.Dto instance);
        bool DeleteBoHang(string ID);
    }
    public class BoHangService : DataInfoServiceBase<BOHANG>, IBoHangService
    {
        public BoHangService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        protected override Expression<Func<BOHANG, bool>> GetKeyFilter(BOHANG instance)
        {
            var unitCode = GetCurrentUnitCode();
            return x => x.MABOHANG == instance.MABOHANG && x.UNITCODE.Equals(unitCode);
        }

        public string BuildCode()
        {
            var unitCode = GetCurrentUnitCode();
            var type = TypeBuildCode.BH.ToString();
            var result = "";
            var idRepo = UnitOfWork.Repository<CAPMA>();
            var config = idRepo.DbSet.FirstOrDefault(x => x.LOAIMA == type && x.UNITCODE == unitCode);
            if (config == null)
            {
                config = new CAPMA
                {
                    ID = Guid.NewGuid().ToString(),
                    LOAIMA = type,
                    NHOMMA = "BOHANG",
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
            var type = TypeBuildCode.BH.ToString();
            var result = "";
            var idRepo = UnitOfWork.Repository<CAPMA>();
            var config = idRepo.DbSet.FirstOrDefault(x => x.LOAIMA == type && x.UNITCODE == unitCode);
            if (config == null)
            {
                config = new CAPMA
                {
                    ID = Guid.NewGuid().ToString(),
                    LOAIMA = type,
                    NHOMMA = "BOHANG",
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

        public BOHANG InsertBohang(BoHangViewModel.Dto instance)
        {
            var dataBoHang = Mapper.Map<BoHangViewModel.Dto, BOHANG>(instance);
            dataBoHang.ID = Guid.NewGuid().ToString();
            dataBoHang.I_STATE = "C";
            var result = AddUnit(dataBoHang);
            var dataDetails = Mapper.Map<List<BoHangViewModel.DataDetails>, List<BOHANG_CHITIET>>(instance.DataDetails);
            result = Insert(result);
            dataDetails.ForEach(x =>
            {
                x.ID = Guid.NewGuid().ToString();
                x.MABOHANG = result.MABOHANG;
                x.I_CREATE_DATE = DateTime.Now;
                x.I_CREATE_BY = result.I_CREATE_BY;
                x.I_STATE = result.I_STATE;
                x.UNITCODE = result.UNITCODE;
            });
            UnitOfWork.Repository<BOHANG_CHITIET>().InsertRange(dataDetails);
            return result;
        }

        public BOHANG UpdateBohang(BoHangViewModel.Dto instance)
        {
            var dataBoHang = Mapper.Map<BoHangViewModel.Dto, BOHANG>(instance);
            dataBoHang.I_STATE = "U";
            var listBoHangChiTiet = UnitOfWork.Repository<BOHANG_CHITIET>().DbSet.Where(x => x.MABOHANG == dataBoHang.MABOHANG).ToList();
            if (listBoHangChiTiet.Count > 0) listBoHangChiTiet.ForEach(x => x.ObjectState = ObjectState.Deleted);
            var dataDetails = Mapper.Map<List<BoHangViewModel.DataDetails>, List<BOHANG_CHITIET>>(instance.DataDetails);
            dataDetails.ForEach(x =>
            {
                x.ID = Guid.NewGuid().ToString();
                x.MABOHANG = dataBoHang.MABOHANG;
                x.I_CREATE_DATE = DateTime.Now;
                x.I_CREATE_BY = dataBoHang.I_CREATE_BY;
                x.I_STATE = dataBoHang.I_STATE;
                x.UNITCODE = dataBoHang.UNITCODE;
            });
            UnitOfWork.Repository<BOHANG_CHITIET>().InsertRange(dataDetails);
            var result = Update(dataBoHang);
            return result;
        }

        public bool DeleteBoHang(string ID)
        {
            bool result = true;
            var chungTu = UnitOfWork.Repository<BOHANG>().DbSet.FirstOrDefault(x => x.ID.Equals(ID));
            if (chungTu == null)
            {
                result = false;
            }
            else
            {
                var boHangChiTiet = UnitOfWork.Repository<BOHANG_CHITIET>().DbSet.Where(x => x.MABOHANG == chungTu.MABOHANG).ToList();
                if (boHangChiTiet.Count > 0)
                {
                    foreach (var row in boHangChiTiet)
                    {
                        row.ObjectState = ObjectState.Deleted;
                    }
                    Delete(chungTu.ID);
                }
                else
                {
                    result = false;
                }
            }
            return result;
        }

    }
}
