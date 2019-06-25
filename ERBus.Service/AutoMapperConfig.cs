using AutoMapper;
using ERBus.Entity;
using ERBus.Entity.Database.Authorize;
using ERBus.Entity.Database.Catalog;
using ERBus.Entity.Database.Knowledge;
using ERBus.Service.Authorize.CuaHang;
using ERBus.Service.Authorize.KyKeToan;
using ERBus.Service.Authorize.NguoiDung;
using ERBus.Service.Authorize.NhomQuyen;
using ERBus.Service.Authorize.ThamSoHeThong;
using ERBus.Service.BuildQuery;
using ERBus.Service.Catalog.BaoBi;
using ERBus.Service.Catalog.BoHang;
using ERBus.Service.Catalog.CauHinhLoaiPhong;
using ERBus.Service.Catalog.DonViTinh;
using ERBus.Service.Catalog.HangKhachHang;
using ERBus.Service.Catalog.KeHang;
using ERBus.Service.Catalog.KhachHang;
using ERBus.Service.Catalog.KhoHang;
using ERBus.Service.Catalog.LoaiHang;
using ERBus.Service.Catalog.LoaiPhong;
using ERBus.Service.Catalog.MatHang;
using ERBus.Service.Catalog.NhaCungCap;
using ERBus.Service.Catalog.NhomHang;
using ERBus.Service.Catalog.Phong;
using ERBus.Service.Catalog.Thue;
using ERBus.Service.Knowledge.DatPhong;
using ERBus.Service.Knowledge.NhapMua;
using ERBus.Service.Knowledge.ThanhToanDatPhong;
using ERBus.Service.Knowledge.XuatBan;
using ERBus.Service.Knowledge.XuatBanLeThuNgan;
using ERBus.Service.Promotion.GiamGiaLoaiHang;
using ERBus.Service.Promotion.GiamGiaNhaCungCap;
using ERBus.Service.Promotion.GiamGiaNhomHang;
using ERBus.Service.Promotion.TienTyLe;
using System;
using System.Web;

