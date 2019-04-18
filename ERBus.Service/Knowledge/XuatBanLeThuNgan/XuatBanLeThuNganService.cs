using ERBus.Entity.Database.Knowledge;
using ERBus.Service.BuildQuery;
using ERBus.Service.Service;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
namespace ERBus.Service.Knowledge.XuatBanLeThuNgan
{
    public interface IXuatBanLeThuNganService : IDataInfoService<GIAODICH>
    {
        PagedObj<XuatBanLeThuNganViewModel.View> QueryPageGiaoDich(string stringConnect, PagedObj<XuatBanLeThuNganViewModel.View> page, string strKey, string maDonVi);
    }
    public class XuatBanLeThuNganService : DataInfoServiceBase<GIAODICH>, IXuatBanLeThuNganService
    {
        public XuatBanLeThuNganService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        protected override Expression<Func<GIAODICH, bool>> GetKeyFilter(GIAODICH instance)
        {
            var unitCode = GetCurrentUnitCode();
            return x => x.MA_GIAODICH == instance.MA_GIAODICH && x.UNITCODE.Equals(unitCode);
        }

        public PagedObj<XuatBanLeThuNganViewModel.View> QueryPageGiaoDich(string stringConnect, PagedObj<XuatBanLeThuNganViewModel.View> page, string strKey, string maDonVi)
        {
            var TotalItem = 0;
            List<XuatBanLeThuNganViewModel.View> ListGiaoDich = new List<XuatBanLeThuNganViewModel.View>();
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
                            command.CommandText = @"TIMKIEM_GIAODICH_PAGINATION";
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
                                            XuatBanLeThuNganViewModel.View GiaoDich = new XuatBanLeThuNganViewModel.View();
                                            if (dataReader["ID"] != null)
                                            {
                                                GiaoDich.ID = dataReader["ID"].ToString();
                                            }
                                            if (dataReader["MA_GIAODICH"] != null)
                                            {
                                                GiaoDich.MA_GIAODICH = dataReader["MA_GIAODICH"].ToString();
                                            }
                                            if (dataReader["LOAI_GIAODICH"] != null)
                                            {
                                                GiaoDich.LOAI_GIAODICH = dataReader["LOAI_GIAODICH"].ToString();
                                            }
                                            if (dataReader["NGAY_GIAODICH"] != DBNull.Value)
                                            {
                                                GiaoDich.NGAY_GIAODICH = DateTime.Parse(dataReader["NGAY_GIAODICH"].ToString());
                                            }
                                            if (dataReader["MAKHACHHANG"] != null)
                                            {
                                                GiaoDich.MAKHACHHANG = dataReader["MAKHACHHANG"].ToString();
                                            }
                                            if (dataReader["THOIGIAN_TAO"] != null)
                                            {
                                                GiaoDich.THOIGIAN_TAO = dataReader["THOIGIAN_TAO"].ToString();
                                            }
                                            if (dataReader["TIENKHACH_TRA"] != DBNull.Value)
                                            {
                                                GiaoDich.TIENKHACH_TRA = decimal.Parse(dataReader["TIENKHACH_TRA"].ToString());
                                            }
                                            if (dataReader["TIEN_TRALAI_KHACH"] != DBNull.Value)
                                            {
                                                GiaoDich.TIEN_TRALAI_KHACH = decimal.Parse(dataReader["TIEN_TRALAI_KHACH"].ToString());
                                            }
                                            if (dataReader["MAKHO_XUAT"] != null)
                                            {
                                                GiaoDich.MAKHO_XUAT = dataReader["MAKHO_XUAT"].ToString();
                                            }
                                            if (dataReader["DIENGIAI"] != null)
                                            {
                                                GiaoDich.MAKHACHHANG = dataReader["DIENGIAI"].ToString();
                                            }
                                            ListGiaoDich.Add(GiaoDich);
                                        }
                                    }
                                }
                            }
                            dataReader.Close();
                            page.Data = ListGiaoDich;
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
    }
}
