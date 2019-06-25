namespace ERBus.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UPDATE_FIELD_TO_TABLE_THANHTOAN_CHITIET_ANHPT : DbMigration
    {
        public override void Up()
        {
            AddColumn("ERBUS.THANHTOAN_DATPHONG", "THOIGIAN_SUDUNG", c => c.String(nullable: false));
            AddColumn("ERBUS.THANHTOAN_DATPHONG", "DONVI_THOIGIAN_TINHTIEN", c => c.Decimal(precision: 10, scale: 0));
            DropColumn("ERBUS.THANHTOAN_DATPHONG", "SOGIO");
        }
        
        public override void Down()
        {
            AddColumn("ERBUS.THANHTOAN_DATPHONG", "SOGIO", c => c.String(nullable: false));
            DropColumn("ERBUS.THANHTOAN_DATPHONG", "DONVI_THOIGIAN_TINHTIEN");
            DropColumn("ERBUS.THANHTOAN_DATPHONG", "THOIGIAN_SUDUNG");
        }
    }
}
