namespace ERBus.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTableLOAIPHONG_BACKGROUND_ICON : DbMigration
    {
        public override void Up()
        {
            AddColumn("ERBUS.LOAIPHONG", "BACKGROUND", c => c.Binary());
            AddColumn("ERBUS.LOAIPHONG", "ICON", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("ERBUS.LOAIPHONG", "ICON");
            DropColumn("ERBUS.LOAIPHONG", "BACKGROUND");
        }
    }
}
