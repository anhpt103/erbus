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

namespace ERBus.Service.Promotion.GiamGiaLoaiHang
{
    public interface IGiamGiaLoaiHangService : IDataInfoService<KHUYENMAI>
    {
        string BuildCode();
        string SaveCode();
        KHUYENMAI InsertKhuyenMai(GiamGiaLoaiHangViewModel.Dto instance);
        KHUYENMAI UpdateKhuyenMai(GiamGiaLoaiHangViewModel.Dto instance);
        bool DeleteKhuyenMai(string ID);
        PagedObj<KHUYENMAI> QueryPageGiamGiaLoaiHang(string stringConnect, PagedObj<KHUYENMAI> page, string strKey, string maDonVi);
    }
    public class GiamGiaLoaiHangService : DataInfoServiceBase<KHUYENMAI>, IGiamGiaLoaiHangService
    {
        public GiamGiaLoaiHangService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        protected override Expression<Func<KHUYENMAI, bool>> GetKeyFilter(KHUYENMAI instance)
        {
            var unitCode = GetCurrentUnitCode();
            return x => x.MA_KHUYENMAI == instance.MA_KHUYENMAI && x.UNITCODE.Equals(unitCode);
        }

        public string BuildCode()
        {
            var unitCode = GetCurrentUnitCode();
            var type = TypeBuildCode.GGLOAIHANG.ToString();
            var result = "";
            var idRepo = UnitOfWork.Repository<CAPMA>();
            var config = idRepo.DbSet.FirstOrDefault(x => x.LOAIMA == type && x.UNITCODE == unitCode);
            if (config == null)
            {
                config = new CAPMA
                {
                    ID = Guid.NewGuid().ToString(),
                    LOAIMA = type,
                    NHOMMA = "KHUYENMAI",
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
            var type = TypeBuildCode.GGLOAIHANG.ToString();
            var result = "";
            var idRepo = UnitOfWork.Repository<CAPMA>();
            var config = idRepo.DbSet.FirstOrDefault(x => x.LOAIMA == type && x.UNITCODE == unitCode);
            if (config == null)
            {
                config = new CAPMA
                {
                    ID = Guid.NewGuid().ToString(),
                    LOAIMA = type,
                    NHOMMA = "KHUYENMAI",
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

        public PagedObj<KHUYENMAI> QueryPageGiamGiaLoaiHang(string stringConnect, PagedObj<KHUYENMAI> page, string strKey, string maDonVi)
        {
            var TotalItem = 0;
            List<KHUYENMAI> ListKhuyenMai = new List<KHUYENMAI>();
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
                            command.CommandText = @"TIMKIEM_KHUYENMAI_PAGINATION";
                            command.Parameters.Add(@"P_MADONVI", OracleDbType.NVarchar2, 50).Value = maDonVi;
                            command.Parameters.Add(@"P_TUKHOA", OracleDbType.NVarchar2, 500).Value = strKey.ToString().ToUpper().Trim();
                            command.Parameters.Add(@"P_PAGENUMBER", OracleDbType.Int32).Value = page.CurrentPage;
                            command.Parameters.Add(@"P_PAGESIZE", OracleDbType.Int32).Value = page.ItemsPerPage;
                            command.Parameters.Add(@"P_LOAI_KHUYENMAI", OracleDbType.NVarchar2, 20).Value = TypeBuildCode.GGLOAIHANG.ToString();
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
                                            KHUYENMAI KhuyenMai = new KHUYENMAI();
                                            if (dataReader["ID"] != null)
                                            {
                                                KhuyenMai.ID = dataReader["ID"].ToString();
                                            }
                                            if (dataReader["MA_KHUYENMAI"] != null)
                                            {
                                                KhuyenMai.MA_KHUYENMAI = dataReader["MA_KHUYENMAI"].ToString();
                                            }
                                            if (dataReader["TUNGAY"] != DBNull.Value)
                                            {
                                                KhuyenMai.TUNGAY = DateTime.Parse(dataReader["TUNGAY"].ToString());
                                            }
                                            if (dataReader["DENNGAY"] != DBNull.Value)
                                            {
                                                KhuyenMai.DENNGAY = DateTime.Parse(dataReader["DENNGAY"].ToString());
                                            }
                                            if (dataReader["THOIGIAN_TAO"] != null)
                                            {
                                                KhuyenMai.THOIGIAN_TAO = dataReader["THOIGIAN_TAO"].ToString();
                                            }
                                            if (dataReader["TUGIO"] != null)
                                            {
                                                KhuyenMai.TUGIO = dataReader["TUGIO"].ToString();
                                            }
                                            if (dataReader["DENGIO"] != null)
                                            {
                                                KhuyenMai.DENGIO = dataReader["DENGIO"].ToString();
                                            }
                                            if (dataReader["MAKHO_KHUYENMAI"] != null)
                                            {
                                                KhuyenMai.MAKHO_KHUYENMAI = dataReader["MAKHO_KHUYENMAI"].ToString();
                                            }
                                            if (dataReader["DIENGIAI"] != null)
                                            {
                                                KhuyenMai.DIENGIAI = dataReader["DIENGIAI"].ToString();
                                            }
                                            if (dataReader["UNITCODE"] != null)
                                            {
                                                KhuyenMai.UNITCODE = dataReader["UNITCODE"].ToString();
                                            }
                                            if (dataReader["TRANGTHAI"] != DBNull.Value)
                                            {
                                                int TRANGTHAI = 0;
                                                int.TryParse(dataReader["TRANGTHAI"].ToString(), out TRANGTHAI);
                                                KhuyenMai.TRANGTHAI = TRANGTHAI;
                                            }
                                            if (dataReader["I_CREATE_DATE"] != DBNull.Value)
                                            {
                                                KhuyenMai.I_CREATE_DATE = DateTime.Parse(dataReader["I_CREATE_DATE"].ToString());
                                            }
                                            ListKhuyenMai.Add(KhuyenMai);
                                        }
                                    }
                                }
                            }
                            dataReader.Close();
                            page.Data = ListKhuyenMai;
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
                throw new Exception("Lỗi không thể truy xuất chương trình khuyến mãi");
            }
            return page;
        }

