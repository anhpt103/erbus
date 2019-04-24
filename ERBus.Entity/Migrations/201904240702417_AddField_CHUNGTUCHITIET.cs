namespace ERBus.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddField_CHUNGTUCHITIET : DbMigration
    {
        public override void Up()
        {
            AddColumn("ERBUS.CHUNGTU_CHITIET", "GIABANLE", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("ERBUS.CHUNGTU_CHITIET", "GIABANLE_VAT", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("ERBUS.CHUNGTU_CHITIET", "GIABANLE_VAT");
            DropColumn("ERBUS.CHUNGTU_CHITIET", "GIABANLE");
        }
    }
}
