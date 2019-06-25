using ERBus.Entity;
using ERBus.Entity.Database.Catalog;
using ERBus.Service.Service;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace ERBus.Service.Catalog.Phong
{
    public interface IPhongService : IDataInfoService<PHONG>
    {
        string BuildCode(string maTang);
        string SaveCode(string maTang);
        DateTime? GetNullableDateTime(OracleDataReader reader, string columnName);
        List<PhongViewModel.StatusRoom> GetListStatusRoom(string unitCode, string stringConnect);
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
        public DateTime? GetNullableDateTime(OracleDataReader reader, string columnName)
        {
            var col = reader.GetOrdinal(columnName);
            return reader.IsDBNull(col) ?
                        (DateTime?)null :
                        (DateTime?)reader.GetDateTime(col);
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

        public List<PhongViewModel.StatusRoom> GetListStatusRoom(string unitCode, string stringConnect)
        {
            List<PhongViewModel.StatusRoom> ListRoom = new List<PhongViewModel.StatusRoom>();
            using (OracleConnection connection = new OracleConnection(stringConnect))
            {
                try
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        OracleCommand command = new OracleCommand();
                        command.Connection = connection;
                        command.CommandType = CommandType.Text;
                        command.CommandText = @"SELECT c.ID,c.MAPHONG,c.TENPHONG,c.TANG,c.VITRI,c.TRANGTHAI_DATPHONG,c.MALOAIPHONG,c.NGAY_DATPHONG,c.THOIGIAN_DATPHONG,c.MA_DATPHONG,D.BACKGROUND,D.ICON FROM (SELECT a.ID,a.MAPHONG,a.TENPHONG,a.TANG,a.VITRI,B.TRANGTHAI AS TRANGTHAI_DATPHONG,A.MALOAIPHONG,b.NGAY_DATPHONG,b.THOIGIAN_DATPHONG,b.MA_DATPHONG,a.UNITCODE FROM PHONG a LEFT JOIN DATPHONG b ON A.MAPHONG = B.MAPHONG AND a.UNITCODE = b.UNITCODE) c INNER JOIN LOAIPHONG d ON c.MALOAIPHONG = d.MALOAIPHONG AND c.UNITCODE = d.UNITCODE AND c.UNITCODE = '" + unitCode + "'";
                        OracleDataReader dataReader = command.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                PhongViewModel.StatusRoom ViewModel = new PhongViewModel.StatusRoom();
                                if (dataReader["ID"] != null)
                                {
                                    ViewModel.ID = dataReader["ID"].ToString();
                                }
                                if (dataReader["MAPHONG"] != null)
                                {
                                    ViewModel.MAPHONG = dataReader["MAPHONG"].ToString();
                                }
                                if (dataReader["TENPHONG"] != null)
                                {
                                    ViewModel.TENPHONG = dataReader["TENPHONG"].ToString();
                                }
                                if (dataReader["TANG"] != DBNull.Value)
                                {
                                    int TANG = 0;
                                    int.TryParse(dataReader["TANG"].ToString(), out TANG);
                                    ViewModel.TANG = TANG;
                                }
                                if (dataReader["VITRI"] != null)
                                {
                                    ViewModel.VITRI = dataReader["VITRI"].ToString();
                                }
                                if (dataReader["TRANGTHAI_DATPHONG"] != DBNull.Value)
                                {
                                    int TRANGTHAI_DATPHONG = 0;
                                    int.TryParse(dataReader["TRANGTHAI_DATPHONG"].ToString(), out TRANGTHAI_DATPHONG);
                                    ViewModel.TRANGTHAI_DATPHONG = TRANGTHAI_DATPHONG;
                                }
                                if (dataReader["MALOAIPHONG"] != null)
                                {
                                    ViewModel.MALOAIPHONG = dataReader["MALOAIPHONG"].ToString();
                                }
                                if (dataReader["THOIGIAN_DATPHONG"] != null)
                                {
                                    ViewModel.THOIGIAN_DATPHONG = dataReader["THOIGIAN_DATPHONG"].ToString();
                                }
                                if (dataReader["NGAY_DATPHONG"] != null)
                                {
                                    ViewModel.NGAY_DATPHONG = GetNullableDateTime(dataReader, "NGAY_DATPHONG") ;
                                }
                                if (dataReader["MA_DATPHONG"] != null)
                                {
                                    ViewModel.MA_DATPHONG = dataReader["MA_DATPHONG"].ToString();
                                }
                                if (dataReader["BACKGROUND"] != null)
                                {
                                    ViewModel.BACKGROUND = (byte[])dataReader["BACKGROUND"];
                                }
                                if (dataReader["ICON"] != null)
                                {
                                    ViewModel.ICON = (byte[])dataReader["ICON"];
                                }
                                ListRoom.Add(ViewModel);
                            }
                        }
                        dataReader.Close();
                    }
                }
                catch
                {
                    ListRoom = null;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
            return ListRoom;
        }
    }
}
