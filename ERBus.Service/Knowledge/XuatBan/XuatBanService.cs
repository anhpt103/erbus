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

namespace ERBus.Service.Knowledge.XuatBan
{
    public interface IXuatBanService : IDataInfoService<CHUNGTU>
    {
        string BuildCode();
        string SaveCode();
        CHUNGTU InsertChungTu(XuatBanViewModel.Dto instance);
        CHUNGTU UpdateChungTu(XuatBanViewModel.Dto instance);
        bool DeleteChungTu(string ID);
        CHUNGTU Approval(string ID, string StringConnection, CHUNGTU ChungTu, string UnitCode);
        PagedObj<CHUNGTU> QueryPageChungTu(string stringConnect, PagedObj<CHUNGTU> page, string strKey, string maDonVi);
        List<XuatBanViewModel.DtoDetails> GetDataDetails(string MaChungTu, string stringConnect, string TableName, string MaKhoXuat);
    }
    public class XuatBanService : DataInfoServiceBase<CHUNGTU>, IXuatBanService
    {
        public XuatBanService(IUnitOfWork unitOfWork) : base(unitOfWork)
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
            var type = TypeBuildCode.XBAN.ToString();
            var result = "";
            var idRepo = UnitOfWork.Repository<CAPMA>();
            var config = idRepo.DbSet.FirstOrDefault(x => x.LOAIMA == type && x.UNITCODE == unitCode);
            if (config == null)
            {
                config = new CAPMA
                {
                    ID = Guid.NewGuid().ToString(),
                    LOAIMA = type,
                    NHOMMA = "XBAN_BUON",
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
            var type = TypeBuildCode.XBAN.ToString();
            var result = "";
            var idRepo = UnitOfWork.Repository<CAPMA>();
            var config = idRepo.DbSet.FirstOrDefault(x => x.LOAIMA == type && x.UNITCODE == unitCode);
            if (config == null)
            {
                config = new CAPMA
                {
                    ID = Guid.NewGuid().ToString(),
                    LOAIMA = type,
                    NHOMMA = "XBAN_BUON",
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
                            command.Parameters.Add(@"P_LOAI_CHUNGTU", OracleDbType.NVarchar2, 50).Value = TypeBuildCode.XBAN.ToString();
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
                throw new Exception("Lỗi không thể truy xuất hàng hóa");
            }
            return page;
        }

        public CHUNGTU InsertChungTu(XuatBanViewModel.Dto instance)
        {
            var dataChungTu = Mapper.Map<XuatBanViewModel.Dto, CHUNGTU>(instance);
            dataChungTu.ID = Guid.NewGuid().ToString();
            dataChungTu.LOAI_CHUNGTU = "XBAN";
            dataChungTu.THOIGIAN_TAO = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
            dataChungTu.MADONVI_NHAP = GetCurrentUnitCode();
            dataChungTu.MAKHACHHANG = instance.MAKHACHHANG;
            dataChungTu.MANHACUNGCAP = instance.MAKHACHHANG;
            dataChungTu.I_STATE = "C";
            if (dataChungTu.TRANGTHAI == (int)TypeState.APPROVAL)
            {
                dataChungTu.NGAY_DUYETPHIEU = DateTime.Now;
                dataChungTu.THOIGIAN_DUYET = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
                //chạy giảm tồn
            }
            var result = AddUnit(dataChungTu);
            var dataDetails = Mapper.Map<List<XuatBanViewModel.DtoDetails>, List<CHUNGTU_CHITIET>>(instance.DataDetails);
            dataDetails.ForEach(x =>
            {
                x.ID = Guid.NewGuid().ToString();
                x.MA_CHUNGTU = result.MA_CHUNGTU;
                x.GIAMUA = 0;
                x.GIAMUA_VAT = 0;
            });
            result = Insert(result);
            UnitOfWork.Repository<CHUNGTU_CHITIET>().InsertRange(dataDetails);
            return result;
        }

        public CHUNGTU UpdateChungTu(XuatBanViewModel.Dto instance)
        {
            var dataChungTu = Mapper.Map<XuatBanViewModel.Dto, CHUNGTU>(instance);
            dataChungTu.I_STATE = "U";
            if (dataChungTu.TRANGTHAI == (int)TypeState.APPROVAL)
            {
                dataChungTu.NGAY_DUYETPHIEU = DateTime.Now;
                dataChungTu.THOIGIAN_DUYET = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
                //chạy giảm tồn
            }
            dataChungTu.UNITCODE = GetCurrentUnitCode();
            dataChungTu.MAKHACHHANG = instance.MAKHACHHANG;
            dataChungTu.MANHACUNGCAP = instance.MAKHACHHANG;
            var listProduct = UnitOfWork.Repository<CHUNGTU_CHITIET>().DbSet.Where(x => x.MA_CHUNGTU == dataChungTu.MA_CHUNGTU).ToList();
            if (listProduct.Count > 0) listProduct.ForEach(x => x.ObjectState = ObjectState.Deleted);
            var dataDetails = Mapper.Map<List<XuatBanViewModel.DtoDetails>, List<CHUNGTU_CHITIET>>(instance.DataDetails);
            dataDetails.ForEach(x =>
            {
                x.ID = Guid.NewGuid().ToString();
                x.MA_CHUNGTU = dataChungTu.MA_CHUNGTU;
                x.GIAMUA = 0;
                x.GIAMUA_VAT = 0;
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
                if(chungTuChiTiet.Count > 0)
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
                if (XuatNhapTon_GiamPhieu(tableName, period.NAM, period.KY, ID, StringConnection))
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

        public List<XuatBanViewModel.DtoDetails> GetDataDetails(string MaChungTu, string stringConnect, string TableName, string MaKhoXuat)
        {
            List<XuatBanViewModel.DtoDetails> result = new List<XuatBanViewModel.DtoDetails>();
            if (!string.IsNullOrEmpty(MaChungTu))
            {
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
                                command.CommandType = CommandType.Text;
                                command.CommandText = @"SELECT a.MA_CHUNGTU,a.MAHANG,b.TENHANG, a.MATHUE_VAO,a.MATHUE_RA,a.SOLUONG,a.GIAMUA,a.GIAMUA_VAT,a.TIEN_GIAMGIA,a.THANHTIEN,a.""INDEX"",a.GIABANLE,a.GIABANLE_VAT,b.MADONVITINH,c.TYLE_LAILE,xnt.GIAVON,xnt.TONCUOIKYSL
                                FROM CHUNGTU_CHITIET a INNER JOIN MATHANG b ON a.MAHANG = b.MAHANG INNER JOIN MATHANG_GIA c ON a.MAHANG = c.MAHANG INNER JOIN DONVITINH d ON b.MADONVITINH = d.MADONVITINH INNER JOIN " + TableName + " xnt ON a.MAHANG = xnt.MAHANG AND xnt.MAKHO = '"+ MaKhoXuat + "' AND a.MA_CHUNGTU = '"+ MaChungTu + "' ";
                                OracleDataReader dataReader = command.ExecuteReader();
                                if (dataReader.HasRows)
                                {
                                    while (dataReader.Read())
                                    {
                                        XuatBanViewModel.DtoDetails row = new XuatBanViewModel.DtoDetails();
                                        if (dataReader["MA_CHUNGTU"] != null)
                                        {
                                            row.MA_CHUNGTU = dataReader["MA_CHUNGTU"].ToString();
                                        }
                                        if (dataReader["MAHANG"] != null)
                                        {
                                            row.MAHANG = dataReader["MAHANG"].ToString();
                                        }

                                        if (dataReader["TENHANG"] != null)
                                        {
                                            row.TENHANG = dataReader["TENHANG"].ToString();
                                        }

                                        if (dataReader["MATHUE_VAO"] != null)
                                        {
                                            row.MATHUE_VAO = dataReader["MATHUE_VAO"].ToString();
                                        }
                                        if (dataReader["MATHUE_RA"] != null)
                                        {
                                            row.MATHUE_RA = dataReader["MATHUE_RA"].ToString();
                                        }

                                        decimal SOLUONG = 0;
                                        if (dataReader["SOLUONG"] != null)
                                        {
                                            decimal.TryParse(dataReader["SOLUONG"].ToString(), out SOLUONG);
                                        }
                                        row.SOLUONG = SOLUONG;

                                        decimal GIAMUA = 0;
                                        if (dataReader["GIAMUA"] != null)
                                        {
                                            decimal.TryParse(dataReader["GIAMUA"].ToString(), out GIAMUA);
                                        }
                                        row.GIAMUA = GIAMUA;

                                        decimal GIAMUA_VAT = 0;
                                        if (dataReader["GIAMUA_VAT"] != null)
                                        {
                                            decimal.TryParse(dataReader["GIAMUA_VAT"].ToString(), out GIAMUA_VAT);
                                        }
                                        row.GIAMUA_VAT = GIAMUA_VAT;

                                        decimal TIEN_GIAMGIA = 0;
                                        if (dataReader["TIEN_GIAMGIA"] != null)
                                        {
                                            decimal.TryParse(dataReader["TIEN_GIAMGIA"].ToString(), out TIEN_GIAMGIA);
                                        }
                                        row.TIEN_GIAMGIA = TIEN_GIAMGIA;

                                        decimal THANHTIEN = 0;
                                        if (dataReader["THANHTIEN"] != null)
                                        {
                                            decimal.TryParse(dataReader["THANHTIEN"].ToString(), out THANHTIEN);
                                        }
                                        row.THANHTIEN = THANHTIEN;

                                        int INDEX = 0;
                                        if (dataReader["INDEX"] != null)
                                        {
                                            int.TryParse(dataReader["INDEX"].ToString(), out INDEX);
                                        }
                                        row.INDEX = INDEX;

                                        decimal GIABANLE = 0;
                                        if (dataReader["GIABANLE"] != null)
                                        {
                                            decimal.TryParse(dataReader["GIABANLE"].ToString(), out GIABANLE);
                                        }
                                        row.GIABANLE = GIABANLE;

                                        decimal GIABANLE_VAT = 0;
                                        if (dataReader["GIABANLE_VAT"] != null)
                                        {
                                            decimal.TryParse(dataReader["GIABANLE_VAT"].ToString(), out GIABANLE_VAT);
                                        }
                                        row.GIABANLE_VAT = GIABANLE_VAT;

                                        decimal TYLE_LAILE = 0;
                                        if (dataReader["TYLE_LAILE"] != null)
                                        {
                                            decimal.TryParse(dataReader["TYLE_LAILE"].ToString(), out TYLE_LAILE);
                                        }
                                        row.TYLE_LAILE = TYLE_LAILE;

                                        decimal GIAVON = 0;
                                        if (dataReader["GIAVON"] != null)
                                        {
                                            decimal.TryParse(dataReader["GIAVON"].ToString(), out GIAVON);
                                        }
                                        row.GIAVON = GIAVON;

                                        decimal TONCUOIKYSL = 0;
                                        if (dataReader["TONCUOIKYSL"] != null)
                                        {
                                            decimal.TryParse(dataReader["TONCUOIKYSL"].ToString(), out TONCUOIKYSL);
                                        }
                                        row.TONCUOIKYSL = TONCUOIKYSL;

                                        if (dataReader["MADONVITINH"] != null)
                                        {
                                            row.MADONVITINH = dataReader["MADONVITINH"].ToString();
                                        }
                                        result.Add(row);
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
                }
                catch
                {
                    throw new Exception("Lỗi không thể truy xuất hàng hóa");
                }
            }
            return result;
        }
    }
}
