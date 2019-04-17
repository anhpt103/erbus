using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using Oracle.ManagedDataAccess.Client;
using ERBus.Service;
using System.Configuration;
using ERBus.Service.Authorize.NguoiDungNhomQuyen;

namespace ERBus.Api.Controllers.Authorize
{
    [RoutePrefix("api/authorize/NguoiDungNhomQuyen")]
    [Route("{id?}")]
    [Authorize]
    public class NguoiDungNhomQuyenController : ApiController
    {
        [HttpGet]
        [Route("GetNhomQuyenByUsername/{userName}")]
        public IHttpActionResult GetNhomQuyenByUsername(string userName)
        {
            var result = new TransferObj<List<NguoiDungNhomQuyenViewModel.ViewModel>>();
            List<NguoiDungNhomQuyenViewModel.ViewModel> listNguoiDungNhom = new List<NguoiDungNhomQuyenViewModel.ViewModel>();
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
                                                                    NN.ID,
                                                                    NN.USERNAME,
                                                                    NN.MANHOMQUYEN,
                                                                    NQ.TENNHOMQUYEN
                                                                FROM
                                                                    NGUOIDUNG_NHOMQUYEN NN
                                                                    INNER JOIN NHOMQUYEN NQ ON NN.MANHOMQUYEN = NQ.MANHOMQUYEN
                                                                WHERE
                                                                    NN.USERNAME = :USERNAME");
                                command.Parameters.Add(@"USERNAME", OracleDbType.NVarchar2, 50).Value = userName;
                                OracleDataReader dataReader = command.ExecuteReader();
                                if (dataReader.HasRows)
                                {
                                    while (dataReader.Read())
                                    {
                                        NguoiDungNhomQuyenViewModel.ViewModel row = new NguoiDungNhomQuyenViewModel.ViewModel();
                                        if (dataReader["ID"] != null)
                                        {
                                            row.ID = dataReader["ID"].ToString();
                                        }
                                        if (dataReader["USERNAME"] != null)
                                        {
                                            row.USERNAME = dataReader["USERNAME"].ToString();
                                        }
                                        if (dataReader["MANHOMQUYEN"] != null)
                                        {
                                            row.MANHOMQUYEN = dataReader["MANHOMQUYEN"].ToString();
                                        }
                                        if (dataReader["TENNHOMQUYEN"] != null)
                                        {
                                            row.TENNHOMQUYEN = dataReader["TENNHOMQUYEN"].ToString();
                                        }
                                        listNguoiDungNhom.Add(row);
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
                    if(listNguoiDungNhom.Count > 0)
                    {
                        result.Status = true;
                        result.Data = listNguoiDungNhom;
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
        [Route("PostNguoiDungNhomQuyen")]
        public IHttpActionResult PostNguoiDungNhomQuyen(NguoiDungNhomQuyenViewModel.ConfigModel model)
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
                                        command.CommandText = string.Format(@"DELETE NGUOIDUNG_NHOMQUYEN WHERE ID = :ID");
                                        command.Parameters.Add(@"ID", OracleDbType.NVarchar2, 50).Value = item.ID;
                                        command.ExecuteNonQuery();
                                    }
                                }
                                if (model.LstAdd != null && model.LstAdd.Count > 0)
                                {
                                    foreach (var item in model.LstAdd)
                                    {
                                        command.Parameters.Clear();
                                        command.CommandText = string.Format(@"INSERT INTO NGUOIDUNG_NHOMQUYEN(ID,USERNAME,MANHOMQUYEN) VALUES (:ID,:USERNAME,:MANHOMQUYEN)");
                                        command.Parameters.Add(@"ID", OracleDbType.NVarchar2, 50).Value = Guid.NewGuid().ToString();
                                        command.Parameters.Add(@"USERNAME", OracleDbType.NVarchar2, 20).Value = item.USERNAME.Trim();
                                        command.Parameters.Add(@"MANHOMQUYEN", OracleDbType.NVarchar2, 50).Value = item.MANHOMQUYEN.Trim();
                                        command.ExecuteNonQuery();
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
