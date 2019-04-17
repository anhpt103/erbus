namespace ERBus.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateGiaoDichChiTiet_Index : DbMigration
    {
        public override void Up()
        {
            AddColumn("ERBUS.GIAODICH_CHITIET", "SAPXEP", c => c.Decimal(precision: 10, scale: 0));
            DropColumn("ERBUS.GIAODICH_CHITIET", "INDEX");
        }
        
        public override void Down()
        {
            AddColumn("ERBUS.GIAODICH_CHITIET", "INDEX", c => c.Decimal(precision: 10, scale: 0));
            DropColumn("ERBUS.GIAODICH_CHITIET", "SAPXEP");
        }
    }
}
