using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using System.Security.Claims;
using ERBus.Entity;
using System.Web.Configuration;
using System.Web;
using ERBus.Service.Catalog.MatHang;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Collections.Generic;
using ERBus.Entity.Database.Authorize;
using ERBus.Service.Authorize.KyKeToan;
using System.Configuration;

namespace ERBus.Service.Service
{
    public class DataInfoServiceBase<TEntity> : EntityServiceBase<TEntity>, IDataInfoService<TEntity>
        where TEntity : DataInfoEntity
    {
        public DataInfoServiceBase(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        public virtual IDataInfoService<TEntity> Include(Expression<Func<TEntity, object>> include)
        {
            Includes.Add(include);
            ((IQueryable<TEntity>) Repository.DbSet).Include(include);
            return this;
        }

        protected virtual Expression<Func<TEntity, bool>> GetIdFilter(string id)
        {
            return x => x.ID == id;
        }

        protected virtual Expression<Func<TEntity, bool>> GetKeyFilter(TEntity instance)
        {
            return x => x.ID == instance.ID;
        }

        public virtual TEntity Find(TEntity instance, bool notracking = false)
        {
            var result = (notracking ? Repository.DbSet.AsNoTracking() : Repository.DbSet)
                .FirstOrDefault(GetKeyFilter(instance));
            return result;
        }

        public virtual TEntity FindById(string id, bool notracking = false)
        {
            var result = (notracking ? Repository.DbSet.AsNoTracking() : Repository.DbSet)
                .FirstOrDefault(GetIdFilter(id));
            return result;
        }

        public string PhysicalPathTemplate()
        {
            return HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath + "Template/";
        }

        public string PhysicalPathUploadFile()
        {
            return HttpContext.Current.Server.MapPath("~/Upload") + "\\ImageMatHang\\";
        }

        public string PhysicalPathUploadLoaiPhong()
        {
            return HttpContext.Current.Server.MapPath("~/Upload") + "\\ImageLoaiPhong\\";
        }
        public virtual TEntity Insert(TEntity instance, bool withUnitCode = true)
        {
            var exist = Find(instance, true);
            if (exist != null)
            {
                throw new Exception("Tồn tại bản ghi có cùng mã!");
            }
            var newInstance = Mapper.DynamicMap<TEntity, TEntity>(instance);
            var currentUser = (HttpContext.Current.User as ClaimsPrincipal);
            newInstance.I_CREATE_DATE = DateTime.Now;
            newInstance.I_CREATE_BY = currentUser.Identity.Name;
            newInstance.ID = Guid.NewGuid().ToString();
            if (withUnitCode) { Repository.Insert(AddUnit(newInstance)); }
            else
            {
                newInstance.UNITCODE = instance.UNITCODE;
                Repository.Insert(newInstance);
            }
            return newInstance;
        }

        public virtual TEntity Update(TEntity instance,
           Action<TEntity, TEntity> updateAction = null,
           Func<TEntity, TEntity, bool> updateCondition = null)
        {
            Mapper.CreateMap<TEntity, TEntity>();
            var entity = Find(instance, false);
            if (entity == null || instance.ID != entity.ID)
            {
                throw new Exception("Bản ghi không tồn tại trong hệ thống");
            }
            var allowUpdate = updateCondition == null || updateCondition(
                instance, entity);
            if (allowUpdate)
            {
                if (updateAction == null)
                {
                    entity = Mapper.Map(instance, entity);
                    var currentUser = (HttpContext.Current.User as ClaimsPrincipal);
                    entity.I_UPDATE_DATE = DateTime.Now;
                    entity.I_UPDATE_BY = currentUser.Identity.Name;
                    entity.UNITCODE = GetCurrentUnitCode();
                }
                else
                {
                    updateAction(instance, entity);
                }
                entity.ObjectState = ObjectState.Modified;
            }
            return entity;
        }
        public virtual TEntity AddUnit(TEntity instance)
        {
            if (HttpContext.Current != null && HttpContext.Current.User is ClaimsPrincipal)
            {
                var currentUser = (HttpContext.Current.User as ClaimsPrincipal);
                var unit = currentUser.Claims.FirstOrDefault(x => x.Type == "unitCode");
                if (unit != null)
                {
                    instance.UNITCODE = unit.Value;
                    
                }  
            }          
            return instance;
        }
        
        public virtual string GetCurrentUnitCode()
        {
            if (HttpContext.Current != null && HttpContext.Current.User is ClaimsPrincipal)
            {
                var currentUser = (HttpContext.Current.User as ClaimsPrincipal);
                var unit = currentUser.Claims.FirstOrDefault(x => x.Type == "unitCode");
                if (unit != null) return unit.Value;
            }
            return "";
        }

        public virtual string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString;
        }

        public virtual List<MatHangViewModel.VIEW_MODEL> GetDataMatHang(string ListMatHang, string UnitCode, string StringConnect)
        {
            List<MatHangViewModel.VIEW_MODEL> result = new List<MatHangViewModel.VIEW_MODEL>();
            if (!string.IsNullOrEmpty(ListMatHang))
            {
                using (OracleConnection connection = new OracleConnection(StringConnect))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            OracleCommand command = new OracleCommand();
                            command.Connection = connection;
                            command.CommandType = CommandType.Text;
                            command.CommandText = @"SELECT a.MAHANG,a.TENHANG,a.MANHACUNGCAP,a.MATHUE_VAO,a.MATHUE_RA,a.MADONVITINH,
                            a.BARCODE,b.GIAMUA,b.GIAMUA_VAT,b.TYLE_LAILE,b.TYLE_LAIBUON,b.GIABANLE_VAT 
                            FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG AND a.UNITCODE = b.UNITCODE 
                            AND a.MAHANG IN ("+ ListMatHang.ToUpper() + ") AND a.UNITCODE = '"+ UnitCode + "'";
                            OracleDataReader dataReader = command.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    MatHangViewModel.VIEW_MODEL ViewModel = new MatHangViewModel.VIEW_MODEL();
                                    if (dataReader["MAHANG"] != null)
                                    {
                                        ViewModel.MAHANG = dataReader["MAHANG"].ToString();
                                    }
                                    if (dataReader["TENHANG"] != null)
                                    {
                                        ViewModel.TENHANG = dataReader["TENHANG"].ToString();
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
                                    decimal TYLE_LAILE = 0;
                                    if (dataReader["TYLE_LAILE"] != DBNull.Value)
                                    decimal.TryParse(dataReader["TYLE_LAILE"].ToString(), out TYLE_LAILE);
                                    ViewModel.TYLE_LAILE = TYLE_LAILE;

                                    decimal TYLE_LAIBUON = 0;
                                    if (dataReader["TYLE_LAIBUON"] != DBNull.Value)
                                    decimal.TryParse(dataReader["TYLE_LAIBUON"].ToString(), out TYLE_LAIBUON);
                                    ViewModel.TYLE_LAIBUON = TYLE_LAIBUON;

                                    decimal GIAMUA = 0;
                                    if (dataReader["GIAMUA"] != DBNull.Value)
                                    decimal.TryParse(dataReader["GIAMUA"].ToString(), out GIAMUA);
                                    ViewModel.GIAMUA = GIAMUA;

                                    decimal GIAMUA_VAT = 0;
                                    if (dataReader["GIAMUA_VAT"] != DBNull.Value)
                                    decimal.TryParse(dataReader["GIAMUA_VAT"].ToString(), out GIAMUA_VAT);
                                    ViewModel.GIAMUA_VAT = GIAMUA_VAT;

                                    decimal GIABANLE_VAT = 0;
                                    if (dataReader["GIABANLE_VAT"] != DBNull.Value)
                                    decimal.TryParse(dataReader["GIABANLE_VAT"].ToString(), out GIABANLE_VAT);
                                    ViewModel.GIABANLE_VAT = GIABANLE_VAT;

                                    result.Add(ViewModel);
                                }
                            }
                        }
                    }
                    catch
                    {
                        result = null;
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

        public virtual string ConvertConditionStringToArray(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            var subStrAray = str.Split(',');
            int length = subStrAray.Length;
            string[] resultArray = new string[length];
            for (int i = 0; i < length; i++)
            {
                resultArray[i] = "'" + subStrAray[i] + "'";
            }
            return String.Join(",", resultArray);
        }

        public virtual string Get_TableName_XNT(int Nam, int Ky)
        {
            return string.Format("XNT_{0}_KY_{1}", Nam, Ky);
        }

        public virtual string Get_TableName_XNT_Previous(int Nam, int Ky)
        {
            var previousPeriod = UnitOfWork.Repository<KYKETOAN>().DbSet.FirstOrDefault(x=>x.NAM  == Nam && x.KY == Ky);
            if(previousPeriod != null)
            {
                return string.Format("XNT_{0}_KY_{1}", Nam, previousPeriod.KY);
            }
            return null;
        }

        public virtual bool XuatNhapTon_TangPhieu(string TableName, int Nam, int Ky, string Id,string StringConnect)
        {
            bool result = false;
            using (OracleConnection connection = new OracleConnection(StringConnect))
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
                            command.CommandText = "ERBUS.XUATNHAPTON.XNT_TANG_PHIEU";
                            command.Parameters.Add("P_TABLENAME", OracleDbType.Varchar2,50, TableName, ParameterDirection.Input);
                            command.Parameters.Add("P_NAM", OracleDbType.Int32, Nam, ParameterDirection.Input);
                            command.Parameters.Add("P_KY", OracleDbType.Int32, Ky, ParameterDirection.Input);
                            command.Parameters.Add("P_ID", OracleDbType.Varchar2, 50, Id, ParameterDirection.Input);
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
            return result;
        }


        public virtual bool XuatNhapTon_GiamPhieu(string TableName, int Nam, int Ky, string Id, string StringConnect)
        {
            bool result = false;
            using (OracleConnection connection = new OracleConnection(StringConnect))
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
                            command.CommandText = "ERBUS.XUATNHAPTON.XNT_GIAM_PHIEU";
                            command.Parameters.Add("P_TABLENAME", OracleDbType.Varchar2, 50, TableName, ParameterDirection.Input);
                            command.Parameters.Add("P_NAM", OracleDbType.Int32, Nam, ParameterDirection.Input);
                            command.Parameters.Add("P_KY", OracleDbType.Int32, Ky, ParameterDirection.Input);
                            command.Parameters.Add("P_ID", OracleDbType.Varchar2, 50, Id, ParameterDirection.Input);
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
            return result;
        }

        public virtual bool KhoaSoNhieuKyKeToan(List<KyKeToanViewModel.ViewModel> listPeriod, string UnitCode, string StringConnect)
        {
            bool result = false;
            if(listPeriod.Count > 0)
            {
                using (OracleConnection connection = new OracleConnection(StringConnect))
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
                               
                                foreach (var kyKeToan in listPeriod)
                                {
                                    string PreviousTableName = Get_TableName_XNT_Previous(kyKeToan.NAM, kyKeToan.KY - 1);
                                    string CurrentTableName = Get_TableName_XNT(kyKeToan.NAM, kyKeToan.KY);
                                    command.Parameters.Clear();
                                    command.CommandType = CommandType.StoredProcedure;
                                    command.CommandText = "ERBUS.XUATNHAPTON.XNT_KHOASO";
                                    command.Parameters.Add("P_TABLENAME_KYTRUOC", OracleDbType.Varchar2, 15, PreviousTableName, ParameterDirection.Input);
                                    command.Parameters.Add("P_TABLENAME", OracleDbType.Varchar2, 15, CurrentTableName, ParameterDirection.Input);
                                    command.Parameters.Add("P_UNITCODE", OracleDbType.Varchar2, 10, UnitCode, ParameterDirection.Input);
                                    command.Parameters.Add("P_NAM", OracleDbType.Int32, kyKeToan.NAM, ParameterDirection.Input);
                                    command.Parameters.Add("P_KY", OracleDbType.Int32, kyKeToan.KY, ParameterDirection.Input);
                                    int countInsert = command.ExecuteNonQuery();
                                    command.Parameters.Clear();
                                    command.CommandType = CommandType.Text;
                                    command.CommandText = "UPDATE KYKETOAN SET TRANGTHAI = 10 WHERE KYKETOAN = :P_KY AND NAM = :P_NAM AND UNITCODE = :P_UNITCODE";
                                    command.Parameters.Add("P_KY", OracleDbType.Int32, kyKeToan.KY, ParameterDirection.Input);
                                    command.Parameters.Add("P_NAM", OracleDbType.Int32, kyKeToan.NAM, ParameterDirection.Input);
                                    command.Parameters.Add("P_UNITCODE", OracleDbType.Varchar2, 10, UnitCode, ParameterDirection.Input);
                                    command.ExecuteNonQuery();
                                }
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


        public virtual bool KhoaSoKyKeToan(string PreviousTableName, string CurrentTableName, string UnitCode, int Nam, int Ky, string StringConnect)
        {
            bool result = false;
            using (OracleConnection connection = new OracleConnection(StringConnect))
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
                            command.CommandText = "ERBUS.XUATNHAPTON.XNT_KHOASO";
                            command.Parameters.Add("P_TABLENAME_KYTRUOC", OracleDbType.Varchar2, 15, PreviousTableName, ParameterDirection.Input);
                            command.Parameters.Add("P_TABLENAME", OracleDbType.Varchar2, 15, CurrentTableName, ParameterDirection.Input);
                            command.Parameters.Add("P_UNITCODE", OracleDbType.Varchar2, 10, UnitCode, ParameterDirection.Input);
                            command.Parameters.Add("P_NAM", OracleDbType.Int32, Nam, ParameterDirection.Input);
                            command.Parameters.Add("P_KY", OracleDbType.Int32, Ky, ParameterDirection.Input);
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
            return result;
        }


        public virtual List<KyKeToanViewModel.ViewModel> KyKeToanChuaKhoa(string UnitCode, string StringConnect)
        {
            List<KyKeToanViewModel.ViewModel> result = new List<KyKeToanViewModel.ViewModel>();
            using (OracleConnection connection = new OracleConnection(StringConnect))
            {
                try
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        OracleCommand command = new OracleCommand();
                        command.Connection = connection;
                        command.CommandType = CommandType.Text;
                        command.CommandText = string.Format(@"SELECT KYKETOAN AS KYCHUAKHOA,NAM,TUNGAY AS NGAYKETOAN FROM KYKETOAN WHERE
                        TO_DATE(DENNGAY,'DD-MM-YY') <= TO_DATE(SYSDATE,'DD-MM-YY') AND TO_DATE(TUNGAY,'DD-MM-YY') > TO_DATE((SELECT MAX(TUNGAY) AS NGAYKETOAN 
                        FROM KYKETOAN WHERE TRANGTHAI = " + (int)TypeState.APPROVAL + " AND NAM = (SELECT MAX(NAM) FROM KYKETOAN) AND UNITCODE = '" + UnitCode + "' GROUP BY NAM),'DD-MM-YY') ORDER BY KYKETOAN");
                        OracleDataReader dataReader = command.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                KyKeToanViewModel.ViewModel row = new KyKeToanViewModel.ViewModel();
                                int KYCHUAKHOA = 0;
                                int.TryParse(dataReader["KYCHUAKHOA"].ToString(), out KYCHUAKHOA);
                                row.KY = KYCHUAKHOA;
                                int NAM = 0;
                                int.TryParse(dataReader["NAM"].ToString(), out NAM);
                                row.NAM = NAM;
                                if(dataReader["NGAYKETOAN"] != DBNull.Value)
                                {
                                    row.NGAYKETOAN = DateTime.Parse(dataReader["NGAYKETOAN"].ToString());
                                }
                                result.Add(row);
                            }
                            dataReader.Close();
                        }
                        else
                        {
                            result = null;
                        }
                    }
                }
                catch
                {
                    result = null;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
            return result;
        }


        public virtual KyKeToanViewModel.ViewModel GetLastestPeriod(string UnitCode, string StringConnect)
        {
            KyKeToanViewModel.ViewModel result = new KyKeToanViewModel.ViewModel();
            using (OracleConnection connection = new OracleConnection(StringConnect))
            {
                try
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        OracleCommand command = new OracleCommand();
                        command.Connection = connection;
                        command.CommandType = CommandType.Text;
                        command.CommandText = @"SELECT MAX(KYKETOAN) AS KY, NAM, MAX(TUNGAY) AS NGAYKETOAN FROM KYKETOAN WHERE TRANGTHAI = " + (int)TypeState.APPROVAL + " AND NAM = (SELECT MAX(NAM) FROM KYKETOAN) AND UNITCODE = '" + UnitCode +"' GROUP BY NAM";
                        OracleDataReader dataReader = command.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                if (dataReader["KY"] != null)
                                {
                                    int KY = 0;
                                    int.TryParse(dataReader["KY"].ToString(), out KY);
                                    result.KY = KY;

                                    int NAM = 0;
                                    int.TryParse(dataReader["NAM"].ToString(), out NAM);
                                    result.NAM = NAM;

                                    DateTime NGAYKETOAN = DateTime.Now;
                                    DateTime.TryParse(dataReader["NGAYKETOAN"].ToString(), out NGAYKETOAN);
                                    result.NGAYKETOAN = NGAYKETOAN;
                                }
                            }
                            dataReader.Close();
                        }
                        else
                        {
                            result = null;
                        }
                    }
                }
                catch
                {
                    result = null;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
            return result;
        }


