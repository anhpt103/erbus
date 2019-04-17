using ERBus.Entity;
using ERBus.Entity.Database.Authorize;
using ERBus.Service.Service;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace ERBus.Service.Authorize.Menu
{
    public interface IMenuService : IDataInfoService<MENU>
    {
        List<MENU> GetAllForStarting(string username, string unitCode);
    }
    public class MenuService : DataInfoServiceBase<MENU>, IMenuService
    {
        public MenuService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        public List<MENU> GetAllForStarting(string username, string unitCode)
        {
            List<MENU> result = new List<MENU>();
            try
            {
                using (var ctx = new ERBusContext())
                {
                    using (var dbContextTransaction = ctx.Database.BeginTransaction())
                    {
                        try
                        {
                            var pUserName = new OracleParameter("P_USERNAME", OracleDbType.Varchar2, username, ParameterDirection.Input);
                            var pUnitCode = new OracleParameter("P_UNITCODE", OracleDbType.Varchar2, unitCode, ParameterDirection.Input);
                            var pReturnData = new OracleParameter("RETURN_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
                            var str = "BEGIN TBNETERP.GET_MENU(:P_USERNAME,:P_UNITCODE, :RETURN_DATA); END;";
                            ctx.Database.ExecuteSqlCommand(str, pUserName, pUnitCode, pReturnData);
                            OracleDataReader reader = ((OracleRefCursor)pReturnData.Value).GetDataReader();
                            while (reader.Read())
                            {
                                var item = new MENU()
                                {
                                    MA_MENU = reader["MA_MENU"].ToString(),
                                    SAPXEP = int.Parse(reader["SAPXEP"].ToString()),
                                    MENU_CHA = reader["MENU_CHA"].ToString(),
                                    TIEUDE = reader["TIEUDE"].ToString()
                                };
                                result.Add(item);

                            }
                            return result;
                        }
                        catch
                        {
                            throw new Exception("Xảy ra lỗi trong khi chạy Store_Procude. Tự động rollback!");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        protected override Expression<Func<MENU, bool>> GetKeyFilter(MENU instance)
        {
            return x => x.MA_MENU == instance.MA_MENU && x.UNITCODE.StartsWith("");
        }
    }
}
