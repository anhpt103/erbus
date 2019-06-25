namespace ERBus.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ANHPT_UPDATE_TABLE_BOHANG_CHITIET : DbMigration
    {
        public override void Up()
        {
            DropColumn("ERBUS.BOHANG_CHITIET", "TONGTIEN");
        }
        
        public override void Down()
        {
            AddColumn("ERBUS.BOHANG_CHITIET", "TONGTIEN", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
