using ERBus.Entity;
using ERBus.Entity.Database.Catalog;
using ERBus.Service.BuildQuery;
using ERBus.Service.Service;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace ERBus.Service.Catalog.KhoHang
{
    public interface IKhoHangService : IDataInfoService<KHOHANG>
    {
        string BuildCode();
        string SaveCode();
        PagedObj<KHOHANG> QueryPageKhoHang(string stringConnect, PagedObj<KHOHANG> page, string strKey, string maDonVi);
    }
    public class KhoHangService : DataInfoServiceBase<KHOHANG>, IKhoHangService
    {
        public KhoHangService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        protected override Expression<Func<KHOHANG, bool>> GetKeyFilter(KHOHANG instance)
        {
            var unitCode = GetCurrentUnitCode();
            return x => x.MAKHO == instance.MAKHO && x.UNITCODE.Equals(unitCode);
        }

        public string BuildCode()
        {
            var unitCode = GetCurrentUnitCode();
            var type = TypeBuildCode.KH.ToString();
            var result = "";
            var idRepo = UnitOfWork.Repository<CAPMA>();
            var config = idRepo.DbSet.FirstOrDefault(x => x.LOAIMA == type && x.UNITCODE == unitCode);
            if (config == null)
            {
                config = new CAPMA
                {
                    ID = Guid.NewGuid().ToString(),
                    LOAIMA = type,
                    NHOMMA = "KHOHANG",
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
            var type = TypeBuildCode.KH.ToString();
            var result = "";
            var idRepo = UnitOfWork.Repository<CAPMA>();
            var config = idRepo.DbSet.FirstOrDefault(x => x.LOAIMA == type && x.UNITCODE == unitCode);
            if (config == null)
            {
                config = new CAPMA
                {
                    ID = Guid.NewGuid().ToString(),
                    LOAIMA = type,
                    NHOMMA = "KHOHANG",
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

        public PagedObj<KHOHANG> QueryPageKhoHang(string stringConnect, PagedObj<KHOHANG> page, string strKey, string maDonVi)
        {
            var TotalItem = 0;
            List<KHOHANG> ListMatHang = new List<KHOHANG>();
            try
            {
                using (OracleConnection connection = new OracleConnection(stringConnect))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            OracleCommand command = new OracleCommand();
                            command.Connection = connection;
                            command.CommandType = CommandType.StoredProcedure;
                            command.CommandText = @"TIMKIEM_KHOHANG_PAGINATION";
                            command.Parameters.Add(@"P_MADONVI", OracleDbType.NVarchar2, 50).Value = maDonVi;
                            command.Parameters.Add(@"P_TUKHOA", OracleDbType.NVarchar2, 500).Value = strKey.ToString().ToUpper().Trim();
                            command.Parameters.Add(@"P_PAGENUMBER", OracleDbType.Int32).Value = page.CurrentPage;
                            command.Parameters.Add(@"P_PAGESIZE", OracleDbType.Int32).Value = page.ItemsPerPage;
                            command.Parameters.Add(@"P_TOTALITEM", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                            command.Parameters.Add(@"CURSOR_RESULT", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                            OracleDataReader dataReader = command.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    if (dataReader["TOTAL_ITEM"] != null)
                                    {
                                        int.TryParse(dataReader["TOTAL_ITEM"].ToString(), out TotalItem);
                                    }
                                }

                                if (dataReader.NextResult())
                                {
                                    if (dataReader.HasRows)
                                    {
                                        while (dataReader.Read())
                                        {
                                            KHOHANG MatHang = new KHOHANG();
                                            if (dataReader["ID"] != null)
                                            {
                                                MatHang.ID = dataReader["ID"].ToString();
                                            }
                                            if (dataReader["MAKHO"] != null)
                                            {
                                                MatHang.MAKHO = dataReader["MAKHO"].ToString();
                                            }
                                            if (dataReader["TENKHO"] != null)
                                            {
                                                MatHang.TENKHO = dataReader["TENKHO"].ToString();
                                            }
                                            if (dataReader["DIENGIAI"] != null)
                                            {
                                                MatHang.DIENGIAI = dataReader["DIENGIAI"].ToString();
                                            }
                                            if (dataReader["UNITCODE"] != null)
                                            {
                                                MatHang.UNITCODE = dataReader["UNITCODE"].ToString();
                                            }
                                            if (dataReader["TRANGTHAI"] != DBNull.Value)
                                            {
                                                MatHang.TRANGTHAI = int.Parse(dataReader["TRANGTHAI"].ToString());
                                            }
                                            ListMatHang.Add(MatHang);
                                        }
                                    }
                                }
                            }
                            dataReader.Close();
                            page.Data = ListMatHang;
                            page.TotalItems = TotalItem;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    finally
                    {
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }
            catch
            {
                throw new Exception("Lỗi không thể truy xuất kho hàng");
            }
            return page;
        }

    }
}
