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

namespace ERBus.Service.Catalog.NhomHang
{
    public interface INhomHangService : IDataInfoService<NHOMHANG>
    {
        string BuildCode(string UnitCode);
        string SaveCode(string UnitCodeParam);
        PagedObj<NHOMHANG> QueryPageNhomHang(string stringConnect, PagedObj<NHOMHANG> page, string strKey, string maDonVi);
    }
    public class NhomHangService : DataInfoServiceBase<NHOMHANG>, INhomHangService
    {
        public NhomHangService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        protected override Expression<Func<NHOMHANG, bool>> GetKeyFilter(NHOMHANG instance)
        {
            return x => x.MANHOM == instance.MANHOM && x.UNITCODE.StartsWith(instance.UNITCODE);
        }

        public string BuildCode(string UnitCode)
        {
            var type = TypeBuildCode.N.ToString();
            var result = "";
            var idRepo = UnitOfWork.Repository<CAPMA>();
            var config = idRepo.DbSet.FirstOrDefault(x => x.LOAIMA == type && x.UNITCODE == UnitCode);
            if (config == null)
            {
                config = new CAPMA
                {
                    ID = Guid.NewGuid().ToString(),
                    LOAIMA = type,
                    NHOMMA = "NHOMHANG",
                    GIATRI = "0",
                    UNITCODE = UnitCode
                };
            }
            var newNumber = config.GenerateNumber();
            config.GIATRI = newNumber;
            result = string.Format("{0}{1}", config.LOAIMA, newNumber);
            return result;
        }

        public string SaveCode(string UnitCodeParam)
        {
            var type = TypeBuildCode.N.ToString();
            var result = "";
            var idRepo = UnitOfWork.Repository<CAPMA>();
            var config = idRepo.DbSet.FirstOrDefault(x => x.LOAIMA == type && x.UNITCODE == UnitCodeParam);
            if (config == null)
            {
                config = new CAPMA
                {
                    ID = Guid.NewGuid().ToString(),
                    LOAIMA = type,
                    NHOMMA = "NHOMHANG",
                    GIATRI = "0",
                    UNITCODE = UnitCodeParam
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

        public PagedObj<NHOMHANG> QueryPageNhomHang(string stringConnect, PagedObj<NHOMHANG> page, string strKey, string maDonVi)
        {
            var TotalItem = 0;
            List<NHOMHANG> ListNhomHang = new List<NHOMHANG>();
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
                            command.CommandText = @"TIMKIEM_NHOMHANG_PAGINATION";
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
                                            NHOMHANG NhomHang = new NHOMHANG();
                                            if (dataReader["ID"] != null)
                                            {
                                                NhomHang.ID = dataReader["ID"].ToString();
                                            }
                                            if (dataReader["MALOAI"] != null)
                                            {
                                                NhomHang.MALOAI = dataReader["MALOAI"].ToString();
                                            }
                                            if (dataReader["MANHOM"] != null)
                                            {
                                                NhomHang.MANHOM = dataReader["MANHOM"].ToString();
                                            }
                                            if (dataReader["TENNHOM"] != null)
                                            {
                                                NhomHang.TENNHOM = dataReader["TENNHOM"].ToString();
                                            }
                                            if (dataReader["TRANGTHAI"] != DBNull.Value)
                                            {
                                                NhomHang.TRANGTHAI = int.Parse(dataReader["TRANGTHAI"].ToString());
                                            }
                                            if (dataReader["UNITCODE"] != null)
                                            {
                                                NhomHang.UNITCODE = dataReader["UNITCODE"].ToString();
                                            }
                                            ListNhomHang.Add(NhomHang);
                                        }
                                    }
                                }
                            }
                            dataReader.Close();
                            page.Data = ListNhomHang;
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
                throw new Exception("Lỗi không thể truy xuất nhóm hàng");
            }
            return page;
        }

    }
}
