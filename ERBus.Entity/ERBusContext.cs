using ERBus.Entity.Database.Authorize;
using ERBus.Entity.Database.Catalog;
using ERBus.Entity.Database.Knowledge;
using System.Data.Entity;
using System.Diagnostics;
namespace ERBus.Entity
{
    public class ERBusContext : DataContext
    {
        public ERBusContext() : base("name=ERBusConnection")
        {
            Configuration.LazyLoadingEnabled = true;
            this.Database.Log = s => Debug.WriteLine(s);
        }
        /// <summary>
        /// Authorize
        /// </summary>
        public virtual DbSet<NGUOIDUNG> NGUOIDUNGs { get; set; }
        public virtual DbSet<MENU> MENUs { get; set; }
        public virtual DbSet<NGUOIDUNG_MENU> NGUOIDUNG_MENUs { get; set; }
        public virtual DbSet<NGUOIDUNG_NHOMQUYEN> NGUOIDUNG_NHOMQUYENs { get; set; }
        public virtual DbSet<NHOMQUYEN> NHOMQUYENs { get; set; }
        public virtual DbSet<NHOMQUYEN_MENU> NHOMQUYEN_MENUs { get; set; }

        public virtual DbSet<THAMSOHETHONG> THAMSOHETHONGs { get; set; }
        public virtual DbSet<KYKETOAN> KYKETOANs { get; set; }
        public virtual DbSet<CUAHANG> CUAHANGs { get; set; }

        /// <summary>
        /// Catalog
        /// </summary>
        public virtual DbSet<BAOBI> BAOBIs { get; set; }
        public virtual DbSet<BOHANG> BOHANGs { get; set; }
        public virtual DbSet<BOHANG_CHITIET> BOHANG_CHITIETs { get; set; }
        public virtual DbSet<DONVITINH> DONVITINHs { get; set; }
        public virtual DbSet<KEHANG> KEHANGs { get; set; }
        public virtual DbSet<KHOHANG> KHOHANGs { get; set; }
        public virtual DbSet<LOAIHANG> LOAIHANGs { get; set; }
        public virtual DbSet<MATHANG> MATHANGs { get; set; }
        public virtual DbSet<MATHANG_GIA> MATHANG_GIAs { get; set; }
        public virtual DbSet<NHACUNGCAP> NHACUNGCAPs { get; set; }
        public virtual DbSet<NHOMHANG> NHOMHANGs { get; set; }
        public virtual DbSet<THUE> THUEs { get; set; }
        public virtual DbSet<KHACHHANG> KHACHHANGs { get; set; }
        public virtual DbSet<HANGKHACHHANG> HANGKHACHHANGs { get; set; }
        public virtual DbSet<LICHSU_TANGHANG> LICHSU_TANGHANGs { get; set; }
        public virtual DbSet<CAPMA> CAPMAs { get; set; }
        public virtual DbSet<CAPMA_THEONGAY> CAPMA_THEONGAYs { get; set; }
        public virtual DbSet<PHONG> PHONGs { get; set; }
        public virtual DbSet<LOAIPHONG> LOAIPHONGs { get; set; }
        public virtual DbSet<CAUHINH_LOAIPHONG> CAUHINH_LOAIPHONGs { get; set; }

        /// <summary>
        /// Knowledge
        /// </summary>
        public virtual DbSet<CHUNGTU> CHUNGTUs { get; set; }
        public virtual DbSet<CHUNGTU_CHITIET> CHUNGTU_CHITIETs { get; set; }

        public virtual DbSet<GIAODICH> GIAODICHs { get; set; }
        public virtual DbSet<GIAODICH_CHITIET> GIAODICH_CHITIETs { get; set; }

        public virtual DbSet<KHUYENMAI> KHUYENMAIs { get; set; }
        public virtual DbSet<KHUYENMAI_CHITIET> KHUYENMAI_CHITIETs { get; set; }
        public virtual DbSet<DATPHONG> DATPHONGs { get; set; }
        public virtual DbSet<LICHSU_DATPHONG> LICHSU_DATPHONGs { get; set; }
        public virtual DbSet<THANHTOAN_DATPHONG> THANHTOAN_DATPHONGs { get; set; }
        public virtual DbSet<THANHTOAN_DATPHONG_CHITIET> THANHTOAN_DATPHONG_CHITIETs { get; set; }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("ERBUS");
            base.OnModelCreating(modelBuilder);
        }
    }
}
