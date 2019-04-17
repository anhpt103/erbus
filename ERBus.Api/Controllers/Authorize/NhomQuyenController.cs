using AutoMapper;
using ERBus.Service;
using ERBus.Service.Authorize.Utils;
using ERBus.Service.BuildQuery;
using ERBus.Service.BuildQuery.Query.Types;
using ERBus.Service.Authorize.NhomQuyen;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ERBus.Entity.Database.Authorize;
using System.Configuration;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace ERBus.Api.Controllers.Authorize
{
    [RoutePrefix("api/Authorize/NhomQuyen")]
    [Route("{id?}")]
    [Authorize]
    public class NhomQuyenController : ApiController
    {
        private INhomQuyenService _service;
        public NhomQuyenController(INhomQuyenService service)
        {
            _service = service;
        }
        [Route("GetAllData")]
        [HttpGet]
        [CustomAuthorize(Method = "XEM", State = "NhomQuyen")]
        public IHttpActionResult GetAllData()
        {
            var result = new TransferObj<List<ChoiceObject>>();
            var unitCode = _service.GetCurrentUnitCode();
            result.Data = _service.Repository.DbSet.Where(x => x.UNITCODE.Equals(unitCode)).OrderBy(x => x.MANHOMQUYEN).Select(x => new ChoiceObject { VALUE = x.MANHOMQUYEN, TEXT = x.MANHOMQUYEN + " | " + x.TENNHOMQUYEN, DESCRIPTION = x.TENNHOMQUYEN, EXTEND_VALUE = x.UNITCODE, ID = x.ID }).ToList();
            if (result.Data.Count > 0)
            {
                result.Status = true;
            }
            else
            {
                result.Status = false;
            }
            return Ok(result);
        }

        [Route("PostQuery")]
        [HttpPost]
        [CustomAuthorize(Method = "XEM", State = "NhomQuyen")]
        public IHttpActionResult PostQuery(JObject jsonData)
        {
            var result = new TransferObj();
            var postData = ((dynamic)jsonData);
            var filtered = ((JObject)postData.filtered).ToObject<FilterObj<NhomQuyenViewModel.Search>>();
            var paged = ((JObject)postData.paged).ToObject<PagedObj<NHOMQUYEN>>();
            var unitCode = _service.GetCurrentUnitCode();
            var query = new QueryBuilder
            {
                Take = paged.ItemsPerPage,
                Skip = paged.FromItem - 1,
                Filter = new QueryFilterLinQ()
                {
                    Property = ClassHelper.GetProperty(() => new NHOMQUYEN().UNITCODE),
                    Method = FilterMethod.EqualTo,
                    Value = unitCode
                },
                Orders = new List<IQueryOrder>()
                {
                    new QueryOrder()
                    {
                        Field = ClassHelper.GetPropertyName(() => new NHOMQUYEN().MANHOMQUYEN),
                        Method = OrderMethod.ASC
                    }
                }
            };
            try
            {
                var filterResult = _service.Filter(filtered, query);
                if (!filterResult.WasSuccessful)
                {
                    return NotFound();
                }
                result.Data = filterResult.Value;
                result.Status = true;
                return Ok(result);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [Route("BuildNewCode")]
        [HttpGet]
        public string BuildNewCode()
        {
            return _service.BuildCode();
        }

        [ResponseType(typeof(NHOMQUYEN))]
        [CustomAuthorize(Method = "THEM", State = "NhomQuyen")]
        public async Task<IHttpActionResult> Post (NhomQuyenViewModel.Dto instance)
        {
            var result = new TransferObj<NHOMQUYEN>();
            var curentUnitCode = _service.GetCurrentUnitCode();
            if (instance.MANHOMQUYEN == "")
            {
                result.Status = false;
                result.Message = "Mã không hợp lệ";
                return Ok(result);
            }
            else
            {
                var exist = _service.Repository.DbSet.FirstOrDefault(x => x.MANHOMQUYEN == instance.MANHOMQUYEN && x.UNITCODE.Equals(curentUnitCode));
                if (exist != null)
                {
                    result.Status = false;
                    result.Message = "Đã tồn tại mã khách hàng này";
                    return Ok(result);
                }
            }
            try
            {
                instance.MANHOMQUYEN = _service.SaveCode();
                var data = Mapper.Map<NhomQuyenViewModel.Dto, NHOMQUYEN>(instance);
                var item = _service.Insert(data);
                int inst = await _service.UnitOfWork.SaveAsync();
                if (inst > 0)
                {
                    result.Status = true;
                    result.Data = item;
                    result.Message = "Thêm mới thành công";
                }
                else
                {
                    result.Status = false;
                    result.Data = null;
                    result.Message = "Thao tác không thành công";
                }
            }
            catch (Exception e)
            {
                result.Status = false;
                result.Message = e.Message;
                
            }
            return Ok(result);
        }


        [ResponseType(typeof(void))]
        [CustomAuthorize(Method = "SUA", State = "NhomQuyen")]
        public async Task<IHttpActionResult> Put (string id, NHOMQUYEN instance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != instance.ID)
            {
                return BadRequest();
            }
            var result = new TransferObj<NHOMQUYEN>();
            if (id != instance.ID)
            {
                result.Status = false;
                result.Message = "Mã ID không hợp lệ";
                return Ok(result);
            }
            try
            {
                var item = _service.Update(instance);
                int upd = await _service.UnitOfWork.SaveAsync();
                if (upd > 0)
                {
                    result.Status = true;
                    result.Data = item;
                    result.Message = "Cập nhật thành công";
                }
                else
                {
                    result.Status = false;
                    result.Data = null;
                    result.Message = "Thao tác không thành công";
                }
            }
            catch (Exception e)
            {
                result.Data = null;
                result.Status = false;
                result.Message = e.Message;
            }
            return Ok(result);
        }


        [ResponseType(typeof(NHOMQUYEN))]
        [CustomAuthorize(Method = "XOA", State = "NhomQuyen")]
        public async Task<IHttpActionResult> Delete(string id)
        {
            var result = new TransferObj<bool>();
            NHOMQUYEN instance = await _service.Repository.FindAsync(id);
            if (instance == null)
            {
                return NotFound();
            }
            try
            {
                 _service.Delete(instance.ID);
                int del = await _service.UnitOfWork.SaveAsync();
                if(del > 0)
                {
                    result.Data = true;
                    result.Status = true;
                    result.Message = "Xóa thành công bản ghi";
                }
                else
                {
                    result.Data = false;
                    result.Status = false;
                    result.Message = "Thao tác không thành công";
                }
            }
            catch (Exception e)
            {
                result.Data = false;
                result.Status = false;
                result.Message = e.Message;
            }
            return Ok(result);
        }


        [HttpGet]
        [Route("GetNhomQuyenChuaCauHinh/{userName}")]
        public IHttpActionResult GetNhomQuyenChuaCauHinh(string userName)
        {
            var result = new TransferObj<List<NhomQuyenViewModel.Dto>>();
            List<NhomQuyenViewModel.Dto> listNhomQuyen = new List<NhomQuyenViewModel.Dto>();
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
                                                                    a.ID,
                                                                    a.MANHOMQUYEN,
                                                                    a.TENNHOMQUYEN
                                                                FROM
                                                                    NHOMQUYEN a
                                                                WHERE a.MANHOMQUYEN NOT IN (
                                                                    SELECT
                                                                        b.MANHOMQUYEN
                                                                    FROM
                                                                        NGUOIDUNG_NHOMQUYEN b
                                                                    WHERE
                                                                        b.USERNAME = :USERNAME)
                                                                    ");
                                command.Parameters.Add(@"USERNAME", OracleDbType.NVarchar2, 50).Value = userName;
                                OracleDataReader dataReader = command.ExecuteReader();
                                if (dataReader.HasRows)
                                {
                                    while (dataReader.Read())
                                    {
                                        NhomQuyenViewModel.Dto row = new NhomQuyenViewModel.Dto();
                                        if (dataReader["ID"] != null)
                                        {
                                            row.ID = dataReader["ID"].ToString();
                                        }
                                        if (dataReader["MANHOMQUYEN"] != null)
                                        {
                                            row.MANHOMQUYEN = dataReader["MANHOMQUYEN"].ToString();
                                        }
                                        if (dataReader["TENNHOMQUYEN"] != null)
                                        {
                                            row.TENNHOMQUYEN = dataReader["TENNHOMQUYEN"].ToString();
                                        }
                                        listNhomQuyen.Add(row);
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
                    if (listNhomQuyen.Count > 0)
                    {
                        result.Status = true;
                        result.Data = listNhomQuyen;
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