        public virtual KyKeToanViewModel.ViewModel GetTableXuatNhapTonTheoNgay(DateTime date, string UnitCode, string StringConnect)
        {
            KyKeToanViewModel.ViewModel result = new KyKeToanViewModel.ViewModel();
            using (OracleConnection connection = new OracleConnection(StringConnect))
            {
                try
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        OracleCommand command = new OracleCommand();
                        command.Connection = connection;
                        command.CommandType = CommandType.Text;
                        command.CommandText = @"SELECT KYKETOAN AS KY, NAM, TUNGAY AS NGAYKETOAN FROM KYKETOAN WHERE TO_DATE(TUNGAY,'DD-MM-YY') = TO_DATE('"+ date.ToString("dd-MMM-yy") + "','DD-MM-YY') AND TRANGTHAI = " + (int)TypeState.APPROVAL + " AND NAM = (SELECT MAX(NAM) FROM KYKETOAN) AND UNITCODE = '" + UnitCode + "'";
                        OracleDataReader dataReader = command.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                if (dataReader["KY"] != null)
                                {
                                    int KY = 0;
                                    int.TryParse(dataReader["KY"].ToString(), out KY);
                                    result.KY = KY;

                                    int NAM = 0;
                                    int.TryParse(dataReader["NAM"].ToString(), out NAM);
                                    result.NAM = NAM;

                                    DateTime NGAYKETOAN = DateTime.Now;
                                    DateTime.TryParse(dataReader["NGAYKETOAN"].ToString(), out NGAYKETOAN);
                                    result.NGAYKETOAN = NGAYKETOAN;
                                }
                            }
                            dataReader.Close();
                        }
                        else
                        {
                            result = null;
                        }
                    }
                }
                catch
                {
                    result = null;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
            return result;
        }

        public virtual ClaimsPrincipal GetClaimsPrincipal()
        {
            var currentClaims = new ClaimsPrincipal();
            if (HttpContext.Current != null && HttpContext.Current.User is ClaimsPrincipal)
            {
                currentClaims = (HttpContext.Current.User as ClaimsPrincipal);
            }
            return currentClaims;
        }
       
        public virtual TEntity Delete(string id)
        {
            var entity = FindById(id, false);
            if (entity == null)
            {
                throw new Exception("Bản ghi không tồn tại trong hệ thống");
            }
            entity.ObjectState = ObjectState.Deleted;
            return entity;
        }
    }
}