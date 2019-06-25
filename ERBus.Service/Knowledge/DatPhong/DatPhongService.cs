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
namespace ERBus.Service.Knowledge.DatPhong
{
    public interface IDatPhongService : IDataInfoService<DATPHONG>
    {
        string BuildCode();
        string SaveCode();
        DateTime? GetNullableDateTime(OracleDataReader reader, string columnName);
        PagedObj<DatPhongViewModel.ViewModel> QueryBookingRoom(string stringConnect, PagedObj<DatPhongViewModel.ViewModel> page, string strKey, string maDonVi);
        List<DatPhongViewModel.DatPhongPayDto> GetListCloseBookingRoom(string unitCode, string stringConnect);
    }
    public class DatPhongService : DataInfoServiceBase<DATPHONG>, IDatPhongService
    {
        public DatPhongService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        protected override Expression<Func<DATPHONG, bool>> GetKeyFilter(DATPHONG instance)
        {
            var unitCode = GetCurrentUnitCode();
            return x => x.MA_DATPHONG == instance.MA_DATPHONG && x.UNITCODE.Equals(unitCode);
        }

        public DateTime? GetNullableDateTime(OracleDataReader reader, string columnName)
        {
            var col = reader.GetOrdinal(columnName);
            return reader.IsDBNull(col) ?
                        (DateTime?)null :
                        (DateTime?)reader.GetDateTime(col);
        }

        public string BuildCode()
        {
            var unitCode = GetCurrentUnitCode();
            var type = TypeBuildCode.DATPHONG.ToString();
            var result = "";
            var idRepo = UnitOfWork.Repository<CAPMA_THEONGAY>();
            DateTime nowDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            DateTime tomorrowDay = DateTime.Now.AddDays(1);
            var config = idRepo.DbSet.FirstOrDefault(x => x.LOAIMA == type && x.NHOMMA == type && x.NGAY_SINHMA >= nowDay && x.NGAY_SINHMA < tomorrowDay && x.UNITCODE == unitCode);
            if (config == null)
            {
                config = new CAPMA_THEONGAY
                {
                    ID = Guid.NewGuid().ToString(),
                    LOAIMA = type,
                    NHOMMA = type,
                    NGAY_SINHMA = DateTime.Now,
                    GIATRI = "0",
                    UNITCODE = unitCode
                };
            }
            var newNumber = config.GenerateNumber();
            config.GIATRI = newNumber;
            result = string.Format("{0}{1}{2}_{3}{4}", DateTime.Now.Day.ToString("D2"), DateTime.Now.Month.ToString("D2"), DateTime.Now.Year, config.LOAIMA, newNumber);
            return result;
        }

        public string SaveCode()
        {
            var unitCode = GetCurrentUnitCode();
            var type = TypeBuildCode.DATPHONG.ToString();
            var result = "";
            var idRepo = UnitOfWork.Repository<CAPMA_THEONGAY>();
            DateTime nowDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            DateTime tomorrowDay = DateTime.Now.AddDays(1);
            var config = idRepo.DbSet.FirstOrDefault(x => x.LOAIMA == type && x.NHOMMA == type && x.NGAY_SINHMA >= nowDay && x.NGAY_SINHMA < tomorrowDay && x.UNITCODE == unitCode);
            if (config == null)
            {
                config = new CAPMA_THEONGAY
                {
                    ID = Guid.NewGuid().ToString(),
                    LOAIMA = type,
                    NHOMMA = type,
                    NGAY_SINHMA = DateTime.Now,
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
            result = string.Format("{0}{1}{2}_{3}{4}", DateTime.Now.Day.ToString("D2"), DateTime.Now.Month.ToString("D2"), DateTime.Now.Year, config.LOAIMA, config.GIATRI);
            return result;
        }


        public PagedObj<DatPhongViewModel.ViewModel> QueryBookingRoom(string stringConnect, PagedObj<DatPhongViewModel.ViewModel> page, string maPhong, string maDonVi)
        {
            var TotalItem = 0;
            List<DatPhongViewModel.ViewModel> ListBookingRoom = new List<DatPhongViewModel.ViewModel>();
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
                            command.CommandText = @"TIMKIEM_DATPHONG_PAGINATION";
                            command.Parameters.Add(@"P_MADONVI", OracleDbType.NVarchar2, 50).Value = maDonVi;
                            command.Parameters.Add(@"P_MAPHONG", OracleDbType.NVarchar2, 500).Value = maPhong.ToString().ToUpper().Trim();
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
                                            DatPhongViewModel.ViewModel DatPhong = new DatPhongViewModel.ViewModel();
                                            if (dataReader["ID"] != null)
                                            {
                                                DatPhong.ID = dataReader["ID"].ToString();
                                            }
                                            if (dataReader["MA_DATPHONG"] != null)
                                            {
                                                DatPhong.MA_DATPHONG = dataReader["MA_DATPHONG"].ToString();
                                            }
                                            if (dataReader["MAPHONG"] != null)
                                            {
                                                DatPhong.MAPHONG = dataReader["MAPHONG"].ToString();
                                            }
                                            if (dataReader["THOIGIAN_DATPHONG"] != null)
                                            {
                                                DatPhong.THOIGIAN_DATPHONG = dataReader["THOIGIAN_DATPHONG"].ToString();
                                            }
                                            if (dataReader["TEN_KHACHHANG"] != null)
                                            {
                                                DatPhong.TEN_KHACHHANG = dataReader["TEN_KHACHHANG"].ToString();
                                            }
                                            if (dataReader["DIENTHOAI"] != null)
                                            {
                                                DatPhong.DIENTHOAI = dataReader["DIENTHOAI"].ToString();
                                            }
                                            if (dataReader["CANCUOC_CONGDAN"] != null)
                                            {
                                                DatPhong.CANCUOC_CONGDAN = dataReader["CANCUOC_CONGDAN"].ToString();
                                            }
                                            if (dataReader["DIENGIAI"] != null)
                                            {
                                                DatPhong.DIENGIAI = dataReader["DIENGIAI"].ToString();
                                            }
                                            if (dataReader["TRANGTHAI"] != DBNull.Value)
                                            {
                                                int TRANGTHAI = 0;
                                                int.TryParse(dataReader["TRANGTHAI"].ToString(), out TRANGTHAI);
                                                DatPhong.TRANGTHAI = TRANGTHAI;
                                            }
                                            if (dataReader["NGAY_DATPHONG"] != null)
                                            {
                                                DatPhong.NGAY_DATPHONG = GetNullableDateTime(dataReader, "NGAY_DATPHONG");
                                            }
                                            if (dataReader["MALOAIPHONG"] != null)
                                            {
                                                DatPhong.MALOAIPHONG = dataReader["MALOAIPHONG"].ToString();
                                            }
                                            if (dataReader["UNITCODE"] != null)
                                            {
                                                DatPhong.UNITCODE = dataReader["UNITCODE"].ToString();
                                            }
                                            ListBookingRoom.Add(DatPhong);
                                        }
                                    }
                                }
                            }
                            dataReader.Close();
                            page.Data = ListBookingRoom;
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

