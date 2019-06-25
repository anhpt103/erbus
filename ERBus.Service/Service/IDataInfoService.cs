using ERBus.Entity;
using ERBus.Service.Authorize.KyKeToan;
using ERBus.Service.Catalog.MatHang;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Claims;

namespace ERBus.Service.Service
{
    public interface IDataInfoService<TEntity> : IEntityService<TEntity>
        where TEntity : DataInfoEntity
    {
        new IDataInfoService<TEntity> Include(Expression<Func<TEntity, object>> include);
        TEntity Find(TEntity instance, bool notracking = false);
        TEntity FindById(string id, bool notracking = false);

        TEntity Insert(TEntity instance, bool withUnitCode = true);

        TEntity Update(TEntity instance,
            Action<TEntity, TEntity> updateAction = null,
            Func<TEntity, TEntity, bool> updateCondition = null);

        TEntity Delete(string id);
        TEntity AddUnit(TEntity instance);
        string GetCurrentUnitCode();
        string PhysicalPathTemplate();
        string PhysicalPathUploadFile();
        string PhysicalPathUploadLoaiPhong();
        ClaimsPrincipal GetClaimsPrincipal();
        string GetConnectionString();
        List<MatHangViewModel.VIEW_MODEL> GetDataMatHang(string ListMatHang, string UnitCode, string StringConnect);
        //convert chuỗi thành định dạng câu lệnh IN
        string ConvertConditionStringToArray(string str);
        //Lấy trên table kỳ kế toán theo tham số truyền vào 
        string Get_TableName_XNT(int Nam, int Ky);
        //Lấy tên table kỳ kế toán liền kề trước theo tham số truyền vào
        string Get_TableName_XNT_Previous(int Nam, int Ky);
        //Tăng tồn theo ID phiếu với nhập và bán trả lại
        bool XuatNhapTon_TangPhieu(string TableName, int Nam, int Ky, string Id, string StringConnect);
        //Khóa sổ từng ngày
        bool KhoaSoKyKeToan(string PreviousTableName, string CurrentTableName, string UnitCode, int Nam, int Ky, string StringConnect);
        //Khóa sổ Nhiều khỳ kế toán
        bool KhoaSoNhieuKyKeToan(List<KyKeToanViewModel.ViewModel> listPeriod, string UnitCode, string StringConnect);
        //lấy ra kỳ kế toán đã khóa sổ cuối cùng
        KyKeToanViewModel.ViewModel GetLastestPeriod(string UnitCode, string StringConnect);
        //lấy ra table xuất nhập tồn theo ngày
        KyKeToanViewModel.ViewModel GetTableXuatNhapTonTheoNgay(DateTime date, string UnitCode, string StringConnect);
        //lấy danh sách các kỳ kế toán chưa khóa sổ
        List<KyKeToanViewModel.ViewModel> KyKeToanChuaKhoa(string UnitCode, string StringConnect);
    }
}