        public KHUYENMAI InsertKhuyenMai(GiamGiaLoaiHangViewModel.Dto instance)
        {
            var dataKhuyenMai = Mapper.Map<GiamGiaLoaiHangViewModel.Dto, KHUYENMAI>(instance);
            dataKhuyenMai.ID = Guid.NewGuid().ToString();
            dataKhuyenMai.LOAI_KHUYENMAI = TypeBuildCode.GGLOAIHANG.ToString();
            dataKhuyenMai.THOIGIAN_TAO = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
            dataKhuyenMai.I_STATE = "C";
            var result = AddUnit(dataKhuyenMai);
            List<KHUYENMAI_CHITIET> listDetails = new List<KHUYENMAI_CHITIET>();
            if(instance.DataDetails.Count > 0)
            {
                foreach(var detail in instance.DataDetails)
                {
                    KHUYENMAI_CHITIET khuyenMaiChiTiet = new KHUYENMAI_CHITIET();
                    khuyenMaiChiTiet.ID = Guid.NewGuid().ToString();
                    khuyenMaiChiTiet.MA_KHUYENMAI = dataKhuyenMai.MA_KHUYENMAI;
                    khuyenMaiChiTiet.MAHANG = detail.MALOAI;
                    khuyenMaiChiTiet.SOLUONG = detail.SOLUONG;
                    khuyenMaiChiTiet.GIATRI_KHUYENMAI = detail.GIATRI_KHUYENMAI;
                    khuyenMaiChiTiet.INDEX = detail.INDEX;
                    listDetails.Add(khuyenMaiChiTiet);
                }
            }
            result = Insert(result);
            UnitOfWork.Repository<KHUYENMAI_CHITIET>().InsertRange(listDetails);
            return result;
        }

        public KHUYENMAI UpdateKhuyenMai(GiamGiaLoaiHangViewModel.Dto instance)
        {
            var dataKhuyenMai = Mapper.Map<GiamGiaLoaiHangViewModel.Dto, KHUYENMAI>(instance);
            dataKhuyenMai.I_STATE = "U";
            dataKhuyenMai.UNITCODE = GetCurrentUnitCode();
            var listProduct = UnitOfWork.Repository<KHUYENMAI_CHITIET>().DbSet.Where(x => x.MA_KHUYENMAI == dataKhuyenMai.MA_KHUYENMAI).ToList();
            if (listProduct.Count > 0) listProduct.ForEach(x => x.ObjectState = ObjectState.Deleted);
            List<KHUYENMAI_CHITIET> listDetails = new List<KHUYENMAI_CHITIET>();
            if (instance.DataDetails.Count > 0)
            {
                foreach (var detail in instance.DataDetails)
                {
                    KHUYENMAI_CHITIET khuyenMaiChiTiet = new KHUYENMAI_CHITIET();
                    khuyenMaiChiTiet.ID = Guid.NewGuid().ToString();
                    khuyenMaiChiTiet.MA_KHUYENMAI = dataKhuyenMai.MA_KHUYENMAI;
                    khuyenMaiChiTiet.MAHANG = detail.MALOAI;
                    khuyenMaiChiTiet.SOLUONG = detail.SOLUONG;
                    khuyenMaiChiTiet.GIATRI_KHUYENMAI = detail.GIATRI_KHUYENMAI;
                    khuyenMaiChiTiet.INDEX = detail.INDEX;
                    listDetails.Add(khuyenMaiChiTiet);
                }
            }
            UnitOfWork.Repository<KHUYENMAI_CHITIET>().InsertRange(listDetails);
            var result = Update(dataKhuyenMai);
            return result;
        }

        public bool DeleteKhuyenMai(string ID)
        {
            bool result = true;
            var chungTu = UnitOfWork.Repository<KHUYENMAI>().DbSet.FirstOrDefault(x => x.ID.Equals(ID));
            if (chungTu == null)
            {
                result = false;
            }
            else
            {
                var chungTuChiTiet = UnitOfWork.Repository<KHUYENMAI_CHITIET>().DbSet.Where(x => x.MA_KHUYENMAI == chungTu.MA_KHUYENMAI).ToList();
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
    }
}
