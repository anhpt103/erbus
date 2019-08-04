namespace ERBus.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CREATE_TABLE_CAUHINH_LOAIPHONG_GIACA_ANHPT : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "ERBUS.CAUHINH_LOAIPHONG_GIACA",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50),
                        MALOAIPHONG = c.String(nullable: false, maxLength: 50),
                        MAHANG = c.String(nullable: false, maxLength: 50),
                        GIABANLE_VAT = c.Decimal(nullable: false, precision: 18, scale: 2),
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
            DropTable("ERBUS.CAUHINH_LOAIPHONG_GIACA");
        }
    }
}
