using AutoMapper;
using ERBus.Entity;
using ERBus.Entity.Database.Catalog;
using ERBus.Service.BuildQuery;
using ERBus.Service.Service;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ERBus.Service.Catalog.MatHang
{
    public interface IMatHangService : IDataInfoService<MATHANG>
    {
        string BuildCode(string maLoaiSelected);
        string SaveCode(string maLoaiSelected);
        MatHangViewModel.InfoUpload UploadImage(bool isAvatar);
        MATHANG InsertDto(MatHangViewModel.Dto instance);
        MATHANG UpdateDto(MatHangViewModel.Dto instance);
        List<MatHangViewModel.VIEW_MODEL> TimKiemMatHang_NhieuDieuKien(string strKey, string unitCode, string stringConnect);
        List<MatHangViewModel.VIEW_MODEL> TimKiemMatHang_TonKho_NhieuDieuKien(string strKey, string unitCode, string stringConnect, string TABLE_NAME, string MaKho);
        string FindExistsBarCode(string barcodeText, string stringConnect);
        bool InsertDataExcel(List<MatHangViewModel.Dto> listExcelData, string unitCode, string stringConnect);
        int KiemTraBarcodeExcelFile(string barcode, string unitCode, string stringConnect);
        PagedObj<MatHangViewModel.VIEW_MODEL> QueryPageMatHang(string stringConnect, PagedObj<MatHangViewModel.VIEW_MODEL> page, string strKey, string maDonVi);
        PagedObj<MatHangViewModel.VIEW_MODEL> QueryPageMatHangInventory(string stringConnect, PagedObj<MatHangViewModel.VIEW_MODEL> page, string strKey, string maDonVi, string TABLE_NAME, string maKho);
    }
    public class MatHangService : DataInfoServiceBase<MATHANG>, IMatHangService
    {
        public MatHangService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        protected override Expression<Func<MATHANG, bool>> GetKeyFilter(MATHANG instance)
        {
            var unitCode = GetCurrentUnitCode();
            return x => x.MAHANG == instance.MAHANG && x.UNITCODE.Equals(unitCode);
        }

        public string BuildCode(string maLoaiSelected)
        {
            var unitCode = GetCurrentUnitCode();
            var type = TypeBuildCode.MATHANG.ToString();
            var result = "";
            var idRepo = UnitOfWork.Repository<CAPMA>();
            var config = idRepo.DbSet.FirstOrDefault(x => x.LOAIMA == type && x.NHOMMA == maLoaiSelected && x.UNITCODE == unitCode);
            if (config == null)
            {
                config = new CAPMA
                {
                    ID = Guid.NewGuid().ToString(),
                    LOAIMA = type,
                    NHOMMA = maLoaiSelected,
                    GIATRI = "000000",
                    UNITCODE = unitCode
                };
            }
            var newNumber = config.GenerateNumber();
            config.GIATRI = newNumber;
            result = string.Format("{0}{1}", config.NHOMMA, newNumber);
            return result;
        }

        public string SaveCode(string maLoaiSelected)
        {
            var unitCode = GetCurrentUnitCode();
            var type = TypeBuildCode.MATHANG.ToString();
            var result = "";
            var idRepo = UnitOfWork.Repository<CAPMA>();
            var config = idRepo.DbSet.FirstOrDefault(x => x.LOAIMA == type && x.NHOMMA == maLoaiSelected && x.UNITCODE == unitCode);
            if (config == null)
            {
                config = new CAPMA
                {
                    ID = Guid.NewGuid().ToString(),
                    LOAIMA = type,
                    NHOMMA = maLoaiSelected,
                    GIATRI = "000000",
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
            result = string.Format("{0}{1}", config.NHOMMA, config.GIATRI);
            return result;
        }

        public PagedObj<MatHangViewModel.VIEW_MODEL> QueryPageMatHang(string stringConnect, PagedObj<MatHangViewModel.VIEW_MODEL> page, string strKey, string maDonVi)
        {
            var TotalItem = 0;
            List<MatHangViewModel.VIEW_MODEL> ListMatHang = new List<MatHangViewModel.VIEW_MODEL>();
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
                            command.CommandText = @"TIMKIEM_MATHANG_PAGINATION";
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
                                            MatHangViewModel.VIEW_MODEL MatHang = new MatHangViewModel.VIEW_MODEL();
                                            if (dataReader["ID"] != null)
                                            {
                                                MatHang.ID = dataReader["ID"].ToString();
                                            }
                                            if (dataReader["MAHANG"] != null)
                                            {
                                                MatHang.MAHANG = dataReader["MAHANG"].ToString();
                                            }
                                            if (dataReader["TENHANG"] != null)
                                            {
                                                MatHang.TENHANG = dataReader["TENHANG"].ToString();
                                            }
                                            if (dataReader["MALOAI"] != null)
                                            {
                                                MatHang.MALOAI = dataReader["MALOAI"].ToString();
                                            }
                                            if (dataReader["MANHOM"] != null)
                                            {
                                                MatHang.MANHOM = dataReader["MANHOM"].ToString();
                                            }
                                            if (dataReader["MADONVITINH"] != null)
                                            {
                                                MatHang.MADONVITINH = dataReader["MADONVITINH"].ToString();
                                            }
                                            if (dataReader["MANHACUNGCAP"] != null)
                                            {
                                                MatHang.MANHACUNGCAP = dataReader["MANHACUNGCAP"].ToString();
                                            }
                                            if (dataReader["MATHUE_VAO"] != null)
                                            {
                                                MatHang.MATHUE_VAO = dataReader["MATHUE_VAO"].ToString();
                                            }
                                            if (dataReader["MATHUE_RA"] != null)
                                            {
                                                MatHang.MATHUE_RA = dataReader["MATHUE_RA"].ToString();
                                            }
                                            if (dataReader["BARCODE"] != null)
                                            {
                                                MatHang.BARCODE = dataReader["BARCODE"].ToString();
                                            }
                                            if (dataReader["TRANGTHAI"] != DBNull.Value)
                                            {
                                                MatHang.TRANGTHAI = int.Parse(dataReader["TRANGTHAI"].ToString());
                                            }
                                            decimal GIABANLE_VAT = 0;
                                            if (dataReader["GIABANLE_VAT"] != DBNull.Value)
                                            {
                                                decimal.TryParse(dataReader["GIABANLE_VAT"].ToString(), out GIABANLE_VAT);
                                            }
                                            MatHang.GIABANLE_VAT = GIABANLE_VAT;
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
                throw new Exception("Lỗi không thể truy xuất hàng hóa");
            }
            return page;
        }


        public PagedObj<MatHangViewModel.VIEW_MODEL> QueryPageMatHangInventory(string stringConnect, PagedObj<MatHangViewModel.VIEW_MODEL> page, string strKey, string maDonVi, string TABLE_NAME, string maKho)
        {
            var TotalItem = 0;
            List<MatHangViewModel.VIEW_MODEL> ListMatHang = new List<MatHangViewModel.VIEW_MODEL>();
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
                            command.CommandText = @"MATHANG_TONKHO_PAGINATION";
                            command.Parameters.Add(@"P_TABLE_NAME", OracleDbType.NVarchar2, 50).Value = TABLE_NAME;
                            command.Parameters.Add(@"P_MAKHO", OracleDbType.NVarchar2, 50).Value = maKho;
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
                                            MatHangViewModel.VIEW_MODEL MatHang = new MatHangViewModel.VIEW_MODEL();
                                            if (dataReader["ID"] != null)
                                            {
                                                MatHang.ID = dataReader["ID"].ToString();
                                            }
                                            if (dataReader["MAHANG"] != null)
                                            {
                                                MatHang.MAHANG = dataReader["MAHANG"].ToString();
                                            }
                                            if (dataReader["TENHANG"] != null)
                                            {
                                                MatHang.TENHANG = dataReader["TENHANG"].ToString();
                                            }
                                            if (dataReader["MALOAI"] != null)
                                            {
                                                MatHang.MALOAI = dataReader["MALOAI"].ToString();
                                            }
                                            if (dataReader["MANHOM"] != null)
                                            {
                                                MatHang.MANHOM = dataReader["MANHOM"].ToString();
                                            }
                                            if (dataReader["MADONVITINH"] != null)
                                            {
                                                MatHang.MADONVITINH = dataReader["MADONVITINH"].ToString();
                                            }
                                            if (dataReader["MANHACUNGCAP"] != null)
                                            {
                                                MatHang.MANHACUNGCAP = dataReader["MANHACUNGCAP"].ToString();
                                            }
                                            if (dataReader["MATHUE_VAO"] != null)
                                            {
                                                MatHang.MATHUE_VAO = dataReader["MATHUE_VAO"].ToString();
                                            }
                                            if (dataReader["MATHUE_RA"] != null)
                                            {
                                                MatHang.MATHUE_RA = dataReader["MATHUE_RA"].ToString();
                                            }
                                            if (dataReader["BARCODE"] != null)
                                            {
                                                MatHang.BARCODE = dataReader["BARCODE"].ToString();
                                            }
                                            if (dataReader["TRANGTHAI"] != DBNull.Value)
                                            {
                                                MatHang.TRANGTHAI = int.Parse(dataReader["TRANGTHAI"].ToString());
                                            }

                                            decimal GIABANLE = 0;
                                            if (dataReader["GIABANLE"] != DBNull.Value)
                                            {
                                                decimal.TryParse(dataReader["GIABANLE"].ToString(), out GIABANLE);
                                            }
                                            MatHang.GIABANLE = GIABANLE;

                                            decimal GIABANLE_VAT = 0;
                                            if (dataReader["GIABANLE_VAT"] != DBNull.Value)
                                            {
                                                decimal.TryParse(dataReader["GIABANLE_VAT"].ToString(), out GIABANLE_VAT);
                                            }
                                            MatHang.GIABANLE_VAT = GIABANLE_VAT;

                                            decimal TYLE_LAILE = 0;
                                            if (dataReader["TYLE_LAILE"] != DBNull.Value)
                                            {
                                                decimal.TryParse(dataReader["TYLE_LAILE"].ToString(), out TYLE_LAILE);
                                            }
                                            MatHang.TYLE_LAILE = TYLE_LAILE;

                                            decimal TYLE_LAIBUON = 0;
                                            if (dataReader["TYLE_LAIBUON"] != DBNull.Value)
                                            {
                                                decimal.TryParse(dataReader["TYLE_LAIBUON"].ToString(), out TYLE_LAIBUON);
                                            }
                                            MatHang.TYLE_LAIBUON = TYLE_LAIBUON;

                                            decimal GIAVON = 0;
                                            if (dataReader["GIAVON"] != DBNull.Value)
                                            {
                                                decimal.TryParse(dataReader["GIAVON"].ToString(), out GIAVON);
                                            }
                                            MatHang.GIAVON = GIAVON;

                                            decimal TONCUOIKYSL = 0;
                                            if (dataReader["TONCUOIKYSL"] != DBNull.Value)
                                            {
                                                decimal.TryParse(dataReader["TONCUOIKYSL"].ToString(), out TONCUOIKYSL);
                                            }
                                            MatHang.TONCUOIKYSL = TONCUOIKYSL;

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
                throw new Exception("Lỗi không thể truy xuất hàng hóa");
            }
            return page;
        }


        public MatHangViewModel.InfoUpload UploadImage(bool isAvatar)
        {
            MatHangViewModel.InfoUpload result = new MatHangViewModel.InfoUpload();
            try
            {
                string path = PhysicalPathUploadFile();
                HttpRequest request = HttpContext.Current.Request;
                var maHang = request.Form["MAHANG"];
                path += maHang + "\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                result.DUONGDAN = "/Upload/ImageMatHang/" + maHang + "/";
                try
                {
                    if (isAvatar)
                    {
                        if (request.Files.Count > 0)
                        {
                            HttpPostedFile file = request.Files[0];
                            List<string> tmp = file.FileName.Split('.').ToList();
                            string extension = tmp.Count > 0 ? tmp[1] : "jpg";
                            string fileName = string.Format("{0}.{1}", maHang, extension);
                            file.SaveAs(path + fileName);
                            result.FILENAME = fileName;
                        }
                    }
                    else
                    {
                        if (request.Files.Count > 0)
                        {
                            for (int i = 0; i < request.Files.Count; i++)
                            {
                                HttpPostedFile file = request.Files[i];
                                List<string> tmp = file.FileName.Split('.').ToList();
                                string extension = tmp.Count > 0 ? tmp[1] : "jpg";
                                string fileName = string.Format("{0}_{1}{2}{3}.{4}", maHang, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond, extension);
                                file.SaveAs(path + fileName);
                                result.FILENAME += fileName + ",";
                            }
                        }
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

        public MATHANG InsertDto(MatHangViewModel.Dto instance)
        {
            var dataMatHang = Mapper.Map<MatHangViewModel.Dto, MATHANG>(instance);
            var dataMatHangGia = Mapper.Map<MatHangViewModel.Dto, MATHANG_GIA>(instance);
            dataMatHang.I_CREATE_BY = GetClaimsPrincipal().Identity.Name;
            dataMatHang.I_CREATE_DATE = DateTime.Now;
            dataMatHang.I_UPDATE_BY = GetClaimsPrincipal().Identity.Name;
            dataMatHang.I_UPDATE_DATE = DateTime.Now;
            dataMatHang.UNITCODE = GetCurrentUnitCode();
            dataMatHang.I_STATE = TypeState.C.ToString();
            dataMatHang.TRANGTHAI = (int)TypeState.USED;

            dataMatHangGia.I_CREATE_BY = GetClaimsPrincipal().Identity.Name;
            dataMatHangGia.I_CREATE_DATE = DateTime.Now;
            dataMatHangGia.I_UPDATE_BY = GetClaimsPrincipal().Identity.Name;
            dataMatHangGia.I_UPDATE_DATE = DateTime.Now;
            dataMatHangGia.UNITCODE = GetCurrentUnitCode();
            dataMatHangGia.I_STATE = TypeState.C.ToString();
            dataMatHangGia.TRANGTHAI = (int)TypeState.USED;
            dataMatHangGia.ID = Guid.NewGuid().ToString();
            if (!string.IsNullOrEmpty(instance.AVATAR_NAME) && !string.IsNullOrEmpty(dataMatHang.DUONGDAN))
            {
                FileStream fs = new FileStream(PhysicalPathUploadFile() + instance.MAHANG + "\\" + instance.AVATAR_NAME, FileMode.Open, FileAccess.Read);
                byte[] ImageBytes = new byte[fs.Length];
                fs.Read(ImageBytes, 0, System.Convert.ToInt32(fs.Length));
                fs.Close();
                dataMatHang.AVATAR = ImageBytes;
            }
            var result = Insert(dataMatHang);
            UnitOfWork.Repository<MATHANG_GIA>().Insert(dataMatHangGia);
            return result;
        }

        public MATHANG UpdateDto(MatHangViewModel.Dto instance)
        {
            MATHANG result = new MATHANG();
            string unitCode = GetCurrentUnitCode();
            MATHANG existItem = FindById(instance.ID);
            if (existItem == null)
            {
                return null;
            }
            else
            {
                MATHANG matHang = Mapper.Map<MatHangViewModel.Dto, MATHANG>(instance);
                matHang.DUONGDAN = "/Upload/ImageMatHang/" + matHang.MAHANG + "/";
                matHang.I_STATE = existItem.I_STATE;
                matHang.I_CREATE_BY = existItem.I_CREATE_BY;
                matHang.I_CREATE_DATE = existItem.I_CREATE_DATE;
                matHang.I_UPDATE_BY = GetClaimsPrincipal().Identity.Name;
                matHang.I_UPDATE_DATE = DateTime.Now;
                matHang.UNITCODE = unitCode;
                if (!string.IsNullOrEmpty(instance.AVATAR_NAME) && !string.IsNullOrEmpty(matHang.DUONGDAN))
                {
                    FileStream fs = new FileStream(PhysicalPathUploadFile() + instance.MAHANG + "\\" + instance.AVATAR_NAME, FileMode.Open, FileAccess.Read);
                    byte[] ImageBytes = new byte[fs.Length];
                    fs.Read(ImageBytes, 0, System.Convert.ToInt32(fs.Length));
                    fs.Close();
                    matHang.AVATAR = ImageBytes;
                }
                var matHangGia = UnitOfWork.Repository<MATHANG_GIA>().DbSet.FirstOrDefault(x => x.MAHANG.Equals(existItem.MAHANG) && x.UNITCODE.Equals(unitCode) && x.TRANGTHAI == (int)TypeState.USED);
                if (matHangGia != null)
                {
                    if (matHang.TRANGTHAI == 0) matHangGia.TRANGTHAI = 0;
                    matHangGia.GIAMUA = instance.GIAMUA;
                    matHangGia.GIAMUA_VAT = instance.GIAMUA_VAT;
                    matHangGia.GIABANLE = instance.GIABANLE;
                    matHangGia.GIABANLE_VAT = instance.GIABANLE_VAT;
                    matHangGia.GIABANBUON = instance.GIABANBUON;
                    matHangGia.GIABANBUON_VAT = instance.GIABANBUON_VAT;
                    matHangGia.TYLE_LAILE = instance.TYLE_LAILE;
                    matHangGia.TYLE_LAIBUON = instance.TYLE_LAIBUON;
                    matHangGia.ObjectState = ObjectState.Modified;
                }
                result = Update(matHang);
                //UnitOfWork.Repository<MATHANG_GIA>().Update(matHangGia);
            }
            return result;
        }

        public List<MatHangViewModel.VIEW_MODEL> TimKiemMatHang_NhieuDieuKien(string strKey, string unitCode, string stringConnect)
        {
            List<MatHangViewModel.VIEW_MODEL> ListViewModel = new List<MatHangViewModel.VIEW_MODEL>();
            if (!string.IsNullOrEmpty(strKey))
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
                            command.CommandText = @"TIMKIEM_MATHANG";
                            command.Parameters.Add(@"P_MADONVI", OracleDbType.NVarchar2, 50).Value = unitCode;
                            command.Parameters.Add(@"P_TUKHOA", OracleDbType.NVarchar2, 50).Value = strKey.ToString().ToUpper().Trim();
                            command.Parameters.Add(@"CURSOR_RESULT", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                            OracleDataReader dataReader = command.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    MatHangViewModel.VIEW_MODEL ViewModel = new MatHangViewModel.VIEW_MODEL();
                                    if (dataReader["ID"] != null)
                                    {
                                        ViewModel.ID = dataReader["ID"].ToString();
                                    }
                                    if (dataReader["MAHANG"] != null)
                                    {
                                        ViewModel.MAHANG = dataReader["MAHANG"].ToString();
                                    }
                                    if (dataReader["TENHANG"] != null)
                                    {
                                        ViewModel.TENHANG = dataReader["TENHANG"].ToString();
                                    }
                                    if (dataReader["MALOAI"] != null)
                                    {
                                        ViewModel.MALOAI = dataReader["MALOAI"].ToString();
                                    }
                                    if (dataReader["MANHOM"] != null)
                                    {
                                        ViewModel.MANHOM = dataReader["MANHOM"].ToString();
                                    }
                                    if (dataReader["MADONVITINH"] != null)
                                    {
                                        ViewModel.MADONVITINH = dataReader["MADONVITINH"].ToString();
                                    }
                                    if (dataReader["MANHACUNGCAP"] != null)
                                    {
                                        ViewModel.MANHACUNGCAP = dataReader["MANHACUNGCAP"].ToString();
                                    }
                                    if (dataReader["MATHUE_VAO"] != null)
                                    {
                                        ViewModel.MATHUE_VAO = dataReader["MATHUE_VAO"].ToString();
                                    }
                                    if (dataReader["MATHUE_RA"] != null)
                                    {
                                        ViewModel.MATHUE_RA = dataReader["MATHUE_RA"].ToString();
                                    }
                                    if (dataReader["BARCODE"] != null)
                                    {
                                        ViewModel.BARCODE = dataReader["BARCODE"].ToString();
                                    }
                                    if (dataReader["TYLE_LAILE"] != DBNull.Value)
                                    {
                                        decimal TYLE_LAILE = 0;
                                        decimal.TryParse(dataReader["TYLE_LAILE"].ToString(), out TYLE_LAILE);
                                        ViewModel.TYLE_LAILE = TYLE_LAILE;
                                    }
                                    if (dataReader["TYLE_LAIBUON"] != DBNull.Value)
                                    {
                                        decimal TYLE_LAIBUON = 0;
                                        decimal.TryParse(dataReader["TYLE_LAIBUON"].ToString(), out TYLE_LAIBUON);
                                        ViewModel.TYLE_LAIBUON = TYLE_LAIBUON;
                                    }
                                    if (dataReader["GIAMUA"] != DBNull.Value)
                                    {
                                        decimal GIAMUA = 0;
                                        decimal.TryParse(dataReader["GIAMUA"].ToString(), out GIAMUA);
                                        ViewModel.GIAMUA = GIAMUA;
                                    }
                                    if (dataReader["GIAMUA_VAT"] != DBNull.Value)
                                    {
                                        decimal GIAMUA_VAT = 0;
                                        decimal.TryParse(dataReader["GIAMUA_VAT"].ToString(), out GIAMUA_VAT);
                                        ViewModel.GIAMUA_VAT = GIAMUA_VAT;
                                    }
                                    if (dataReader["GIABANLE"] != DBNull.Value)
                                    {
                                        decimal GIABANLE = 0;
                                        decimal.TryParse(dataReader["GIABANLE"].ToString(), out GIABANLE);
                                        ViewModel.GIABANLE = GIABANLE;
                                    }
                                    if (dataReader["GIABANLE_VAT"] != DBNull.Value)
                                    {
                                        decimal GIABANLE_VAT = 0;
                                        decimal.TryParse(dataReader["GIABANLE_VAT"].ToString(), out GIABANLE_VAT);
                                        ViewModel.GIABANLE_VAT = GIABANLE_VAT;
                                    }
                                    if (dataReader["GIABANBUON"] != DBNull.Value)
                                    {
                                        decimal GIABANBUON = 0;
                                        decimal.TryParse(dataReader["GIABANBUON"].ToString(), out GIABANBUON);
                                        ViewModel.GIABANBUON = GIABANBUON;
                                    }
                                    if (dataReader["GIABANBUON_VAT"] != DBNull.Value)
                                    {
                                        decimal GIABANBUON_VAT = 0;
                                        decimal.TryParse(dataReader["GIABANBUON_VAT"].ToString(), out GIABANBUON_VAT);
                                        ViewModel.GIABANBUON_VAT = GIABANBUON_VAT;
                                    }
                                    if (dataReader["TRANGTHAI"] != DBNull.Value)
                                    {
                                        int TRANGTHAI = 0;
                                        int.TryParse(dataReader["TRANGTHAI"].ToString(), out TRANGTHAI);
                                        ViewModel.TRANGTHAI = TRANGTHAI;
                                    }
                                    ListViewModel.Add(ViewModel);
                                }
                            }
                        }
                    }
                    catch
                    {
                        ListViewModel = null;
                    }
                    finally
                    {
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }
            return ListViewModel;
        }


        public List<MatHangViewModel.VIEW_MODEL> TimKiemMatHang_TonKho_NhieuDieuKien(string strKey, string unitCode, string stringConnect, string TABLE_NAME, string MaKho)
        {
            List<MatHangViewModel.VIEW_MODEL> ListViewModel = new List<MatHangViewModel.VIEW_MODEL>();
            if (!string.IsNullOrEmpty(strKey))
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
                            command.CommandText = @"TIMKIEM_MATHANG_TONKHO";
                            command.Parameters.Add(@"P_TABLE_NAME", OracleDbType.NVarchar2, 50).Value = TABLE_NAME;
                            command.Parameters.Add(@"P_MAKHO", OracleDbType.NVarchar2, 50).Value = MaKho;
                            command.Parameters.Add(@"P_MADONVI", OracleDbType.NVarchar2, 50).Value = unitCode;
                            command.Parameters.Add(@"P_TUKHOA", OracleDbType.NVarchar2, 50).Value = strKey.ToString().ToUpper().Trim();
                            command.Parameters.Add(@"CURSOR_RESULT", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                            OracleDataReader dataReader = command.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    MatHangViewModel.VIEW_MODEL ViewModel = new MatHangViewModel.VIEW_MODEL();
                                    if (dataReader["ID"] != null)
                                    {
                                        ViewModel.ID = dataReader["ID"].ToString();
                                    }
                                    if (dataReader["MAHANG"] != null)
                                    {
                                        ViewModel.MAHANG = dataReader["MAHANG"].ToString();
                                    }
                                    if (dataReader["TENHANG"] != null)
                                    {
                                        ViewModel.TENHANG = dataReader["TENHANG"].ToString();
                                    }
                                    if (dataReader["MALOAI"] != null)
                                    {
                                        ViewModel.MALOAI = dataReader["MALOAI"].ToString();
                                    }
                                    if (dataReader["MANHOM"] != null)
                                    {
                                        ViewModel.MANHOM = dataReader["MANHOM"].ToString();
                                    }
                                    if (dataReader["MADONVITINH"] != null)
                                    {
                                        ViewModel.MADONVITINH = dataReader["MADONVITINH"].ToString();
                                    }
                                    if (dataReader["MANHACUNGCAP"] != null)
                                    {
                                        ViewModel.MANHACUNGCAP = dataReader["MANHACUNGCAP"].ToString();
                                    }
                                    if (dataReader["MATHUE_VAO"] != null)
                                    {
                                        ViewModel.MATHUE_VAO = dataReader["MATHUE_VAO"].ToString();
                                    }
                                    if (dataReader["MATHUE_RA"] != null)
                                    {
                                        ViewModel.MATHUE_RA = dataReader["MATHUE_RA"].ToString();
                                    }
                                    if (dataReader["BARCODE"] != null)
                                    {
                                        ViewModel.BARCODE = dataReader["BARCODE"].ToString();
                                    }
                                    if (dataReader["TYLE_LAILE"] != DBNull.Value)
                                    {
                                        decimal TYLE_LAILE = 0;
                                        decimal.TryParse(dataReader["TYLE_LAILE"].ToString(), out TYLE_LAILE);
                                        ViewModel.TYLE_LAILE = TYLE_LAILE;
                                    }
                                    if (dataReader["TYLE_LAIBUON"] != DBNull.Value)
                                    {
                                        decimal TYLE_LAIBUON = 0;
                                        decimal.TryParse(dataReader["TYLE_LAIBUON"].ToString(), out TYLE_LAIBUON);
                                        ViewModel.TYLE_LAIBUON = TYLE_LAIBUON;
                                    }
                                    if (dataReader["GIAMUA"] != DBNull.Value)
                                    {
                                        decimal GIAMUA = 0;
                                        decimal.TryParse(dataReader["GIAMUA"].ToString(), out GIAMUA);
                                        ViewModel.GIAMUA = GIAMUA;
                                    }
                                    if (dataReader["GIAMUA_VAT"] != DBNull.Value)
                                    {
                                        decimal GIAMUA_VAT = 0;
                                        decimal.TryParse(dataReader["GIAMUA_VAT"].ToString(), out GIAMUA_VAT);
                                        ViewModel.GIAMUA_VAT = GIAMUA_VAT;
                                    }
                                    if (dataReader["GIABANLE"] != DBNull.Value)
                                    {
                                        decimal GIABANLE = 0;
                                        decimal.TryParse(dataReader["GIABANLE"].ToString(), out GIABANLE);
                                        ViewModel.GIABANLE = GIABANLE;
                                    }
                                    if (dataReader["GIABANLE_VAT"] != DBNull.Value)
                                    {
                                        decimal GIABANLE_VAT = 0;
                                        decimal.TryParse(dataReader["GIABANLE_VAT"].ToString(), out GIABANLE_VAT);
                                        ViewModel.GIABANLE_VAT = GIABANLE_VAT;
                                    }
                                    if (dataReader["GIABANBUON"] != DBNull.Value)
                                    {
                                        decimal GIABANBUON = 0;
                                        decimal.TryParse(dataReader["GIABANBUON"].ToString(), out GIABANBUON);
                                        ViewModel.GIABANBUON = GIABANBUON;
                                    }
                                    if (dataReader["GIABANBUON_VAT"] != DBNull.Value)
                                    {
                                        decimal GIABANBUON_VAT = 0;
                                        decimal.TryParse(dataReader["GIABANBUON_VAT"].ToString(), out GIABANBUON_VAT);
                                        ViewModel.GIABANBUON_VAT = GIABANBUON_VAT;
                                    }
                                    if (dataReader["GIAVON"] != DBNull.Value)
                                    {
                                        decimal GIAVON = 0;
                                        decimal.TryParse(dataReader["GIAVON"].ToString(), out GIAVON);
                                        ViewModel.GIAVON = GIAVON;
                                    }
                                    if (dataReader["TONCUOIKYSL"] != DBNull.Value)
                                    {
                                        decimal TONCUOIKYSL = 0;
                                        decimal.TryParse(dataReader["TONCUOIKYSL"].ToString(), out TONCUOIKYSL);
                                        ViewModel.TONCUOIKYSL = TONCUOIKYSL;
                                    }
                                    if (dataReader["TRANGTHAI"] != DBNull.Value)
                                    {
                                        int TRANGTHAI = 0;
                                        int.TryParse(dataReader["TRANGTHAI"].ToString(), out TRANGTHAI);
                                        ViewModel.TRANGTHAI = TRANGTHAI;
                                    }
                                    ListViewModel.Add(ViewModel);
                                }
                            }
                        }
                    }
                    catch
                    {
                        ListViewModel = null;
                    }
                    finally
                    {
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }
            return ListViewModel;
        }


        public string FindExistsBarCode(string barcodeText, string stringConnect)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(barcodeText))
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
                            command.CommandText = string.Format(@"SELECT A.MAHANG FROM
                                                                (
                                                                    SELECT
                                                                        MAHANG,SUBSTR(BARCODE, INSTR(BARCODE, :P_BARCODE), INSTR(BARCODE, ';') - 1) AS BARCODE
                                                                    FROM
                                                                        MATHANG
                                                                    WHERE
                                                                        INSTR(BARCODE, :P_BARCODE) > 0
                                                                ) A WHERE LENGTH(A.BARCODE) = LENGTH(:P_BARCODE)");
                            command.Parameters.Add(@"P_BARCODE", OracleDbType.NVarchar2, 100).Value = barcodeText;
                            OracleDataReader dataReader = command.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    result = dataReader["MAHANG"] != null ? dataReader["MAHANG"].ToString() : string.Empty;
                                }
                            }
                        }
                    }
                    catch
                    {
                        result = string.Empty;
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

        public int KiemTraBarcodeExcelFile(string barcode, string unitCode, string stringConnect)
        {
            int result = 0;
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
                        command.CommandText = @"KIEMTRA_TRUNGBARCODE_EXCEL";
                        command.Parameters.Add(@"P_MADONVI", OracleDbType.NVarchar2, 50).Value = unitCode;
                        command.Parameters.Add(@"P_BARCODE", OracleDbType.NVarchar2, 2000).Value = barcode.ToString().Trim();
                        command.Parameters.Add(@"CURSOR_RESULT", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        OracleDataReader dataReader = command.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                int.TryParse(dataReader["REFUND_DATA"] != null ? dataReader["REFUND_DATA"].ToString() : "", out result);
                            }
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

        public string MergeStringToNewCode(string input, int length, string character)
        {
            var result = input;
            while (result.Length < length)
            {
                result = string.Format("{0}{1}", character, result);
            }
            return result;
        }

        public string IncrementNewNumber(string giaTri)
        {
            var result = "";
            int number;
            var length = giaTri.Length;
            if (int.TryParse(giaTri, out number))
            {
                result = string.Format("{0}", number + 1);
                result = MergeStringToNewCode(result, length, "0");
            }
            return result;
        }

        public bool InsertDataExcel(List<MatHangViewModel.Dto> listExcelData, string unitCode, string stringConnect)
        {
            var result = false;
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
                            if (listExcelData.Count > 0)
                            {
                                foreach (MatHangViewModel.Dto dataRowDto in listExcelData)
                                {
                                    //get increment code
                                    string GIATRI = "";
                                    var type = TypeBuildCode.MATHANG.ToString();
                                    command.Parameters.Clear();
                                    command.CommandType = CommandType.Text;
                                    command.CommandText = "SELECT NHOMMA,MAX(GIATRI) AS GIATRI FROM CAPMA WHERE LOAIMA = :P_LOAIMA AND NHOMMA = :P_NHOMMA AND UNITCODE = :P_UNITCODE GROUP BY NHOMMA";
                                    command.Parameters.Add("P_LOAIMA", OracleDbType.Varchar2, 50, type, ParameterDirection.Input);
                                    command.Parameters.Add("P_NHOMMA", OracleDbType.Varchar2, dataRowDto.MALOAI, ParameterDirection.Input);
                                    command.Parameters.Add("P_UNITCODE", OracleDbType.Varchar2, 10, unitCode, ParameterDirection.Input);
                                    OracleDataReader dataReader = command.ExecuteReader();
                                    if (dataReader.HasRows)
                                    {
                                        while (dataReader.Read())
                                        {
                                            GIATRI = dataReader["GIATRI"] != null ? dataReader["GIATRI"].ToString() : "";
                                            var newNumber = IncrementNewNumber(GIATRI);
                                            dataRowDto.MAHANG = string.Format("{0}{1}", dataRowDto.MALOAI, newNumber);
                                            command.Parameters.Clear();
                                            command.CommandType = CommandType.Text;
                                            command.CommandText = "UPDATE CAPMA SET GIATRI = :P_GIATRI WHERE LOAIMA = :P_LOAIMA AND NHOMMA = :P_NHOMMA AND UNITCODE = :P_UNITCODE";
                                            command.Parameters.Add("P_GIATRI", OracleDbType.Varchar2, 50, newNumber, ParameterDirection.Input);
                                            command.Parameters.Add("P_LOAIMA", OracleDbType.Varchar2, 50, type, ParameterDirection.Input);
                                            command.Parameters.Add("P_NHOMMA", OracleDbType.Varchar2, dataRowDto.MALOAI, ParameterDirection.Input);
                                            command.Parameters.Add("P_UNITCODE", OracleDbType.Varchar2, 10, unitCode, ParameterDirection.Input);
                                        }
                                    }
                                    else
                                    {
                                        GIATRI = "000000";
                                        var newNumber = IncrementNewNumber(GIATRI);
                                        dataRowDto.MAHANG = string.Format("{0}{1}", dataRowDto.MALOAI, newNumber);
                                        command.Parameters.Clear();
                                        command.CommandType = CommandType.Text;
                                        command.CommandText = "INSERT INTO CAPMA(ID,LOAIMA,NHOMMA,GIATRI,UNITCODE) VALUES(:P_ID,:P_LOAIMA,:P_NHOMMA,:P_GIATRI,:P_UNITCODE) ";
                                        command.Parameters.Add("P_ID", OracleDbType.Varchar2, 50, newNumber, ParameterDirection.Input);
                                        command.Parameters.Add("P_LOAIMA", OracleDbType.Varchar2, 50, type, ParameterDirection.Input);
                                        command.Parameters.Add("P_NHOMMA", OracleDbType.Varchar2, dataRowDto.MALOAI, ParameterDirection.Input);
                                        command.Parameters.Add("P_GIATRI", OracleDbType.Varchar2, 50, newNumber, ParameterDirection.Input);
                                        command.Parameters.Add("P_UNITCODE", OracleDbType.Varchar2, 10, unitCode, ParameterDirection.Input);
                                    }
                                    command.ExecuteNonQuery();
                                    //validate thông tin hàng hóa
                                    var dataMatHang = Mapper.Map<MatHangViewModel.Dto, MATHANG>(dataRowDto);
                                    var dataMatHangGia = Mapper.Map<MatHangViewModel.Dto, MATHANG_GIA>(dataRowDto);
                                    dataMatHang.I_CREATE_BY = GetClaimsPrincipal().Identity.Name;
                                    dataMatHang.I_CREATE_DATE = DateTime.Now;
                                    dataMatHang.I_UPDATE_BY = GetClaimsPrincipal().Identity.Name;
                                    dataMatHang.I_UPDATE_DATE = DateTime.Now;
                                    dataMatHang.UNITCODE = GetCurrentUnitCode();
                                    dataMatHang.I_STATE = TypeState.I.ToString();
                                    dataMatHang.TRANGTHAI = (int)TypeState.USED;
                                    dataMatHang.ID = Guid.NewGuid().ToString();
                                    dataMatHang.UNITCODE = unitCode;

                                    dataMatHangGia.I_CREATE_BY = GetClaimsPrincipal().Identity.Name;
                                    dataMatHangGia.I_CREATE_DATE = DateTime.Now;
                                    dataMatHangGia.I_UPDATE_BY = GetClaimsPrincipal().Identity.Name;
                                    dataMatHangGia.I_UPDATE_DATE = DateTime.Now;
                                    dataMatHangGia.UNITCODE = GetCurrentUnitCode();
                                    dataMatHangGia.I_STATE = TypeState.I.ToString();
                                    dataMatHangGia.TRANGTHAI = (int)TypeState.USED;
                                    dataMatHangGia.ID = Guid.NewGuid().ToString();
                                    dataMatHangGia.UNITCODE = unitCode;
                                    if (!string.IsNullOrEmpty(dataMatHang.MAHANG))
                                    {
                                        //insert table MATHANG
                                        command.Parameters.Clear();
                                        command.CommandType = CommandType.Text;
                                        command.CommandText = string.Format(@"INSERT INTO MATHANG(ID,MAHANG,TENHANG,MANHACUNGCAP,MALOAI,MANHOM,MATHUE_VAO,MATHUE_RA,MADONVITINH,BARCODE,TRANGTHAI,I_CREATE_DATE,I_CREATE_BY,I_UPDATE_DATE,I_UPDATE_BY,I_STATE,UNITCODE) VALUES (:P_ID,:P_MAHANG,:P_TENHANG,:P_MANHACUNGCAP,:P_MALOAI,:P_MANHOM,:P_MATHUE_VAO,:P_MATHUE_RA,:P_MADONVITINH,:P_BARCODE,:P_TRANGTHAI,:P_I_CREATE_DATE,:P_I_CREATE_BY,:P_I_UPDATE_DATE,:P_I_UPDATE_BY,:P_I_STATE,:P_UNITCODE)");
                                        command.Parameters.Add("P_ID", OracleDbType.Varchar2, 50, dataMatHang.ID, ParameterDirection.Input);
                                        command.Parameters.Add("P_MAHANG", OracleDbType.Varchar2, 50, dataMatHang.MAHANG, ParameterDirection.Input);
                                        command.Parameters.Add("P_TENHANG", OracleDbType.Varchar2, 300, dataMatHang.TENHANG, ParameterDirection.Input);
                                        command.Parameters.Add("P_MANHACUNGCAP", OracleDbType.Varchar2, 50, dataMatHang.MANHACUNGCAP, ParameterDirection.Input);
                                        command.Parameters.Add("P_MALOAI", OracleDbType.Varchar2, 50, dataMatHang.MALOAI, ParameterDirection.Input);
                                        command.Parameters.Add("P_MANHOM", OracleDbType.Varchar2, 50, dataMatHang.MANHOM, ParameterDirection.Input);
                                        command.Parameters.Add("P_MATHUE_VAO", OracleDbType.Varchar2, 50, dataMatHang.MATHUE_VAO, ParameterDirection.Input);
                                        command.Parameters.Add("P_MATHUE_RA", OracleDbType.Varchar2, 50, dataMatHang.MATHUE_RA, ParameterDirection.Input);
                                        command.Parameters.Add("P_MADONVITINH", OracleDbType.Varchar2, 50, dataMatHang.MADONVITINH, ParameterDirection.Input);
                                        command.Parameters.Add("P_BARCODE", OracleDbType.Varchar2, 2000, dataMatHang.BARCODE, ParameterDirection.Input);
                                        command.Parameters.Add("P_TRANGTHAI", OracleDbType.Int32, dataMatHang.TRANGTHAI, ParameterDirection.Input);
                                        command.Parameters.Add("P_I_CREATE_DATE", OracleDbType.Date, dataMatHang.I_CREATE_DATE, ParameterDirection.Input);
                                        command.Parameters.Add("P_I_CREATE_BY", OracleDbType.Varchar2, 50, dataMatHang.I_CREATE_BY, ParameterDirection.Input);
                                        command.Parameters.Add("P_I_UPDATE_DATE", OracleDbType.Date, dataMatHang.I_UPDATE_DATE, ParameterDirection.Input);
                                        command.Parameters.Add("P_I_UPDATE_BY", OracleDbType.Varchar2, 50, dataMatHang.I_UPDATE_BY, ParameterDirection.Input);
                                        command.Parameters.Add("P_I_STATE", OracleDbType.Varchar2, 1, dataMatHang.I_STATE, ParameterDirection.Input);
                                        command.Parameters.Add("P_UNITCODE", OracleDbType.Varchar2, 10, dataMatHang.UNITCODE, ParameterDirection.Input);
                                        command.ExecuteNonQuery();
                                        //insert table MATHANG_GIA
                                        command.Parameters.Clear();
                                        command.CommandType = CommandType.Text;
                                        command.CommandText = string.Format(@"INSERT INTO MATHANG_GIA(ID,MAHANG,GIAMUA,GIAMUA_VAT,GIABANLE,GIABANLE_VAT,GIABANBUON,GIABANBUON_VAT,TYLE_LAILE,TYLE_LAIBUON,TRANGTHAI,I_CREATE_DATE,I_CREATE_BY,I_UPDATE_DATE,I_UPDATE_BY,I_STATE,UNITCODE) VALUES (:P_ID,:P_MAHANG,:P_GIAMUA,:P_GIAMUA_VAT,:P_GIABANLE,:P_GIABANLE_VAT,:P_GIABANBUON,:P_GIABANBUON_VAT,:P_TYLE_LAILE,:P_TYLE_LAIBUON,:P_TRANGTHAI,:P_I_CREATE_DATE,:P_I_CREATE_BY,:P_I_UPDATE_DATE,:P_I_UPDATE_BY,:P_I_STATE,:P_UNITCODE)");
                                        command.Parameters.Add("P_ID", OracleDbType.Varchar2, 50, dataMatHangGia.ID, ParameterDirection.Input);
                                        command.Parameters.Add("P_MAHANG", OracleDbType.Varchar2, 50, dataMatHangGia.MAHANG, ParameterDirection.Input);
                                        command.Parameters.Add("P_GIAMUA", OracleDbType.Decimal, dataMatHangGia.GIAMUA, ParameterDirection.Input);
                                        command.Parameters.Add("P_GIAMUA_VAT", OracleDbType.Decimal, dataMatHangGia.GIAMUA_VAT, ParameterDirection.Input);
                                        command.Parameters.Add("P_GIABANLE", OracleDbType.Decimal, dataMatHangGia.GIABANLE, ParameterDirection.Input);
                                        command.Parameters.Add("P_GIABANLE_VAT", OracleDbType.Decimal, dataMatHangGia.GIABANLE_VAT, ParameterDirection.Input);
                                        command.Parameters.Add("P_GIABANBUON", OracleDbType.Decimal, dataMatHangGia.GIABANBUON, ParameterDirection.Input);
                                        command.Parameters.Add("P_GIABANBUON_VAT", OracleDbType.Decimal, dataMatHangGia.GIABANBUON_VAT, ParameterDirection.Input);
                                        command.Parameters.Add("P_TYLE_LAILE", OracleDbType.Decimal, dataMatHangGia.TYLE_LAILE, ParameterDirection.Input);
                                        command.Parameters.Add("P_TYLE_LAIBUON", OracleDbType.Decimal, dataMatHangGia.TYLE_LAIBUON, ParameterDirection.Input);
                                        command.Parameters.Add("P_TRANGTHAI", OracleDbType.Int32, dataMatHangGia.TRANGTHAI, ParameterDirection.Input);
                                        command.Parameters.Add("P_I_CREATE_DATE", OracleDbType.Date, dataMatHangGia.I_CREATE_DATE, ParameterDirection.Input);
                                        command.Parameters.Add("P_I_CREATE_BY", OracleDbType.Varchar2, 50, dataMatHangGia.I_CREATE_BY, ParameterDirection.Input);
                                        command.Parameters.Add("P_I_UPDATE_DATE", OracleDbType.Date, dataMatHangGia.I_UPDATE_DATE, ParameterDirection.Input);
                                        command.Parameters.Add("P_I_UPDATE_BY", OracleDbType.Varchar2, 50, dataMatHangGia.I_UPDATE_BY, ParameterDirection.Input);
                                        command.Parameters.Add("P_I_STATE", OracleDbType.Varchar2, 1, dataMatHangGia.I_STATE, ParameterDirection.Input);
                                        command.Parameters.Add("P_UNITCODE", OracleDbType.Varchar2, 10, dataMatHangGia.UNITCODE, ParameterDirection.Input);
                                        command.ExecuteNonQuery();
                                    }
                                }
                            }
                            transaction.Commit();
                            result = true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            result = false;
                        }
                        finally
                        {
                            transaction.Dispose();
                        }
                    }
                }
                catch (Exception ex)
                {
                    result = false;
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
