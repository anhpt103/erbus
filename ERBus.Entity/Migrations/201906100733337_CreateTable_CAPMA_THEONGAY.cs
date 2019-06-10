namespace ERBus.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTable_CAPMA_THEONGAY : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "ERBUS.CAPMA_THEONGAY",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50),
                        LOAIMA = c.String(maxLength: 18),
                        NHOMMA = c.String(maxLength: 18),
                        NGAY_SINHMA = c.DateTime(),
                        GIATRI = c.String(maxLength: 10),
                        UNITCODE = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("ERBUS.CAPMA_THEONGAY");
        }
    }
}
