namespace ERBus.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CREATE_LICHSU_DATPHONG_ANHPT : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "ERBUS.LICHSU_DATPHONG",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50),
                        MA_DATPHONG = c.String(nullable: false, maxLength: 70),
                        MAPHONG = c.String(nullable: false, maxLength: 50),
                        NGAY_DATPHONG = c.DateTime(nullable: false),
                        THOIGIAN_DATPHONG = c.String(nullable: false, maxLength: 12),
                        TEN_KHACHHANG = c.String(maxLength: 100),
                        DIENTHOAI = c.String(maxLength: 20),
                        CANCUOC_CONGDAN = c.String(maxLength: 20),
                        DIENGIAI = c.String(maxLength: 200),
                        TRANGTHAI = c.Decimal(precision: 10, scale: 0),
                        I_CREATE_DATE = c.DateTime(),
                        I_CREATE_BY = c.String(maxLength: 25),
                        UNITCODE = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("ERBUS.LICHSU_DATPHONG");
        }
    }
}
