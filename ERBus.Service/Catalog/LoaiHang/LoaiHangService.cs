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

namespace ERBus.Service.Catalog.LoaiHang
{
    public interface ILoaiHangService : IDataInfoService<LOAIHANG>
    {
        string BuildCode();
        string SaveCode();
        PagedObj<LOAIHANG> QueryPageLoaiHang(string stringConnect, PagedObj<LOAIHANG> page, string strKey, string maDonVi);
    }
    public class LoaiHangService : DataInfoServiceBase<LOAIHANG>, ILoaiHangService
    {
        public LoaiHangService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        protected override Expression<Func<LOAIHANG, bool>> GetKeyFilter(LOAIHANG instance)
        {
            var unitCode = GetCurrentUnitCode();
            return x => x.MALOAI == instance.MALOAI && x.UNITCODE.Equals(unitCode);
        }

        public string BuildCode()
        {
            var unitCode = GetCurrentUnitCode();
            var type = TypeBuildCode.LOAIHANG.ToString();
            var result = "";
            var idRepo = UnitOfWork.Repository<CAPMA>();
            var config = idRepo.DbSet.FirstOrDefault(x => x.LOAIMA == type && x.UNITCODE == unitCode);
            if (config == null)
            {
                config = new CAPMA
                {
                    ID = Guid.NewGuid().ToString(),
                    LOAIMA = type,
                    NHOMMA = "LOAIHANG",
                    GIATRI = ((char)64).ToString(),
                    UNITCODE = unitCode
                };
            }
            result = config.GenerateChar();
            config.GIATRI = result;
            return result;
        }

        public string SaveCode()
        {
            var unitCode = GetCurrentUnitCode();
            var type = TypeBuildCode.LOAIHANG.ToString();
            var result = "";
            var idRepo = UnitOfWork.Repository<CAPMA>();
            var config = idRepo.DbSet.FirstOrDefault(x => x.LOAIMA == type && x.UNITCODE == unitCode);
            if (config == null)
            {
                config = new CAPMA
                {
                    ID = Guid.NewGuid().ToString(),
                    LOAIMA = type,
                    NHOMMA = "LOAIHANG",
                    GIATRI = ((char)64).ToString(),
                    UNITCODE = unitCode
                };
                result = config.GenerateChar();
                config.GIATRI = result;
                idRepo.Insert(config);
            }
            else
            {
                result = config.GenerateChar();
                config.GIATRI = result;
                config.ObjectState = ObjectState.Modified;
            }
            return result;
        }
        public PagedObj<LOAIHANG> QueryPageLoaiHang(string stringConnect, PagedObj<LOAIHANG> page, string strKey, string maDonVi)
        {
            var TotalItem = 0;
            List<LOAIHANG> ListLoaiHang = new List<LOAIHANG>();
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
                            command.CommandText = @"TIMKIEM_LOAIHANG_PAGINATION";
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
                                            LOAIHANG LoaiHang = new LOAIHANG();
                                            if (dataReader["ID"] != null)
                                            {
                                                LoaiHang.ID = dataReader["ID"].ToString();
                                            }
                                            if (dataReader["MALOAI"] != null)
                                            {
                                                LoaiHang.MALOAI = dataReader["MALOAI"].ToString();
                                            }
                                            if (dataReader["TENLOAI"] != null)
                                            {
                                                LoaiHang.TENLOAI = dataReader["TENLOAI"].ToString();
                                            }
                                            if (dataReader["TRANGTHAI"] != DBNull.Value)
                                            {
                                                LoaiHang.TRANGTHAI = int.Parse(dataReader["TRANGTHAI"].ToString());
                                            }
                                            if (dataReader["UNITCODE"] != null)
                                            {
                                                LoaiHang.UNITCODE = dataReader["UNITCODE"].ToString();
                                            }
                                            ListLoaiHang.Add(LoaiHang);
                                        }
                                    }
                                }
                            }
                            dataReader.Close();
                            page.Data = ListLoaiHang;
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
                throw new Exception("Lỗi không thể truy xuất loại hàng");
            }
            return page;
        }
    }
}
