using AutoMapper;
using ERBus.Entity;
using ERBus.Entity.Database.Catalog;
using ERBus.Entity.Database.Knowledge;
using ERBus.Service.BuildQuery;
using ERBus.Service.Service;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace ERBus.Service.Knowledge.NhapMua
{
    public interface INhapMuaService : IDataInfoService<CHUNGTU>
    {
        string BuildCode();
        string SaveCode();
        CHUNGTU InsertChungTu(NhapMuaViewModel.Dto instance);
        CHUNGTU UpdateChungTu(NhapMuaViewModel.Dto instance);
        int UpdateMatHangThayDoiGia(string ID, string unitCode, string stringConnect);
        bool DeleteChungTu(string ID);
        CHUNGTU Approval(string ID, string StringConnection, CHUNGTU ChungTu, string UnitCode);
        PagedObj<CHUNGTU> QueryPageChungTu(string stringConnect, PagedObj<CHUNGTU> page, string strKey, string maDonVi);
        List<NhapMuaViewModel.PrintItemDto> GetPrintItemDetails(string ID, string stringConnect);
    }
    public class NhapMuaService : DataInfoServiceBase<CHUNGTU>, INhapMuaService
    {
        public NhapMuaService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        protected override Expression<Func<CHUNGTU, bool>> GetKeyFilter(CHUNGTU instance)
        {
            var unitCode = GetCurrentUnitCode();
            return x => x.MA_CHUNGTU == instance.MA_CHUNGTU && x.UNITCODE.Equals(unitCode);
        }

        public string BuildCode()
        {
            var unitCode = GetCurrentUnitCode();
            var type = TypeBuildCode.NMUA.ToString();
            var result = "";
            var idRepo = UnitOfWork.Repository<CAPMA>();
            var config = idRepo.DbSet.FirstOrDefault(x => x.LOAIMA == type && x.UNITCODE == unitCode);
            if (config == null)
            {
                config = new CAPMA
                {
                    ID = Guid.NewGuid().ToString(),
                    LOAIMA = type,
                    NHOMMA = "NHAPMUA",
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
            var type = TypeBuildCode.NMUA.ToString();
            var result = "";
            var idRepo = UnitOfWork.Repository<CAPMA>();
            var config = idRepo.DbSet.FirstOrDefault(x => x.LOAIMA == type && x.UNITCODE == unitCode);
            if (config == null)
            {
                config = new CAPMA
                {
                    ID = Guid.NewGuid().ToString(),
                    LOAIMA = type,
                    NHOMMA = "NHAPMUA",
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

        public PagedObj<CHUNGTU> QueryPageChungTu(string stringConnect, PagedObj<CHUNGTU> page, string strKey, string maDonVi)
        {
            var TotalItem = 0;
            List<CHUNGTU> ListChungTu = new List<CHUNGTU>();
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
                            command.CommandText = @"TIMKIEM_CHUNGTU_PAGINATION";
                            command.Parameters.Add(@"P_MADONVI", OracleDbType.NVarchar2, 50).Value = maDonVi;
                            command.Parameters.Add(@"P_TUKHOA", OracleDbType.NVarchar2, 500).Value = strKey.ToString().ToUpper().Trim();
                            command.Parameters.Add(@"P_PAGENUMBER", OracleDbType.Int32).Value = page.CurrentPage;
                            command.Parameters.Add(@"P_PAGESIZE", OracleDbType.Int32).Value = page.ItemsPerPage;
                            command.Parameters.Add(@"P_LOAI_CHUNGTU", OracleDbType.NVarchar2, 50).Value = TypeBuildCode.NMUA.ToString();
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
                                            CHUNGTU ChungTu = new CHUNGTU();
                                            if (dataReader["ID"] != null)
                                            {
                                                ChungTu.ID = dataReader["ID"].ToString();
                                            }
                                            if (dataReader["MA_CHUNGTU"] != null)
                                            {
                                                ChungTu.MA_CHUNGTU = dataReader["MA_CHUNGTU"].ToString();
                                            }
                                            if (dataReader["NGAY_CHUNGTU"] != DBNull.Value)
                                            {
                                                ChungTu.NGAY_CHUNGTU = DateTime.Parse(dataReader["NGAY_CHUNGTU"].ToString());
                                            }
                                            if (dataReader["NGAY_DUYETPHIEU"] != DBNull.Value)
                                            {
                                                ChungTu.NGAY_DUYETPHIEU = DateTime.Parse(dataReader["NGAY_DUYETPHIEU"].ToString());
                                            }
                                            if (dataReader["MAKHO_NHAP"] != null)
                                            {
                                                ChungTu.MAKHO_NHAP = dataReader["MAKHO_NHAP"].ToString();
                                            }
                                            if (dataReader["MAKHO_XUAT"] != null)
                                            {
                                                ChungTu.MAKHO_XUAT = dataReader["MAKHO_XUAT"].ToString();
                                            }
                                            if (dataReader["TRANGTHAI"] != null)
                                            {
                                                int TRANGTHAI = 0;
                                                int.TryParse(dataReader["TRANGTHAI"].ToString(), out TRANGTHAI);
                                                ChungTu.TRANGTHAI = TRANGTHAI;
                                            }
                                            if (dataReader["MANHACUNGCAP"] != null)
                                            {
                                                ChungTu.MANHACUNGCAP = dataReader["MANHACUNGCAP"].ToString();
                                            }
                                            if (dataReader["MAKHACHHANG"] != null)
                                            {
                                                ChungTu.MAKHACHHANG = dataReader["MAKHACHHANG"].ToString();
                                            }
                                            ListChungTu.Add(ChungTu);
                                        }
                                    }
                                }
                            }
                            dataReader.Close();
                            page.Data = ListChungTu;
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
                throw new Exception("Lỗi không thể truy xuất phiếu nhập mua");
            }
            return page;
        }

        public CHUNGTU InsertChungTu(NhapMuaViewModel.Dto instance)
        {
            var dataChungTu = Mapper.Map<NhapMuaViewModel.Dto, CHUNGTU>(instance);
            dataChungTu.ID = Guid.NewGuid().ToString();
            dataChungTu.LOAI_CHUNGTU = "NMUA";
            dataChungTu.THOIGIAN_TAO = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
            dataChungTu.MADONVI_NHAP = GetCurrentUnitCode();
            dataChungTu.MAKHACHHANG = instance.MANHACUNGCAP;
            dataChungTu.I_STATE = "C";
            if (dataChungTu.TRANGTHAI == (int)TypeState.APPROVAL)
            {
                dataChungTu.NGAY_DUYETPHIEU = DateTime.Now;
                dataChungTu.THOIGIAN_DUYET = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
                //chạy tăng tồn
            }
            var result = AddUnit(dataChungTu);
            var dataDetails = Mapper.Map<List<NhapMuaViewModel.DtoDetails>, List<CHUNGTU_CHITIET>>(instance.DataDetails);
            dataDetails.ForEach(x =>
            {
                x.ID = Guid.NewGuid().ToString();
                x.MA_CHUNGTU = result.MA_CHUNGTU;
                x.GIABANLE = 0;
                x.GIABANLE_VAT = 0;
            });
            result = Insert(result);
            UnitOfWork.Repository<CHUNGTU_CHITIET>().InsertRange(dataDetails);
            return result;
        }

        public CHUNGTU UpdateChungTu(NhapMuaViewModel.Dto instance)
        {
            var dataChungTu = Mapper.Map<NhapMuaViewModel.Dto, CHUNGTU>(instance);
            dataChungTu.I_STATE = "U";
            if (dataChungTu.TRANGTHAI == (int)TypeState.APPROVAL)
            {
                dataChungTu.NGAY_DUYETPHIEU = DateTime.Now;
                dataChungTu.THOIGIAN_DUYET = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
                //chạy tăng tồn
            }
            dataChungTu.UNITCODE = GetCurrentUnitCode();
            dataChungTu.MAKHACHHANG = instance.MANHACUNGCAP;
            var listProduct = UnitOfWork.Repository<CHUNGTU_CHITIET>().DbSet.Where(x => x.MA_CHUNGTU == dataChungTu.MA_CHUNGTU).ToList();
            if (listProduct.Count > 0) listProduct.ForEach(x => x.ObjectState = ObjectState.Deleted);
            var dataDetails = Mapper.Map<List<NhapMuaViewModel.DtoDetails>, List<CHUNGTU_CHITIET>>(instance.DataDetails);
            dataDetails.ForEach(x =>
            {
                x.ID = Guid.NewGuid().ToString();
                x.MA_CHUNGTU = dataChungTu.MA_CHUNGTU;
                x.GIABANLE = 0;
                x.GIABANLE_VAT = 0;
            });
            UnitOfWork.Repository<CHUNGTU_CHITIET>().InsertRange(dataDetails);
            var result = Update(dataChungTu);
            return result;
        }

        public bool DeleteChungTu(string ID)
        {
            bool result = true;
            var chungTu = UnitOfWork.Repository<CHUNGTU>().DbSet.FirstOrDefault(x => x.ID.Equals(ID));
            if (chungTu == null)
            {
                result = false;
            }
            else
            {
                var chungTuChiTiet = UnitOfWork.Repository<CHUNGTU_CHITIET>().DbSet.Where(x => x.MA_CHUNGTU == chungTu.MA_CHUNGTU).ToList();
                if (chungTuChiTiet.Count > 0)
                {
                    foreach (var row in chungTuChiTiet)
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

        public CHUNGTU Approval(string ID, string StringConnection, CHUNGTU ChungTu, string UnitCode)
        {
            var period = GetLastestPeriod(UnitCode, StringConnection);
            if (period != null && period.KY > 0 && period.NAM > 0)
            {
                var tableName = Get_TableName_XNT(period.NAM, period.KY);
                if (XuatNhapTon_TangPhieu(tableName, period.NAM, period.KY, ID, StringConnection))
                {
                    ChungTu.TRANGTHAI = (int)TypeState.APPROVAL;
                    ChungTu.NGAY_DUYETPHIEU = period.NGAYKETOAN;
                    ChungTu.THOIGIAN_DUYET = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
                    ChungTu.ObjectState = ObjectState.Modified;
                }
                else
                {
                    ChungTu = null;
                }

            }
            else
            {
                ChungTu = null;
            }
            return ChungTu;
        }

        public int UpdateMatHangThayDoiGia(string ID, string unitCode, string stringConnect)
        {
            var result = 0;
            using (OracleConnection connection = new OracleConnection(stringConnect))
            {
                try
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        OracleTransaction transaction;
                        OracleCommand command = new OracleCommand();
                        command.Connection = connection;
                        transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
                        command.Transaction = transaction;
                        try
                        {
                            if (!string.IsNullOrEmpty(ID))
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                command.CommandText = @"UPDATE_GIA_SAUDUYET_NHAPMUA";
                                command.Parameters.Add(@"P_MADONVI", OracleDbType.NVarchar2, 50).Value = unitCode;
                                command.Parameters.Add(@"P_ID", OracleDbType.NVarchar2, 500).Value = ID.Trim();
                                command.Parameters.Add(@"P_RESULT_UPDATE", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                                OracleDataReader dataReader = command.ExecuteReader();
                                if (dataReader.HasRows)
                                {
                                    while (dataReader.Read())
                                    {
                                        if (dataReader["RESULT_UPDATE"] != null)
                                        {
                                            int.TryParse(dataReader["RESULT_UPDATE"].ToString(), out result);
                                        }
                                    }
                                }
                                dataReader.Close();
                            }
                            if (result > 0) transaction.Commit();
                            else transaction.Rollback();
                        }
                        catch
                        {
                            result = 0;
                        }
                        finally
                        {
                            transaction.Dispose();
                        }
                    }
                }
                catch
                {
                    result = 0;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
            return result;
        }

        public List<NhapMuaViewModel.PrintItemDto> GetPrintItemDetails(string ID, string stringConnect)
        {
            List<NhapMuaViewModel.PrintItemDto> result = new List<NhapMuaViewModel.PrintItemDto>();
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
                        command.CommandText = @"SELECT
                                    chungtu_chitiet.MAHANG,
                                    mathang.TENHANG,
                                    mathang.BARCODE,
                                    mathang_gia.GIABANBUON_VAT,
                                    mathang_gia.GIABANLE_VAT,
                                    chungtu.MANHACUNGCAP,
                                    chungtu_chitiet.SOLUONG
                                FROM
                                    CHUNGTU chungtu
                                INNER JOIN CHUNGTU_CHITIET chungtu_chitiet
                                ON CHUNGTU.MA_CHUNGTU = chungtu_chitiet.MA_CHUNGTU
                                INNER JOIN MATHANG mathang ON mathang.MAHANG = chungtu_chitiet.MAHANG
                                INNER JOIN MATHANG_GIA mathang_gia ON mathang.MAHANG = mathang_gia.MAHANG
                                WHERE chungtu.ID = :ID ";
                        command.Parameters.Add(@"ID", OracleDbType.NVarchar2, 50).Value = ID;
                        OracleDataReader dataReader = command.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                NhapMuaViewModel.PrintItemDto printItem = new NhapMuaViewModel.PrintItemDto();
                                if (dataReader["MAHANG"] != null)
                                {
                                    printItem.MAHANG = dataReader["MAHANG"].ToString();
                                }
                                if (dataReader["TENHANG"] != null)
                                {
                                    printItem.TENHANG = dataReader["TENHANG"].ToString();
                                }
                                if (dataReader["BARCODE"] != DBNull.Value)
                                {
                                    printItem.BARCODE = dataReader["BARCODE"].ToString();
                                }
                                if (dataReader["GIABANBUON_VAT"] != DBNull.Value)
                                {
                                    printItem.GIABANBUON_VAT = decimal.Parse(dataReader["GIABANBUON_VAT"].ToString());
                                }
                                if (dataReader["GIABANLE_VAT"] != DBNull.Value)
                                {
                                    printItem.GIABANLE_VAT = decimal.Parse(dataReader["GIABANLE_VAT"].ToString());
                                }
                                if (dataReader["MANHACUNGCAP"] != null)
                                {
                                    printItem.MANHACUNGCAP = dataReader["MANHACUNGCAP"].ToString();
                                }
                                if (dataReader["SOLUONG"] != null)
                                {
                                    printItem.SOLUONG = decimal.Parse(dataReader["SOLUONG"].ToString());
                                }
                                result.Add(printItem);
                            }
                        }
                        dataReader.Close();
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
            return result;
        }
    }
}
