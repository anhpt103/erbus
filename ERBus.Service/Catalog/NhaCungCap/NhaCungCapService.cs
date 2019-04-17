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

namespace ERBus.Service.Catalog.NhaCungCap
{
    public interface INhaCungCapService : IDataInfoService<NHACUNGCAP>
    {
        string BuildCode();
        string SaveCode();
        PagedObj<NHACUNGCAP> QueryPageNhaCungCap(string stringConnect, PagedObj<NHACUNGCAP> page, string strKey, string maDonVi);
    }
    public class NhaCungCapService : DataInfoServiceBase<NHACUNGCAP>, INhaCungCapService
    {
        public NhaCungCapService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        protected override Expression<Func<NHACUNGCAP, bool>> GetKeyFilter(NHACUNGCAP instance)
        {
            var unitCode = GetCurrentUnitCode();
            return x => x.MANHACUNGCAP == instance.MANHACUNGCAP && x.UNITCODE.Equals(unitCode);
        }

        public string BuildCode()
        {
            var unitCode = GetCurrentUnitCode();
            var type = TypeBuildCode.PP.ToString();
            var result = "";
            var idRepo = UnitOfWork.Repository<CAPMA>();
            var config = idRepo.DbSet.FirstOrDefault(x => x.LOAIMA == type && x.UNITCODE == unitCode);
            if (config == null)
            {
                config = new CAPMA
                {
                    ID = Guid.NewGuid().ToString(),
                    LOAIMA = type,
                    NHOMMA = "NHACUNGCAP",
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
            var type = TypeBuildCode.PP.ToString();
            var result = "";
            var idRepo = UnitOfWork.Repository<CAPMA>();
            var config = idRepo.DbSet.FirstOrDefault(x => x.LOAIMA == type && x.UNITCODE == unitCode);
            if (config == null)
            {
                config = new CAPMA
                {
                    ID = Guid.NewGuid().ToString(),
                    LOAIMA = type,
                    NHOMMA = "NHACUNGCAP",
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

        public PagedObj<NHACUNGCAP> QueryPageNhaCungCap(string stringConnect, PagedObj<NHACUNGCAP> page, string strKey, string maDonVi)
        {
            var TotalItem = 0;
            List<NHACUNGCAP> ListNhaCungCap = new List<NHACUNGCAP>();
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
                            command.CommandText = @"TIMKIEM_NHACUNGCAP_PAGINATION";
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
                                            NHACUNGCAP NhaCungCap = new NHACUNGCAP();
                                            if (dataReader["ID"] != null)
                                            {
                                                NhaCungCap.ID = dataReader["ID"].ToString();
                                            }
                                            if (dataReader["MANHACUNGCAP"] != null)
                                            {
                                                NhaCungCap.MANHACUNGCAP = dataReader["MANHACUNGCAP"].ToString();
                                            }
                                            if (dataReader["TENNHACUNGCAP"] != null)
                                            {
                                                NhaCungCap.TENNHACUNGCAP = dataReader["TENNHACUNGCAP"].ToString();
                                            }
                                            if (dataReader["DIACHI"] != null)
                                            {
                                                NhaCungCap.DIACHI = dataReader["DIACHI"].ToString();
                                            }
                                            if (dataReader["MASOTHUE"] != null)
                                            {
                                                NhaCungCap.MASOTHUE = dataReader["MASOTHUE"].ToString();
                                            }
                                            if (dataReader["DIENTHOAI"] != null)
                                            {
                                                NhaCungCap.DIENTHOAI = dataReader["DIENTHOAI"].ToString();
                                            }
                                            if (dataReader["DIENGIAI"] != null)
                                            {
                                                NhaCungCap.DIENGIAI = dataReader["DIENGIAI"].ToString();
                                            }
                                            if (dataReader["TRANGTHAI"] != DBNull.Value)
                                            {
                                                NhaCungCap.TRANGTHAI = int.Parse(dataReader["TRANGTHAI"].ToString());
                                            }
                                            if (dataReader["UNITCODE"] != null)
                                            {
                                                NhaCungCap.UNITCODE = dataReader["UNITCODE"].ToString();
                                            }
                                            ListNhaCungCap.Add(NhaCungCap);
                                        }
                                    }
                                }
                            }
                            dataReader.Close();
                            page.Data = ListNhaCungCap;
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
                throw new Exception("Lỗi không thể truy xuất nhà cung cấp");
            }
            return page;
        }
    }
}
