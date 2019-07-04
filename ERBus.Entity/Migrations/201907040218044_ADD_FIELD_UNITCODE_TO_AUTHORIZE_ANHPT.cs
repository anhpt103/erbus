namespace ERBus.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ADD_FIELD_UNITCODE_TO_AUTHORIZE_ANHPT : DbMigration
    {
        public override void Up()
        {
            AddColumn("ERBUS.NGUOIDUNG_MENU", "UNITCODE", c => c.String(maxLength: 10));
            AddColumn("ERBUS.NGUOIDUNG_NHOMQUYEN", "UNITCODE", c => c.String(maxLength: 10));
            AddColumn("ERBUS.NHOMQUYEN_MENU", "UNITCODE", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            DropColumn("ERBUS.NHOMQUYEN_MENU", "UNITCODE");
            DropColumn("ERBUS.NGUOIDUNG_NHOMQUYEN", "UNITCODE");
            DropColumn("ERBUS.NGUOIDUNG_MENU", "UNITCODE");
        }
    }
}
