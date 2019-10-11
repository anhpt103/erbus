namespace ERBus.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ADD_COLUMN_SAPXEP_BOHANG_CHITIET : DbMigration
    {
        public override void Up()
        {
            AddColumn("ERBUS.BOHANG_CHITIET", "SAPXEP", c => c.Decimal(precision: 10, scale: 0));
        }
        
        public override void Down()
        {
            DropColumn("ERBUS.BOHANG_CHITIET", "SAPXEP");
        }
    }
}
