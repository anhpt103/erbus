namespace ERBus.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ANHPT_CREATE_CAUHINH_LOAIPHONG : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "ERBUS.CAUHINH_LOAIPHONG",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50),
                        MALOAIPHONG = c.String(nullable: false, maxLength: 50),
                        MAHANG = c.String(nullable: false, maxLength: 50),
                        SOPHUT = c.Decimal(nullable: false, precision: 10, scale: 0),
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
            DropTable("ERBUS.CAUHINH_LOAIPHONG");
        }
    }
}
