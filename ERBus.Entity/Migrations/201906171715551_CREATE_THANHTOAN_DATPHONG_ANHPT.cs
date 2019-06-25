namespace ERBus.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CREATE_THANHTOAN_DATPHONG_ANHPT : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "ERBUS.THANHTOAN_DATPHONG_CHITIET",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50),
                        MA_DATPHONG = c.String(nullable: false, maxLength: 70),
                        MABOHANG = c.String(maxLength: 50),
                        MAHANG = c.String(nullable: false, maxLength: 50),
                        MATHUE_RA = c.String(nullable: false, maxLength: 50),
                        SOLUONG = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GIABANLE_VAT = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CHIETKHAU = c.Decimal(precision: 18, scale: 2),
                        SAPXEP = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "ERBUS.THANHTOAN_DATPHONG",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50),
                        MA_DATPHONG = c.String(nullable: false, maxLength: 70),
                        MAPHONG = c.String(nullable: false, maxLength: 50),
                        NGAY_DATPHONG = c.DateTime(nullable: false),
                        THOIGIAN_DATPHONG = c.String(nullable: false, maxLength: 12),
                        NGAY_THANHTOAN = c.DateTime(nullable: false),
                        THOIGIAN_THANHTOAN = c.String(nullable: false, maxLength: 12),
                        SOGIO = c.String(nullable: false),
                        TEN_KHACHHANG = c.String(maxLength: 100),
                        DIENTHOAI = c.String(maxLength: 20),
                        CANCUOC_CONGDAN = c.String(maxLength: 20),
                        DIENGIAI = c.String(maxLength: 200),
                        MABOHANG = c.String(maxLength: 50),
                        I_CREATE_DATE = c.DateTime(),
                        I_CREATE_BY = c.String(maxLength: 25),
                        I_UPDATE_DATE = c.DateTime(),
                        I_UPDATE_BY = c.String(maxLength: 25),
                        I_STATE = c.String(maxLength: 1),
                        UNITCODE = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("ERBUS.THANHTOAN_DATPHONG");
            DropTable("ERBUS.THANHTOAN_DATPHONG_CHITIET");
        }
    }
}
