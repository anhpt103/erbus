namespace ERBus.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitializerLichSuLenHang_MaHang : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "ERBUS.LICHSU_TANGHANG",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50),
                        MAKHACHHANG = c.String(nullable: false, maxLength: 50),
                        MAHANG_CU = c.String(nullable: false, maxLength: 50),
                        MAHANG_MOI = c.String(nullable: false, maxLength: 50),
                        NGAY_LENHANG = c.DateTime(nullable: false),
                        THOIGIAN_LENHANG = c.String(maxLength: 50),
                        MA_GIAODICH_LENHANG = c.String(maxLength: 70),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("ERBUS.KHACHHANG", "MAHANG", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("ERBUS.KHACHHANG", "MAHANG");
            DropTable("ERBUS.LICHSU_TANGHANG");
        }
    }
}
