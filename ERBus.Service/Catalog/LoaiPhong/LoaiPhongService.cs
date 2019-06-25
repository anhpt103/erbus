using ERBus.Entity;
using ERBus.Entity.Database.Catalog;
using ERBus.Service.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ERBus.Service.Catalog.LoaiPhong
{
    public interface ILoaiPhongService : IDataInfoService<LOAIPHONG>
    {
        string BuildCode();
        string SaveCode();
        LoaiPhongViewModel.InfoUpload UploadImageLoaiPhong();
    }
    public class LoaiPhongService : DataInfoServiceBase<LOAIPHONG>, ILoaiPhongService
    {
        public LoaiPhongService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        protected override Expression<Func<LOAIPHONG, bool>> GetKeyFilter(LOAIPHONG instance)
        {
            var unitCode = GetCurrentUnitCode();
            return x => x.MALOAIPHONG == instance.MALOAIPHONG && x.UNITCODE.Equals(unitCode);
        }

        public string BuildCode()
        {
            var unitCode = GetCurrentUnitCode();
            var type = TypeBuildCode.LOAIPHONG.ToString();
            var result = "";
            var idRepo = UnitOfWork.Repository<CAPMA>();
            var config = idRepo.DbSet.FirstOrDefault(x => x.LOAIMA == type && x.UNITCODE == unitCode);
            if (config == null)
            {
                config = new CAPMA
                {
                    ID = Guid.NewGuid().ToString(),
                    LOAIMA = type,
                    NHOMMA = "LOAIPHONG",
                    GIATRI = "0",
                    UNITCODE = unitCode
                };
            }
            var newNumber = config.GenerateNumber();
            config.GIATRI = newNumber;
            result = string.Format("{0}{1}", config.LOAIMA, newNumber);
            config.GIATRI = result;
            return result;
        }

        public string SaveCode()
        {
            var unitCode = GetCurrentUnitCode();
            var type = TypeBuildCode.LOAIPHONG.ToString();
            var result = "";
            var idRepo = UnitOfWork.Repository<CAPMA>();
            var config = idRepo.DbSet.FirstOrDefault(x => x.LOAIMA == type && x.UNITCODE == unitCode);
            if (config == null)
            {
                config = new CAPMA
                {
                    ID = Guid.NewGuid().ToString(),
                    LOAIMA = type,
                    NHOMMA = "LOAIPHONG",
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

        public LoaiPhongViewModel.InfoUpload UploadImageLoaiPhong()
        {
            LoaiPhongViewModel.InfoUpload result = new LoaiPhongViewModel.InfoUpload();
            try
            {
                string path = PhysicalPathUploadLoaiPhong();
                HttpRequest request = HttpContext.Current.Request;
                var maLoaiPhong = request.Form["MALOAIPHONG"];
                path += maLoaiPhong + "\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                try
                {
                    if (request.Files.Count > 0)
                    {
                        HttpPostedFile file = request.Files[0];
                        List<string> tmp = file.FileName.Split('.').ToList();
                        string extension = tmp.Count > 0 ? tmp[1] : "jpg";
                        string fileName = string.Format("{0}.{1}", maLoaiPhong, extension);
                        file.SaveAs(path + fileName);
                        result.FILENAME = fileName;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
