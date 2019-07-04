using ERBus.Entity;
using ERBus.Entity.Database.Authorize;
using ERBus.Service;
using ERBus.Service.Authorize.Menu;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.Http;

namespace ERBus.Api.Controllers.Authorize
{
    [RoutePrefix("api/Authorize/Menu")]
    [Route("{id?}")]
    [Authorize]
    public class MenuController : ApiController
    {
        private IMenuService _service;
        public MenuController(IMenuService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("GetMenu/{username}/{unitCode}")]
        public IHttpActionResult GetMenu(string username, string unitCode)
        {
            var result = new TransferObj<List<ChoiceObject>>();
            try
            {
                List<MENU> lstMenu = new List<MENU>();
                if (username.Equals("admin"))
                {
                    lstMenu = _service.Repository.DbSet.Where(x => x.TRANGTHAI == (int)TypeState.USED && x.UNITCODE == unitCode).OrderBy(x => x.SAPXEP).ToList();
                }
                else
                {
                    lstMenu = _service.GetAllForStarting(username, unitCode);
                }
                result.Data = new List<ChoiceObject>();
                if (lstMenu != null)
                {
                    lstMenu.ForEach(x =>
                    {
                        ChoiceObject obj = new ChoiceObject()
                        {
                            ID = x.ID,
                            TEXT = x.TIEUDE,
                            PARENT = x.MENU_CHA,
                            VALUE = x.MA_MENU,
                            EXTEND_VALUE = x.ICON
                        };
                        result.Data.Add(obj);
                    });
                }
                if (result.Data.Count > 0)
                {
                    result.Status = true;
                    result.Message = "Success";
                }
                else
                {
                    result.Status = false;
                    result.Message = "NoData";
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
                result.Data = null;
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("GetAllNhomQuyenMenu/{maNhomQuyen}")]
        public IHttpActionResult GetAllNhomQuyenMenu(string maNhomQuyen)
        {
            var result = new TransferObj<List<MenuViewModel.Dto>>();
            List<MenuViewModel.Dto> listNhomQuyenMenu = new List<MenuViewModel.Dto>();
            if (!string.IsNullOrEmpty(maNhomQuyen))
            {
                try
                {
                    string stringConnect = ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString;
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
                                command.CommandText = string.Format(@"SELECT
                                                                    B.MENU_CHA,
                                                                    B.MA_MENU,
                                                                    B.TIEUDE,
                                                                    B.TRANGTHAI,
                                                                    B.SAPXEP
                                                                FROM
                                                                    MENU B
                                                                WHERE
                                                                    B.MENU_CHA NOT IN (
                                                                        SELECT
                                                                            A.MA_MENU
                                                                        FROM
                                                                            NHOMQUYEN_MENU A
                                                                        WHERE
                                                                            A.MANHOMQUYEN = :MANHOMQUYEN
                                                                    )
                                                                    AND B.TRANGTHAI = 10
                                                                ORDER BY
                                                                    B.SAPXEP");
                                command.Parameters.Add(@"MANHOMQUYEN", OracleDbType.NVarchar2, 50).Value = maNhomQuyen;
                                OracleDataReader dataReader = command.ExecuteReader();
                                if (dataReader.HasRows)
                                {
                                    while (dataReader.Read())
                                    {
                                        MenuViewModel.Dto row = new MenuViewModel.Dto();
                                        if (dataReader["MENU_CHA"] != null)
                                        {
                                            row.MENU_CHA = dataReader["MENU_CHA"].ToString();
                                        }
                                        if (dataReader["MA_MENU"] != null)
                                        {
                                            row.MA_MENU = dataReader["MA_MENU"].ToString();
                                        }
                                        if (dataReader["TIEUDE"] != null)
                                        {
                                            row.TIEUDE = dataReader["TIEUDE"].ToString();
                                        }
                                        if (dataReader["SAPXEP"] != DBNull.Value)
                                        {
                                            row.SAPXEP = int.Parse(dataReader["SAPXEP"].ToString());
                                        }
                                        listNhomQuyenMenu.Add(row);
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
                    if (listNhomQuyenMenu.Count > 0)
                    {
                        result.Status = true;
                        result.Data = listNhomQuyenMenu;
                    }
                    else
                    {
                        result.Status = false;
                        result.Data = null;
                    }
                }
                catch (Exception ex)
                {
                    result.Status = false;
                    result.Message = ex.Message;
                }
            }
            return Ok(result);
        }



        [HttpGet]
        [Route("GetAllQuyenMenu/{userName}")]
        public IHttpActionResult GetAllQuyenMenu(string userName)
        {
            var result = new TransferObj<List<MenuViewModel.Dto>>();
            List<MenuViewModel.Dto> listNhomQuyenMenu = new List<MenuViewModel.Dto>();
            if (!string.IsNullOrEmpty(userName))
            {
                try
                {
                    string stringConnect = ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString;
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
                                command.CommandText = string.Format(@"SELECT
                                                                    A.MENU_CHA,
                                                                    A.MA_MENU,
                                                                    A.TIEUDE,
                                                                    A.SAPXEP,
                                                                    A.TRANGTHAI
                                                                FROM
                                                                    MENU A
                                                                WHERE
                                                                    TRANGTHAI = 10
                                                                    AND MA_MENU NOT IN (
                                                                        SELECT
                                                                            MA_MENU
                                                                        FROM
                                                                            NGUOIDUNG_MENU
                                                                        WHERE
                                                                            USERNAME = :USERNAME
                                                                    )
                                                                ORDER BY
                                                                    A.SAPXEP");
                                command.Parameters.Add(@"USERNAME", OracleDbType.NVarchar2, 20).Value = userName;
                                OracleDataReader dataReader = command.ExecuteReader();
                                if (dataReader.HasRows)
                                {
                                    while (dataReader.Read())
                                    {
                                        MenuViewModel.Dto row = new MenuViewModel.Dto();
                                        if (dataReader["MENU_CHA"] != null)
                                        {
                                            row.MENU_CHA = dataReader["MENU_CHA"].ToString();
                                        }
                                        if (dataReader["MA_MENU"] != null)
                                        {
                                            row.MA_MENU = dataReader["MA_MENU"].ToString();
                                        }
                                        if (dataReader["TIEUDE"] != null)
                                        {
                                            row.TIEUDE = dataReader["TIEUDE"].ToString();
                                        }
                                        if (dataReader["SAPXEP"] != DBNull.Value)
                                        {
                                            row.SAPXEP = int.Parse(dataReader["SAPXEP"].ToString());
                                        }
                                        listNhomQuyenMenu.Add(row);
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
                    if (listNhomQuyenMenu.Count > 0)
                    {
                        result.Status = true;
                        result.Data = listNhomQuyenMenu;
                    }
                    else
                    {
                        result.Status = false;
                        result.Data = null;
                    }
                }
                catch (Exception ex)
                {
                    result.Status = false;
                    result.Message = ex.Message;
                }
            }
            return Ok(result);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _service.Repository.DataContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}