        public List<DatPhongViewModel.DatPhongPayDto> GetListCloseBookingRoom(string unitCode, string stringConnect)
        {
            List<DatPhongViewModel.DatPhongPayDto> ListDatPhong = new List<DatPhongViewModel.DatPhongPayDto>();
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
                        command.CommandText = @"SELECT a.MA_DATPHONG,a.MAPHONG,B.TENPHONG,B.TANG,a.NGAY_DATPHONG,a.THOIGIAN_DATPHONG,a.TEN_KHACHHANG,a.DIENGIAI,a.CANCUOC_CONGDAN,C.MALOAIPHONG,C.TENLOAIPHONG,C.MABOHANG FROM DATPHONG a INNER JOIN PHONG b ON A.MAPHONG = B.MAPHONG INNER JOIN LOAIPHONG c ON B.MALOAIPHONG = C.MALOAIPHONG AND a.UNITCODE = b.UNITCODE AND b.UNITCODE = c.UNITCODE AND A.TRANGTHAI = "+ (int)TypeState.CLOSE + " AND A.UNITCODE = '" + unitCode + "' AND TO_DATE(A.NGAY_DATPHONG,'DD-MM-YY') < TO_DATE((SYSDATE + 1),'DD-MM-YY') ";
                        OracleDataReader dataReader = command.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                DatPhongViewModel.DatPhongPayDto ViewModel = new DatPhongViewModel.DatPhongPayDto();
                                if (dataReader["MA_DATPHONG"] != null)
                                {
                                    ViewModel.MA_DATPHONG = dataReader["MA_DATPHONG"].ToString();
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
                                if (dataReader["NGAY_DATPHONG"] != null)
                                {
                                    ViewModel.NGAY_DATPHONG = GetNullableDateTime(dataReader, "NGAY_DATPHONG");
                                }
                                if (dataReader["THOIGIAN_DATPHONG"] != null)
                                {
                                    ViewModel.THOIGIAN_DATPHONG = dataReader["THOIGIAN_DATPHONG"].ToString();
                                }
                                if (dataReader["TEN_KHACHHANG"] != null)
                                {
                                    ViewModel.TEN_KHACHHANG = dataReader["TEN_KHACHHANG"].ToString();
                                }
                                if (dataReader["DIENGIAI"] != null)
                                {
                                    ViewModel.DIENGIAI = dataReader["DIENGIAI"].ToString();
                                }
                                if (dataReader["CANCUOC_CONGDAN"] != null)
                                {
                                    ViewModel.CANCUOC_CONGDAN = dataReader["CANCUOC_CONGDAN"].ToString();
                                }
                                if (dataReader["MALOAIPHONG"] != null)
                                {
                                    ViewModel.MALOAIPHONG = dataReader["MALOAIPHONG"].ToString();
                                }
                                if (dataReader["TENLOAIPHONG"] != null)
                                {
                                    ViewModel.TENLOAIPHONG = dataReader["TENLOAIPHONG"].ToString();
                                }
                                if (dataReader["MABOHANG"] != null)
                                {
                                    ViewModel.MABOHANG = dataReader["MABOHANG"].ToString();
                                }
                                ListDatPhong.Add(ViewModel);
                            }
                        }
                        dataReader.Close();
                    }
                }
                catch
                {
                    ListDatPhong = null;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
            return ListDatPhong;
        }
    }
}