namespace ERBus.Service
{
    public static class AutoMapperConfig
    {
        public static void Config()
        {
            Mapper.Initialize(cfg =>
            {
                //Catalog
                cfg.CreateMap(typeof(PagedObj), typeof(PagedObj));
                cfg.CreateMap(typeof(PagedObj<>), typeof(PagedObj<>));

                cfg.CreateMap<LOAIHANG, LoaiHangViewModel.Dto>();
                cfg.CreateMap<LoaiHangViewModel.Dto, LOAIHANG>();

                cfg.CreateMap<NHOMHANG, NhomHangViewModel.Dto>();
                cfg.CreateMap<NhomHangViewModel.Dto, NHOMHANG>();

                cfg.CreateMap<BAOBI, BaoBiViewModel.Dto>();
                cfg.CreateMap<BaoBiViewModel.Dto, BAOBI>();

                cfg.CreateMap<DONVITINH, DonViTinhViewModel.Dto>();
                cfg.CreateMap<DonViTinhViewModel.Dto, DONVITINH>();

                cfg.CreateMap<BOHANG, BoHangViewModel.Dto>();
                cfg.CreateMap<BoHangViewModel.Dto, BOHANG>();

                cfg.CreateMap<BOHANG_CHITIET, BoHangViewModel.DataDetails>();
                cfg.CreateMap<BoHangViewModel.DataDetails, BOHANG_CHITIET>();

                cfg.CreateMap<THUE, ThueViewModel.Dto>();
                cfg.CreateMap<ThueViewModel.Dto, THUE>();

                cfg.CreateMap<HANGKHACHHANG, HangKhachHangViewModel.Dto>();
                cfg.CreateMap<HangKhachHangViewModel.Dto, HANGKHACHHANG>();

                cfg.CreateMap<KHOHANG, KhoHangViewModel.Dto>();
                cfg.CreateMap<KhoHangViewModel.Dto, KHOHANG>();

                cfg.CreateMap<KEHANG, KeHangViewModel.Dto>();
                cfg.CreateMap<KeHangViewModel.Dto, KEHANG>();

                cfg.CreateMap<KHACHHANG, KhachHangViewModel.Dto>();
                cfg.CreateMap<KhachHangViewModel.Dto, KHACHHANG>();

                cfg.CreateMap<NHACUNGCAP, NhaCungCapViewModel.Dto>();
                cfg.CreateMap<NhaCungCapViewModel.Dto, NHACUNGCAP>();

                cfg.CreateMap<PHONG, PhongViewModel.Dto>();
                cfg.CreateMap<PhongViewModel.Dto, PHONG>();

                cfg.CreateMap<LOAIPHONG, LoaiPhongViewModel.Dto>();
                cfg.CreateMap<LoaiPhongViewModel.Dto, LOAIPHONG>();

                cfg.CreateMap<LOAIPHONG, LoaiPhongViewModel.DtoCauHinh>();
                cfg.CreateMap<LoaiPhongViewModel.DtoCauHinh, LOAIPHONG>();

                cfg.CreateMap<CAUHINH_LOAIPHONG, CauHinhLoaiPhongViewModel.Dto>();
                cfg.CreateMap<CauHinhLoaiPhongViewModel.Dto, CAUHINH_LOAIPHONG>();

                cfg.CreateMap<MATHANG, MatHangViewModel.Dto>();
                cfg.CreateMap<MatHangViewModel.Dto, MATHANG>();
                cfg.CreateMap<MATHANG_GIA, MatHangViewModel.Dto>();
                cfg.CreateMap<MatHangViewModel.Dto, MATHANG_GIA>();
                cfg.CreateMap<MATHANG, MatHangViewModel.VIEW_MODEL>();
                cfg.CreateMap<MatHangViewModel.VIEW_MODEL, MATHANG>();

                //Authorize

                cfg.CreateMap<THAMSOHETHONG, ThamSoHeThongViewModel.Dto>();
                cfg.CreateMap<ThamSoHeThongViewModel.Dto, THAMSOHETHONG>();

                cfg.CreateMap<KYKETOAN, KyKeToanViewModel.Dto>();
                cfg.CreateMap<KyKeToanViewModel.Dto, KYKETOAN>();

                cfg.CreateMap<CUAHANG, CuaHangViewModel.Dto>();
                cfg.CreateMap<CuaHangViewModel.Dto, CUAHANG>();

                cfg.CreateMap<NHOMQUYEN, NhomQuyenViewModel.Dto>();
                cfg.CreateMap<NhomQuyenViewModel.Dto, NHOMQUYEN>();

                cfg.CreateMap<NGUOIDUNG, NguoiDungViewModel.Dto>();
                cfg.CreateMap<NguoiDungViewModel.Dto, NGUOIDUNG>();

                //Knowledge
                cfg.CreateMap<CHUNGTU, NhapMuaViewModel.Dto>();
                cfg.CreateMap<NhapMuaViewModel.Dto, CHUNGTU>();

                cfg.CreateMap<CHUNGTU_CHITIET, NhapMuaViewModel.DtoDetails>();
                cfg.CreateMap<NhapMuaViewModel.DtoDetails, CHUNGTU_CHITIET>();

                cfg.CreateMap<CHUNGTU, XuatBanViewModel.Dto>();
                cfg.CreateMap<XuatBanViewModel.Dto, CHUNGTU>();

                cfg.CreateMap<CHUNGTU_CHITIET, XuatBanViewModel.DtoDetails>();
                cfg.CreateMap<XuatBanViewModel.DtoDetails, CHUNGTU_CHITIET>();

                cfg.CreateMap<GIAODICH, XuatBanLeThuNganViewModel.Dto>();
                cfg.CreateMap<XuatBanLeThuNganViewModel.Dto, GIAODICH>();

                cfg.CreateMap<GIAODICH_CHITIET, XuatBanLeThuNganViewModel.DtoDetails>();
                cfg.CreateMap<XuatBanLeThuNganViewModel.DtoDetails, GIAODICH_CHITIET>();

                cfg.CreateMap<DATPHONG, DatPhongViewModel.Dto>();
                cfg.CreateMap<DatPhongViewModel.Dto, DATPHONG>();

                cfg.CreateMap<LICHSU_DATPHONG, DATPHONG>();
                cfg.CreateMap<DATPHONG, LICHSU_DATPHONG>();
                
                cfg.CreateMap<THANHTOAN_DATPHONG, ThanhToanDatPhongViewModel.Dto>();
                cfg.CreateMap<ThanhToanDatPhongViewModel.Dto, THANHTOAN_DATPHONG>();

                cfg.CreateMap<THANHTOAN_DATPHONG_CHITIET, ThanhToanDatPhongViewModel.DtoDetail>();
                cfg.CreateMap<ThanhToanDatPhongViewModel.DtoDetail, THANHTOAN_DATPHONG_CHITIET>();
                //Promotion

                cfg.CreateMap<KHUYENMAI, TienTyLeViewModel.Dto>();
                cfg.CreateMap<TienTyLeViewModel.Dto, KHUYENMAI>();

                cfg.CreateMap<KHUYENMAI, GiamGiaLoaiHangViewModel.Dto>();
                cfg.CreateMap<GiamGiaLoaiHangViewModel.Dto, KHUYENMAI>();

                cfg.CreateMap<KHUYENMAI, GiamGiaNhomHangViewModel.Dto>();
                cfg.CreateMap<GiamGiaNhomHangViewModel.Dto, KHUYENMAI>();

                cfg.CreateMap<KHUYENMAI, GiamGiaNhaCungCapViewModel.Dto>();
                cfg.CreateMap<GiamGiaNhaCungCapViewModel.Dto, KHUYENMAI>();

                cfg.CreateMap<KHUYENMAI_CHITIET, TienTyLeViewModel.DtoDetails>();
                cfg.CreateMap<TienTyLeViewModel.DtoDetails, KHUYENMAI_CHITIET>();
            });
        }
        public static IMappingExpression<TSource, TDestination> IgnoreDataInfoSelfMapping<TSource, TDestination>(
           this IMappingExpression<TSource, TDestination> map)
           where TSource : DataInfoEntity
           where TDestination : DataInfoEntity
        {
            map.ForMember(dest => dest.I_CREATE_DATE, config => config.Ignore());
            map.ForMember(dest => dest.I_CREATE_BY, config => config.Ignore());
            map.ForMember(dest => dest.UNITCODE, config => config.Ignore());
            map.AfterMap((src, dest) =>
            {
                if (string.IsNullOrEmpty(dest.I_CREATE_BY) && dest.I_CREATE_DATE == null)
                {
                    if (HttpContext.Current != null && HttpContext.Current.User.Identity.IsAuthenticated)
                        dest.I_CREATE_BY = HttpContext.Current.User.Identity.Name;
                        dest.I_CREATE_DATE = DateTime.Now;
                }
            });
            return map;
        }

