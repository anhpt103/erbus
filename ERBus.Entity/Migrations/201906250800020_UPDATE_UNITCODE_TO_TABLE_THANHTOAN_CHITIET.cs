namespace ERBus.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UPDATE_UNITCODE_TO_TABLE_THANHTOAN_CHITIET : DbMigration
    {
        public override void Up()
        {
            AddColumn("ERBUS.THANHTOAN_DATPHONG_CHITIET", "UNITCODE", c => c.String(nullable: false, maxLength: 10));
        }
        
        public override void Down()
        {
            DropColumn("ERBUS.THANHTOAN_DATPHONG_CHITIET", "UNITCODE");
        }
    }
}
