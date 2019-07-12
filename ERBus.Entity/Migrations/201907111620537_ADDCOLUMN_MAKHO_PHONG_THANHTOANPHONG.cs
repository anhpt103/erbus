namespace ERBus.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ADDCOLUMN_MAKHO_PHONG_THANHTOANPHONG : DbMigration
    {
        public override void Up()
        {
            AddColumn("ERBUS.PHONG", "MAKHO", c => c.String(nullable: false, maxLength: 50));
            AddColumn("ERBUS.THANHTOAN_DATPHONG", "MAKHO", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("ERBUS.THANHTOAN_DATPHONG", "MAKHO");
            DropColumn("ERBUS.PHONG", "MAKHO");
        }
    }
}
