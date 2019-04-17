using BTS.API.SERVICE.Authorize;
using ERBus.Api.Utils;
using ERBus.Entity;
using ERBus.Entity.Database.Authorize;
using ERBus.Entity.Database.Catalog;
using ERBus.Entity.Database.Knowledge;
using ERBus.Service;
using ERBus.Service.Authorize.CuaHang;
using ERBus.Service.Authorize.KyKeToan;
using ERBus.Service.Authorize.Menu;
using ERBus.Service.Authorize.NguoiDung;
using ERBus.Service.Authorize.NhomQuyen;
using ERBus.Service.Catalog.BaoBi;
using ERBus.Service.Catalog.BoHang;
using ERBus.Service.Catalog.DonViTinh;
using ERBus.Service.Catalog.HangKhachHang;
using ERBus.Service.Catalog.KeHang;
using ERBus.Service.Catalog.KhachHang;
using ERBus.Service.Catalog.KhoHang;
using ERBus.Service.Catalog.LoaiHang;
using ERBus.Service.Catalog.MatHang;
using ERBus.Service.Catalog.NhaCungCap;
using ERBus.Service.Catalog.NhomHang;
using ERBus.Service.Catalog.ThamSoHeThong;
using ERBus.Service.Catalog.Thue;
using ERBus.Service.Knowledge.NhapMua;
using ERBus.Service.Knowledge.XuatBan;
using ERBus.Service.Promotion.GiamGiaLoaiHang;
using ERBus.Service.Promotion.GiamGiaNhaCungCap;
using ERBus.Service.Promotion.GiamGiaNhomHang;
using ERBus.Service.Promotion.TienTyLe;
using System.Web.Http;
using Unity;
using Unity.Lifetime;

