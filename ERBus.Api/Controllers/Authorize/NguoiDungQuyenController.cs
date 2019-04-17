using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using Oracle.ManagedDataAccess.Client;
using ERBus.Service;
using System.Configuration;
using ERBus.Service.Authorize.NguoiDungNhomQuyen;
using ERBus.Entity.Database.Authorize;
using ERBus.Entity;

namespace ERBus.Api.Controllers.Authorize
{
    [RoutePrefix("api/authorize/NguoiDungQuyen")]
    [Route("{id?}")]
    [Authorize]
    public class NguoiDungQuyenController : ApiController
    {
        [HttpGet]
        [Route("GetAllQuyenByUsername/{userName}")]
        public IHttpActionResult GetAllQuyenByUsername(string userName)
        {
            var result = new TransferObj<List<NguoiDungQuyenViewModel.ViewModel>>();
            List<NguoiDungQuyenViewModel.ViewModel> listNguoiDungQuyen = new List<NguoiDungQuyenViewModel.ViewModel>();
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
                                                                    A.ID,
                                                                    A.USERNAME,
                                                                    A.MA_MENU,
                                                                    B.TIEUDE,
                                                                    B.SAPXEP,
                                                                    A.XEM,
                                                                    A.THEM,
                                                                    A.SUA,
                                                                    A.XOA,
                                                                    A.DUYET,
                                                                    A.GIAMUA,
                                                                    A.GIABAN,
                                                                    A.GIAVON,
                                                                    A.TYLELAI,
                                                                    A.BANCHIETKHAU,
                                                                    A.BANBUON,
                                                                    A.BANTRALAI
                                                                FROM
                                                                    NGUOIDUNG_MENU A
                                                                    INNER JOIN MENU B ON A.MA_MENU = B.MA_MENU
                                                                WHERE
                                                                    A.USERNAME = :USERNAME");
                                command.Parameters.Add(@"USERNAME", OracleDbType.NVarchar2, 50).Value = userName;
                                OracleDataReader dataReader = command.ExecuteReader();
                                if (dataReader.HasRows)
                                {
                                    while (dataReader.Read())
                                    {
                                        NguoiDungQuyenViewModel.ViewModel row = new NguoiDungQuyenViewModel.ViewModel();
                                        if (dataReader["ID"] != null)
                                        {
                                            row.ID = dataReader["ID"].ToString();
                                        }
                                        if (dataReader["USERNAME"] != null)
                                        {
                                            row.USERNAME = dataReader["USERNAME"].ToString();
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
                                        if (dataReader["XEM"] != DBNull.Value)
                                        {
                                            row.XEM = int.Parse(dataReader["XEM"].ToString()) == 1 ? true : false;
                                        }
                                        if (dataReader["THEM"] != DBNull.Value)
                                        {
                                            row.THEM = int.Parse(dataReader["THEM"].ToString()) == 1 ? true : false;
                                        }
                                        if (dataReader["SUA"] != DBNull.Value)
                                        {
                                            row.SUA = int.Parse(dataReader["SUA"].ToString()) == 1 ? true : false;
                                        }
                                        if (dataReader["XOA"] != DBNull.Value)
                                        {
                                            row.XOA = int.Parse(dataReader["XOA"].ToString()) == 1 ? true : false;
                                        }
                                        if (dataReader["DUYET"] != DBNull.Value)
                                        {
                                            row.DUYET = int.Parse(dataReader["DUYET"].ToString()) == 1 ? true : false;
                                        }
                                        if (dataReader["GIAMUA"] != DBNull.Value)
                                        {
                                            row.GIAMUA = int.Parse(dataReader["GIAMUA"].ToString()) == 1 ? true : false;
                                        }
                                        if (dataReader["GIABAN"] != DBNull.Value)
                                        {
                                            row.GIABAN = int.Parse(dataReader["GIABAN"].ToString()) == 1 ? true : false;
                                        }
                                        if (dataReader["GIAVON"] != DBNull.Value)
                                        {
                                            row.GIAVON = int.Parse(dataReader["GIAVON"].ToString()) == 1 ? true : false;
                                        }
                                        if (dataReader["TYLELAI"] != DBNull.Value)
                                        {
                                            row.TYLELAI = int.Parse(dataReader["TYLELAI"].ToString()) == 1 ? true : false;
                                        }
                                        if (dataReader["BANCHIETKHAU"] != DBNull.Value)
                                        {
                                            row.BANCHIETKHAU = int.Parse(dataReader["BANCHIETKHAU"].ToString()) == 1 ? true : false;
                                        }
                                        if (dataReader["BANBUON"] != DBNull.Value)
                                        {
                                            row.BANBUON = int.Parse(dataReader["BANBUON"].ToString()) == 1 ? true : false;
                                        }
                                        if (dataReader["BANTRALAI"] != DBNull.Value)
                                        {
                                            row.BANTRALAI = int.Parse(dataReader["BANTRALAI"].ToString()) == 1 ? true : false;
                                        }
                                        listNguoiDungQuyen.Add(row);
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
                    if(listNguoiDungQuyen.Count > 0)
                    {
                        result.Status = true;
                        result.Data = listNguoiDungQuyen;
                        result.Message = "Thành công";
                    }
                    else
                    {
                        result.Status = false;
                        result.Data = null;
                        result.Message = "Không có dữ liệu";
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

        [HttpPost]
        [Route("PostNguoiDungQuyen")]
        public IHttpActionResult PostNguoiDungQuyen(NguoiDungQuyenViewModel.ConfigModel model)
        {
            var result = new TransferObj<bool>();
            if (!string.IsNullOrEmpty(model.USERNAME))
            {
                string stringConnect = ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString;
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
                                if (model.LstDelete != null && model.LstDelete.Count > 0)
                                {
                                    foreach (var item in model.LstDelete)
                                    {
                                        command.Parameters.Clear();
                                        command.CommandType = CommandType.Text;
                                        command.CommandText = string.Format(@"DELETE NGUOIDUNG_MENU WHERE ID = :ID");
                                        command.Parameters.Add(@"ID", OracleDbType.NVarchar2, 50).Value = item.ID;
                                        int countDelete = command.ExecuteNonQuery();
                                    }
                                }

                                if (model.LstAdd != null && model.LstAdd.Count > 0)
                                {
                                    foreach (var item in model.LstAdd)
                                    {
                                        command.Parameters.Clear();
                                        command.CommandType = CommandType.Text;
                                        command.CommandText = string.Format(@"INSERT INTO NGUOIDUNG_MENU(ID,MA_MENU,USERNAME,XEM,THEM,SUA,XOA,DUYET,GIAMUA,GIABAN,GIAVON,TYLELAI,BANCHIETKHAU,BANBUON,BANTRALAI) VALUES (:ID,:MA_MENU,:USERNAME,:XEM,:THEM,:SUA,:XOA,:DUYET,:GIAMUA,:GIABAN,:GIAVON,:TYLELAI,:BANCHIETKHAU,:BANBUON,:BANTRALAI)");
                                        command.Parameters.Add(@"ID", OracleDbType.NVarchar2, 50).Value = Guid.NewGuid().ToString();
                                        command.Parameters.Add(@"MA_MENU", OracleDbType.NVarchar2, 20).Value = item.MA_MENU.Trim();
                                        command.Parameters.Add(@"USERNAME", OracleDbType.NVarchar2, 50).Value = item.USERNAME.Trim();
                                        command.Parameters.Add(@"XEM", OracleDbType.Int32).Value = item.XEM;
                                        command.Parameters.Add(@"THEM", OracleDbType.Int32).Value = item.THEM;
                                        command.Parameters.Add(@"SUA", OracleDbType.Int32).Value = item.SUA;
                                        command.Parameters.Add(@"XOA", OracleDbType.Int32).Value = item.XOA;
                                        command.Parameters.Add(@"DUYET", OracleDbType.Int32).Value = item.DUYET;
                                        command.Parameters.Add(@"GIAMUA", OracleDbType.Int32).Value = item.GIAMUA;
                                        command.Parameters.Add(@"GIABAN", OracleDbType.Int32).Value = item.GIABAN;
                                        command.Parameters.Add(@"GIAVON", OracleDbType.Int32).Value = item.GIAVON;
                                        command.Parameters.Add(@"TYLELAI", OracleDbType.Int32).Value = item.TYLELAI;
                                        command.Parameters.Add(@"BANCHIETKHAU", OracleDbType.Int32).Value = item.BANCHIETKHAU;
                                        command.Parameters.Add(@"BANBUON", OracleDbType.Int32).Value = item.BANBUON;
                                        command.Parameters.Add(@"BANTRALAI", OracleDbType.Int32).Value = item.BANTRALAI;
                                        int countInsert = command.ExecuteNonQuery();
                                    }
                                }

                                if (model.LstEdit != null && model.LstEdit.Count > 0)
                                {
                                    foreach (var item in model.LstEdit)
                                    {
                                        command.Parameters.Clear();
                                        command.BindByName = true;
                                        command.CommandType = CommandType.Text;
                                        command.CommandText = string.Format(@"UPDATE NGUOIDUNG_MENU SET MA_MENU = :MA_MENU,USERNAME = :USERNAME,XEM = :XEM,THEM = :THEM,SUA = :SUA,XOA = :XOA,DUYET = :DUYET,GIAMUA = :GIAMUA,GIABAN = :GIABAN,GIAVON = :GIAVON,TYLELAI = :TYLELAI,BANCHIETKHAU = :BANCHIETKHAU,BANBUON = :BANBUON,BANTRALAI = :BANTRALAI WHERE ID = :ID");
                                        command.Parameters.Add(@"ID", OracleDbType.NVarchar2, 50).Value = item.ID;
                                        command.Parameters.Add(@"MA_MENU", OracleDbType.NVarchar2, 20).Value = item.MA_MENU.Trim();
                                        command.Parameters.Add(@"USERNAME", OracleDbType.NVarchar2, 50).Value = item.USERNAME.Trim();
                                        command.Parameters.Add(@"XEM", OracleDbType.Char).Value = item.XEM ? '1' : '0';
                                        command.Parameters.Add(@"THEM", OracleDbType.Char).Value = item.THEM ? '1' : '0';
                                        command.Parameters.Add(@"SUA", OracleDbType.Char).Value = item.SUA ? '1' : '0';
                                        command.Parameters.Add(@"XOA", OracleDbType.Char).Value = item.XOA ? '1' : '0';
                                        command.Parameters.Add(@"DUYET", OracleDbType.Char).Value = item.DUYET ? '1' : '0';
                                        command.Parameters.Add(@"GIAMUA", OracleDbType.Char).Value = item.GIAMUA ? '1' : '0';
                                        command.Parameters.Add(@"GIABAN", OracleDbType.Char).Value = item.GIABAN ? '1' : '0';
                                        command.Parameters.Add(@"GIAVON", OracleDbType.Char).Value = item.GIAVON ? '1' : '0';
                                        command.Parameters.Add(@"TYLELAI", OracleDbType.Char).Value = item.TYLELAI ? '1' : '0';
                                        command.Parameters.Add(@"BANCHIETKHAU", OracleDbType.Char).Value = item.BANCHIETKHAU ? '1' : '0';
                                        command.Parameters.Add(@"BANBUON", OracleDbType.Char).Value = item.BANBUON ? '1' : '0';
                                        command.Parameters.Add(@"BANTRALAI", OracleDbType.Char).Value = item.BANTRALAI ? '1' : '0';
                                        int countUpdate = command.ExecuteNonQuery();
                                    }
                                }
                                transaction.Commit();
                                result.Data = true;
                                result.Status = true;
                                result.Message = "Cập nhật thành công";
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                result.Data = false;
                                result.Status = false;
                                result.Message = "Cập nhật không thành công";
                            }
                            finally
                            {
                                transaction.Dispose();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        result.Data = false;
                        result.Status = false;
                        result.Message = "Xảy ra lỗi";
                    }
                    finally
                    {
                        connection.Close();
                        connection.Dispose();
                    }
                }
            };
            return Ok(result);
        }
    }
}