        public static IMappingExpression<TSource, TDestination> IgnoreDataInfo<TSource, TDestination>(
          this IMappingExpression<TSource, TDestination> map)
            where TSource : DataInfoEntityDto
            where TDestination : DataInfoEntity
        {
            map.ForMember(dest => dest.I_CREATE_DATE, config => config.Ignore());
            map.ForMember(dest => dest.I_CREATE_BY, config => config.Ignore());
            map.ForMember(dest => dest.I_UPDATE_DATE,
                config => config.ResolveUsing<UpdateDateResolver>().FromMember(x => x.I_UPDATE_DATE));
            map.ForMember(dest => dest.I_UPDATE_BY,
                config => config.ResolveUsing<UpdateByResolver>().FromMember(x => x.I_UPDATE_BY));
            map.ForMember(dest => dest.I_STATE, config => config.Ignore());
            map.AfterMap((src, dest) =>
            {
                if (string.IsNullOrEmpty(dest.I_CREATE_BY) && dest.I_CREATE_DATE == null)
                {
                    if (HttpContext.Current != null && HttpContext.Current.User.Identity.IsAuthenticated)
                        dest.I_CREATE_BY = HttpContext.Current.User.Identity.Name;
                    dest.I_CREATE_DATE = DateTime.Now;
                }
            });
            return map;
        }
        public class IdResolver : ValueResolver<string, string>
        {
            protected override string ResolveCore(string source)
            {
                return Guid.NewGuid().ToString();
            }
        }
        public class UpdateDateResolver : ValueResolver<DateTime?, DateTime?>
        {
            protected override DateTime? ResolveCore(DateTime? source)
            {
                return DateTime.Now;
            }
        }
        public class UpdateByResolver : ValueResolver<string, string>
        {
            protected override string ResolveCore(string source)
            {
                var result = source;
                if (HttpContext.Current != null && HttpContext.Current.User.Identity.IsAuthenticated)
                    result = HttpContext.Current.User.Identity.Name;
                return result;
            }
        }
    }
}