namespace ERBus.Api.App_Start
{
    public static class WebApiConfig
    {
        public static void RegisterComponents(HttpConfiguration config)
        {
            var container = new UnityContainer();
            container.RegisterType<IDataContext, ERBusContext>(new HierarchicalLifetimeManager());
            container.RegisterType<IUnitOfWork, UnitOfWork>(new HierarchicalLifetimeManager());
          
            //Catalog

            container.RegisterType<IRepository<NGUOIDUNG>, Repository<NGUOIDUNG>>(new HierarchicalLifetimeManager());
            container.RegisterType<INguoiDungService, NguoiDungService>(new HierarchicalLifetimeManager());

            container.RegisterType<IRepository<MENU>, Repository<MENU>>(new HierarchicalLifetimeManager());
            container.RegisterType<IMenuService, MenuService>(new HierarchicalLifetimeManager());

            container.RegisterType<IAccessService, AccessService>(new HierarchicalLifetimeManager());

            container.RegisterType<IRepository<LOAIHANG>, Repository<LOAIHANG>>(new HierarchicalLifetimeManager());
            container.RegisterType<ILoaiHangService, LoaiHangService>(new HierarchicalLifetimeManager());

            container.RegisterType<IRepository<NHOMHANG>, Repository<NHOMHANG>>(new HierarchicalLifetimeManager());
            container.RegisterType<INhomHangService, NhomHangService>(new HierarchicalLifetimeManager());

            container.RegisterType<IRepository<BAOBI>, Repository<BAOBI>>(new HierarchicalLifetimeManager());
            container.RegisterType<IBaoBiService, BaoBiService>(new HierarchicalLifetimeManager());

            container.RegisterType<IRepository<DONVITINH>, Repository<DONVITINH>>(new HierarchicalLifetimeManager());
            container.RegisterType<IDonViTinhService, DonViTinhService>(new HierarchicalLifetimeManager());

            container.RegisterType<IRepository<BOHANG>, Repository<BOHANG>>(new HierarchicalLifetimeManager());
            container.RegisterType<IBoHangService, BoHangService>(new HierarchicalLifetimeManager());

            container.RegisterType<IRepository<DONVITINH>, Repository<DONVITINH>>(new HierarchicalLifetimeManager());
            container.RegisterType<IDonViTinhService, DonViTinhService>(new HierarchicalLifetimeManager());

            container.RegisterType<IRepository<HANGKHACHHANG>, Repository<HANGKHACHHANG>>(new HierarchicalLifetimeManager());
            container.RegisterType<IHangKhachHangService, HangKhachHangService>(new HierarchicalLifetimeManager());

            container.RegisterType<IRepository<KEHANG>, Repository<KEHANG>>(new HierarchicalLifetimeManager());
            container.RegisterType<IKeHangService, KeHangService>(new HierarchicalLifetimeManager());

            container.RegisterType<IRepository<KHOHANG>, Repository<KHOHANG>>(new HierarchicalLifetimeManager());
            container.RegisterType<IKhoHangService, KhoHangService>(new HierarchicalLifetimeManager());

            container.RegisterType<IRepository<THUE>, Repository<THUE>>(new HierarchicalLifetimeManager());
            container.RegisterType<IThueService, ThueService>(new HierarchicalLifetimeManager());

            container.RegisterType<IRepository<KHACHHANG>, Repository<KHACHHANG>>(new HierarchicalLifetimeManager());
            container.RegisterType<IKhachHangService, KhachHangService>(new HierarchicalLifetimeManager());

            container.RegisterType<IRepository<NHACUNGCAP>, Repository<NHACUNGCAP>>(new HierarchicalLifetimeManager());
            container.RegisterType<INhaCungCapService, NhaCungCapService>(new HierarchicalLifetimeManager());

            container.RegisterType<IRepository<MATHANG>, Repository<MATHANG>>(new HierarchicalLifetimeManager());
            container.RegisterType<IMatHangService, MatHangService>(new HierarchicalLifetimeManager());

            //Authorize
            container.RegisterType<IRepository<THAMSOHETHONG>, Repository<THAMSOHETHONG>>(new HierarchicalLifetimeManager());
            container.RegisterType<IThamSoHeThongService, ThamSoHeThongService>(new HierarchicalLifetimeManager());

            container.RegisterType<IRepository<KYKETOAN>, Repository<KYKETOAN>>(new HierarchicalLifetimeManager());
            container.RegisterType<IKyKeToanService, KyKeToanService>(new HierarchicalLifetimeManager());

            container.RegisterType<IRepository<CUAHANG>, Repository<CUAHANG>>(new HierarchicalLifetimeManager());
            container.RegisterType<ICuaHangService, CuaHangService>(new HierarchicalLifetimeManager());

            container.RegisterType<IRepository<NHOMQUYEN>, Repository<NHOMQUYEN>>(new HierarchicalLifetimeManager());
            container.RegisterType<INhomQuyenService, NhomQuyenService>(new HierarchicalLifetimeManager());

            container.RegisterType<IRepository<NGUOIDUNG>, Repository<NGUOIDUNG>>(new HierarchicalLifetimeManager());
            container.RegisterType<INguoiDungService, NguoiDungService>(new HierarchicalLifetimeManager());

            //End Catalog

            //Knowledge
            container.RegisterType<IRepository<CHUNGTU>, Repository<CHUNGTU>>(new HierarchicalLifetimeManager());
            container.RegisterType<INhapMuaService, NhapMuaService>(new HierarchicalLifetimeManager());
            container.RegisterType<IXuatBanService, XuatBanService>(new HierarchicalLifetimeManager());

            //Promotion
            container.RegisterType<IRepository<KHUYENMAI>, Repository<KHUYENMAI>>(new HierarchicalLifetimeManager());
            container.RegisterType<ITienTyLeService, TienTyLeService>(new HierarchicalLifetimeManager());
            container.RegisterType<IGiamGiaLoaiHangService, GiamGiaLoaiHangService>(new HierarchicalLifetimeManager());
            container.RegisterType<IGiamGiaNhomHangService, GiamGiaNhomHangService>(new HierarchicalLifetimeManager());
            container.RegisterType<IGiamGiaNhaCungCapService, GiamGiaNhaCungCapService>(new HierarchicalLifetimeManager());

            //End Knowledge
            config.DependencyResolver = new UnityResolver(container);
        }
        
        public static void Register(HttpConfiguration config)
        {
            RegisterComponents(config);
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
             name: "actionApi",
             routeTemplate: "api/{controller}/{action}/{code}",
             defaults: new { code = RouteParameter.Optional }
            );
        }
    }
}