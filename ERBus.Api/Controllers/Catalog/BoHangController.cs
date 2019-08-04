using AutoMapper;
using ERBus.Entity;
using ERBus.Entity.Database.Catalog;
using ERBus.Service;
using ERBus.Service.Authorize.Utils;
using ERBus.Service.BuildQuery;
using ERBus.Service.BuildQuery.Query.Types;
using ERBus.Service.Catalog.BoHang;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ERBus.Api.Controllers.Catalog
{
    [RoutePrefix("api/Catalog/BoHang")]
    [Route("{id?}")]
    [Authorize]
    public class BoHangController : ApiController
    {
        private IBoHangService _service;
        public BoHangController(IBoHangService service)
        {
            _service = service;
        }
        
        [Route("GetAllData")]
        [HttpGet]
        [CustomAuthorize(Method = "XEM", State = "BoHang")]
        public IHttpActionResult GetAllData()
        {
            var result = new TransferObj<List<ChoiceObject>>();
            var unitCode = _service.GetCurrentUnitCode();
            result.Data = _service.Repository.DbSet.Where(x => x.TRANGTHAI == (int)TypeState.USED && x.UNITCODE.Equals(unitCode)).OrderBy(x => x.MABOHANG).Select(x => new ChoiceObject { VALUE = x.MABOHANG, TEXT = x.MABOHANG + "|" + x.TENBOHANG, DESCRIPTION = x.TENBOHANG, EXTEND_VALUE = x.UNITCODE, ID = x.ID }).ToList();
            if(result.Data.Count > 0)
            {
                result.Status = true;
            }
            else
            {
                result.Status = false;
            }
            return Ok(result);
        }

        [Route("GetMatHangTrongBo/{maBoHang}")]
        [HttpGet]
        [CustomAuthorize(Method = "XEM", State = "BoHang")]
        public IHttpActionResult GetMatHangTrongBo(string maBoHang)
        {
            var result = new TransferObj<List<ChoiceObject>>();
            List<ChoiceObject> listResult = new List<ChoiceObject>();
            if (!string.IsNullOrEmpty(maBoHang))
            {
                var unitCode = _service.GetCurrentUnitCode();

                var boHangChiTiet = _service.UnitOfWork.Repository<BOHANG_CHITIET>().DbSet.Where(x => x.MABOHANG.Equals(maBoHang) && x.UNITCODE.Equals(unitCode)).OrderBy(x => x.MAHANG).ToList();
                if (boHangChiTiet.Count > 0)
                {
                    foreach (var dongHang in boHangChiTiet)
                    {
                        ChoiceObject obj = new ChoiceObject();
                        obj.PARENT = dongHang.MABOHANG;
                        obj.VALUE = dongHang.MAHANG;
                        var matHang = _service.UnitOfWork.Repository<MATHANG>().DbSet.FirstOrDefault(x => x.MAHANG.Equals(dongHang.MAHANG) && x.UNITCODE.Equals(unitCode));
                        var matHangGia = _service.UnitOfWork.Repository<MATHANG_GIA>().DbSet.FirstOrDefault(x => x.MAHANG.Equals(dongHang.MAHANG) && x.UNITCODE.Equals(unitCode));
                        obj.TEXT = matHang != null ? matHang.TENHANG : "";
                        obj.GIATRI = matHang != null ? matHangGia.GIABANLE_VAT : 0;
                        listResult.Add(obj);
                    }
                }
                if (listResult.Count > 0)
                {
                    result.Data = listResult;
                    result.Status = true;
                }
                else
                {
                    result.Data = null;
                    result.Status = false;
                }
            }
            else
            {
                result.Data = null;
                result.Status = false;
            }
            return Ok(result);
        }

        

        [Route("PostQuery")]
        [HttpPost]
        [CustomAuthorize(Method = "XEM", State = "BoHang")]
        public IHttpActionResult PostQuery(JObject jsonData)
        {
            var result = new TransferObj();
            var postData = ((dynamic)jsonData);
            var filtered = ((JObject)postData.filtered).ToObject<FilterObj<BoHangViewModel.Search>>();
            var paged = ((JObject)postData.paged).ToObject<PagedObj<BOHANG>>();
            var unitCode = string.IsNullOrEmpty(filtered.PARENT_UNITCODE) ? filtered.UNITCODE : filtered.PARENT_UNITCODE;
            var query = new QueryBuilder
            {
                Take = paged.ItemsPerPage,
                Skip = paged.FromItem - 1,
                Filter = new QueryFilterLinQ()
                {
                    Property = ClassHelper.GetProperty(() => new BOHANG().UNITCODE),
                    Method = FilterMethod.EqualTo,
                    Value = unitCode
                },
                Orders = new List<IQueryOrder>()
                {
                    new QueryOrder()
                    {
                        Field = ClassHelper.GetPropertyName(() => new BOHANG().MABOHANG),
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

        [ResponseType(typeof(BOHANG))]
        [CustomAuthorize(Method = "THEM", State = "BoHang")]
        public async Task<IHttpActionResult> Post (BoHangViewModel.Dto instance)
        {
            var result = new TransferObj<BOHANG>();
            var curentUnitCode = _service.GetCurrentUnitCode();
            if (instance.MABOHANG == "")
            {
                result.Status = false;
                result.Message = "Mã không hợp lệ";
                return Ok(result);
            }
            else
            {
                var exist = _service.Repository.DbSet.FirstOrDefault(x => x.MABOHANG == instance.MABOHANG && x.UNITCODE.Equals(curentUnitCode));
                if (exist != null)
                {
                    result.Status = false;
                    result.Message = "Đã tồn tại mã loại này";
                    return Ok(result);
                }
            }
            try
            {
                instance.MABOHANG = _service.SaveCode();
                var item = _service.InsertBohang(instance);
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
        [CustomAuthorize(Method = "SUA", State = "BoHang")]
        public async Task<IHttpActionResult> Put (string id, BoHangViewModel.Dto instance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != instance.ID)
            {
                return BadRequest();
            }
            var result = new TransferObj<BOHANG>();
            if (id != instance.ID)
            {
                result.Status = false;
                result.Message = "Mã ID không hợp lệ";
                return Ok(result);
            }
            try
            {
                var item = _service.UpdateBohang(instance);
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


        [Route("GetDetails/{ID}")]
        [HttpGet]
        public IHttpActionResult GetDetails(string ID)
        {
            var result = new TransferObj<BoHangViewModel.Dto>();
            BoHangViewModel.Dto dto = new BoHangViewModel.Dto();
            if (string.IsNullOrEmpty(ID))
            {
                return BadRequest("ID không chính xác");
            }
            else
            {
                var unitCode = _service.GetCurrentUnitCode();
                var boHang = _service.Repository.DbSet.FirstOrDefault(x => x.UNITCODE.Equals(unitCode) && x.ID.Equals(ID));
                if (boHang != null)
                {
                    string connectString = ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString;
                    dto = Mapper.Map<BOHANG, BoHangViewModel.Dto>(boHang);
                    var boHangChiTiet = _service.UnitOfWork.Repository<BOHANG_CHITIET>().DbSet.Where(x => x.MABOHANG.Equals(boHang.MABOHANG)).ToList();
                    dto.DataDetails = Mapper.Map<List<BOHANG_CHITIET>, List<BoHangViewModel.DataDetails>>(boHangChiTiet);
                    if (dto.DataDetails.Count > 0)
                    {
                        string listMatHang = "";
                        foreach (var matHang in dto.DataDetails)
                        {
                            listMatHang += matHang.MAHANG + ",";
                        }
                        listMatHang = listMatHang.Substring(0, listMatHang.Length - 1);
                        var MatHangViewModel = _service.GetDataMatHang(_service.ConvertConditionStringToArray(listMatHang), unitCode, connectString);
                        foreach (var row in dto.DataDetails)
                        {
                            var hang = MatHangViewModel.FirstOrDefault(x => x.MAHANG.Equals(row.MAHANG));
                            if (hang != null)
                            {
                                row.TENHANG = hang.TENHANG;
                                row.MADONVITINH = hang.MADONVITINH;
                                row.GIABANLE_VAT = hang.GIABANLE_VAT;
                            }
                        }
                    }
                }
                if (dto != null && !string.IsNullOrEmpty(dto.MABOHANG))
                {
                    result.Data = dto;
                    result.Status = true;
                    result.Message = "Oke";
                }
                else
                {
                    result.Data = null;
                    result.Status = false;
                    result.Message = "NotFound";
                }
            }
            return Ok(result);
        }

        [ResponseType(typeof(BOHANG))]
        [CustomAuthorize(Method = "XOA", State = "BoHang")]
        public async Task<IHttpActionResult> Delete(string id)
        {
            var result = new TransferObj<bool>();
            BOHANG instance = await _service.Repository.FindAsync(id);
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