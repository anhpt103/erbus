namespace ERBus.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ANHPT_CREATE_TABLE_PHONG : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "ERBUS.PHONG",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50),
                        MAPHONG = c.String(nullable: false, maxLength: 50),
                        TENPHONG = c.String(nullable: false, maxLength: 300),
                        TANG = c.Decimal(precision: 10, scale: 0),
                        VITRI = c.String(maxLength: 200),
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
            DropTable("ERBUS.PHONG");
        }
    }
}
