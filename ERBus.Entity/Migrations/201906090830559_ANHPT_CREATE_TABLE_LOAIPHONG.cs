namespace ERBus.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ANHPT_CREATE_TABLE_LOAIPHONG : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "ERBUS.LOAIPHONG",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50),
                        MALOAIPHONG = c.String(nullable: false, maxLength: 50),
                        TENLOAIPHONG = c.String(nullable: false, maxLength: 100),
                        MABOHANG = c.String(maxLength: 50),
                        TRANGTHAI = c.Decimal(precision: 10, scale: 0),
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
            DropTable("ERBUS.LOAIPHONG");
        }
    }
}
