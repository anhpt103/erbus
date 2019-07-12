using AutoMapper;
using ERBus.Entity.Database.Knowledge;
using ERBus.Service.Knowledge.DatPhong;
using ERBus.Service.Service;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ERBus.Service.Knowledge.ThanhToanDatPhong
{
    public interface IThanhToanDatPhongService : IDataInfoService<THANHTOAN_DATPHONG>
    {
        ThanhToanDatPhongViewModel.Dto GetMerchandiseInBundleGoods(string unitCode, string stringConnect, DatPhongViewModel.DatPhongPayDto data);
        THANHTOAN_DATPHONG InsertThanhToan(ThanhToanDatPhongViewModel.Dto instance);
        bool UpdateTrangThaiDatPhong(THANHTOAN_DATPHONG instance);
        bool Approval(string ID, string StringConnection, string UnitCode);
    }
    public class ThanhToanDatPhongService : DataInfoServiceBase<THANHTOAN_DATPHONG>, IThanhToanDatPhongService
    {
        public ThanhToanDatPhongService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public bool Approval(string ID, string StringConnection, string UnitCode)
        {
            if (string.IsNullOrEmpty(ID)) return false;
            var result = false;
            var period = GetLastestPeriod(UnitCode, StringConnection);
            if (period != null && period.KY > 0 && period.NAM > 0)
            {
                var TableName = Get_TableName_XNT(period.NAM, period.KY);
                using (OracleConnection connection = new OracleConnection(StringConnection))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            OracleTransaction transaction;
                            OracleCommand command = new OracleCommand();
                            command.Connection = connection;
                            // Start a local transaction
                            transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
                            // Assign transaction object for a pending local transaction
                            command.Transaction = transaction;
                            try
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                command.CommandText = "ERBUS.XUATNHAPTON.XNT_THANHTOAN_DATPHONG";
                                command.Parameters.Add("P_TABLENAME", OracleDbType.Varchar2, 50, TableName, ParameterDirection.Input);
                                command.Parameters.Add("P_NAM", OracleDbType.Int32, period.NAM, ParameterDirection.Input);
                                command.Parameters.Add("P_KY", OracleDbType.Int32, period.KY, ParameterDirection.Input);
                                command.Parameters.Add("P_ID", OracleDbType.Varchar2, 50, ID, ParameterDirection.Input);
                                command.ExecuteNonQuery();
                                transaction.Commit();
                                result = true;
                            }
                            catch
                            {
                                transaction.Rollback();
                                result = false;
                            }
                        }
                    }
                    catch
                    {
                        result = false;
                    }
                    finally
                    {
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }
            return result;
        }

        public ThanhToanDatPhongViewModel.Dto GetMerchandiseInBundleGoods(string unitCode, string stringConnect, DatPhongViewModel.DatPhongPayDto data)
        {
            ThanhToanDatPhongViewModel.Dto thanhToanDto = new ThanhToanDatPhongViewModel.Dto();
            List<ThanhToanDatPhongViewModel.DtoDetail> listDetails = new List<ThanhToanDatPhongViewModel.DtoDetail>();
            using (OracleConnection connection = new OracleConnection(stringConnect))
            {
                if (data != null)
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            OracleCommand command = new OracleCommand();
                            command.Connection = connection;
                            command.CommandType = CommandType.Text;
                            command.CommandText = @"SELECT a.MABOHANG, c.MAHANG, f.TENHANG, g.TENDONVITINH AS DONVITINH, C.CHIETKHAU, CASE WHEN C.CHIETKHAU <= 100 THEN ROUND(C.SOLUONG * (E.GIABANLE_VAT - (E.GIABANLE_VAT * C.CHIETKHAU / 100)), 2) ELSE ROUND(C.SOLUONG * (E.GIABANLE_VAT - C.CHIETKHAU), 2) END AS GIABANLE_VAT, f.MATHUE_RA FROM LOAIPHONG a INNER JOIN BOHANG b ON a.MABOHANG = b.MABOHANG INNER JOIN BOHANG_CHITIET c ON B.MABOHANG = c.MABOHANG INNER JOIN PHONG d ON D.MALOAIPHONG = A.MALOAIPHONG INNER JOIN MATHANG_GIA e ON C.MAHANG = E.MAHANG INNER JOIN MATHANG f ON e.MAHANG = f.MAHANG INNER JOIN DONVITINH g ON f.MADONVITINH = g.MADONVITINH AND e.UNITCODE = f.UNITCODE AND f.UNITCODE = g.UNITCODE AND a.UNITCODE = b.UNITCODE AND b.UNITCODE = c.UNITCODE AND a.UNITCODE = d.UNITCODE AND c.UNITCODE = e.UNITCODE AND D.MAPHONG = '" + data.MAPHONG + "' AND a.UNITCODE = '" + unitCode + "' ";
                            OracleDataReader dataReader = command.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    ThanhToanDatPhongViewModel.DtoDetail detail = new ThanhToanDatPhongViewModel.DtoDetail();
                                    if (dataReader["MABOHANG"] != null)
                                    {
                                        detail.MABOHANG = dataReader["MABOHANG"].ToString();
                                    }
                                    if (dataReader["MAHANG"] != null)
                                    {
                                        detail.MAHANG = dataReader["MAHANG"].ToString();
                                    }
                                    if (dataReader["TENHANG"] != null)
                                    {
                                        detail.TENHANG = dataReader["TENHANG"].ToString();
                                    }
                                    if (dataReader["DONVITINH"] != null)
                                    {
                                        detail.DONVITINH = dataReader["DONVITINH"].ToString();
                                    }
                                    detail.SOLUONG = 1;
                                    if (dataReader["GIABANLE_VAT"] != null)
                                    {
                                        decimal GIABANLE_VAT = 0;
                                        decimal.TryParse(dataReader["GIABANLE_VAT"].ToString(), out GIABANLE_VAT);
                                        detail.GIABANLE_VAT = GIABANLE_VAT;
                                    }
                                    if (dataReader["CHIETKHAU"] != null)
                                    {
                                        decimal CHIETKHAU = 0;
                                        decimal.TryParse(dataReader["CHIETKHAU"].ToString(), out CHIETKHAU);
                                        detail.CHIETKHAU = CHIETKHAU;
                                    }
                                    if (dataReader["MATHUE_RA"] != null)
                                    {
                                        detail.MATHUE_RA = dataReader["MATHUE_RA"].ToString();
                                    }
                                    listDetails.Add(detail);
                                }
                            }
                            dataReader.Close();
                            command.CommandType = CommandType.Text;
                            command.CommandText = @"SELECT C.MALOAIPHONG, D.MAHANG, D.MAHANG_DICHVU, NVL(D.SOPHUT,0) AS DONVI_THOIGIAN_TINHTIEN FROM (SELECT A.MALOAIPHONG,A.UNITCODE FROM LOAIPHONG A INNER JOIN PHONG B ON A.MALOAIPHONG = B.MALOAIPHONG AND A.UNITCODE = B.UNITCODE AND B.MAPHONG = '" + data.MAPHONG + "' AND A.UNITCODE = '" + unitCode + "') C LEFT JOIN CAUHINH_LOAIPHONG D ON C.MALOAIPHONG = D.MALOAIPHONG AND C.UNITCODE = D.UNITCODE ";
                            OracleDataReader dataReaderCauHinh = command.ExecuteReader();
                            if (dataReaderCauHinh.HasRows)
                            {
                                while (dataReaderCauHinh.Read())
                                {
                                    if (dataReaderCauHinh["MAHANG"] != null)
                                    {
                                        thanhToanDto.MAHANG = dataReaderCauHinh["MAHANG"].ToString();
                                    }
                                    if (dataReaderCauHinh["MAHANG_DICHVU"] != null)
                                    {
                                        thanhToanDto.MAHANG_DICHVU = dataReaderCauHinh["MAHANG_DICHVU"].ToString();
                                    }
                                    if (dataReaderCauHinh["DONVI_THOIGIAN_TINHTIEN"] != null)
                                    {
                                        int DONVI_THOIGIAN_TINHTIEN = 0;
                                        int.TryParse(dataReaderCauHinh["DONVI_THOIGIAN_TINHTIEN"].ToString(), out DONVI_THOIGIAN_TINHTIEN);
                                        thanhToanDto.DONVI_THOIGIAN_TINHTIEN = DONVI_THOIGIAN_TINHTIEN;
                                    }
                                }
                                dataReaderCauHinh.Close();
                            }
                            thanhToanDto.MA_DATPHONG = data.MA_DATPHONG;
                            thanhToanDto.MAPHONG = data.MAPHONG;
                            thanhToanDto.MAKHO = data.MAKHO;
                            thanhToanDto.NGAY_DATPHONG = data.NGAY_DATPHONG;
                            thanhToanDto.THOIGIAN_DATPHONG = data.THOIGIAN_DATPHONG;
                            thanhToanDto.TEN_KHACHHANG = data.TEN_KHACHHANG;
                            thanhToanDto.DIENTHOAI = data.DIENTHOAI;
                            thanhToanDto.CANCUOC_CONGDAN = data.CANCUOC_CONGDAN;
                            thanhToanDto.DIENGIAI = data.DIENGIAI;
                            thanhToanDto.MABOHANG = data.MABOHANG;
                            thanhToanDto.DtoDetails = listDetails;

                        }
                    }
                    catch
                    {
                        thanhToanDto = null;
                    }
                    finally
                    {
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }
            return thanhToanDto;
        }

        public THANHTOAN_DATPHONG InsertThanhToan(ThanhToanDatPhongViewModel.Dto instance)
        {
            var dataThanhToanDatPhong = Mapper.Map<ThanhToanDatPhongViewModel.Dto, THANHTOAN_DATPHONG>(instance);
            dataThanhToanDatPhong.ID = Guid.NewGuid().ToString();
            dataThanhToanDatPhong.NGAY_THANHTOAN = DateTime.Now;
            dataThanhToanDatPhong.THOIGIAN_THANHTOAN = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
            dataThanhToanDatPhong.I_STATE = "P";
            var result = AddUnit(dataThanhToanDatPhong);
            var dataDetails = Mapper.Map<List<ThanhToanDatPhongViewModel.DtoDetail>, List<THANHTOAN_DATPHONG_CHITIET>>(instance.DtoDetails);
            dataDetails.ForEach(x =>
            {
                x.ID = Guid.NewGuid().ToString();
                x.MA_DATPHONG = result.MA_DATPHONG;
                x.UNITCODE = result.UNITCODE;
                if (!string.IsNullOrEmpty(instance.MAHANG) && x.MAHANG.Equals(instance.MAHANG))
                {
                    decimal THOIGIAN_SUDUNG = 0;
                    decimal.TryParse(instance.THOIGIAN_SUDUNG, out THOIGIAN_SUDUNG);
                    x.SOLUONG = THOIGIAN_SUDUNG;
                }
            });
            result = Insert(result);
            UnitOfWork.Repository<THANHTOAN_DATPHONG_CHITIET>().InsertRange(dataDetails);
            return result;
        }

        public bool UpdateTrangThaiDatPhong(THANHTOAN_DATPHONG instance)
        {
            var result = false;
            DateTime ngayDatPhong = new DateTime(instance.NGAY_DATPHONG.Year, instance.NGAY_DATPHONG.Month, instance.NGAY_DATPHONG.Day, 0, 0, 0);
            var phieuDatPhong = UnitOfWork.Repository<DATPHONG>().DbSet.FirstOrDefault(x => x.MA_DATPHONG.Equals(instance.MA_DATPHONG) && x.MAPHONG.Equals(instance.MAPHONG));
            if (phieuDatPhong != null)
            {
                var LichSuDatPhong = Mapper.Map<DATPHONG, LICHSU_DATPHONG>(phieuDatPhong);
                UnitOfWork.Repository<LICHSU_DATPHONG>().Insert(LichSuDatPhong);
                UnitOfWork.Repository<DATPHONG>().Delete(phieuDatPhong);
                UnitOfWork.Save();
                result = true;
            }
            return result;
        }

        protected override Expression<Func<THANHTOAN_DATPHONG, bool>> GetKeyFilter(THANHTOAN_DATPHONG instance)
        {
            var unitCode = GetCurrentUnitCode();
            return x => x.MA_DATPHONG == instance.MA_DATPHONG && x.UNITCODE.Equals(unitCode);
        }
    }
}
