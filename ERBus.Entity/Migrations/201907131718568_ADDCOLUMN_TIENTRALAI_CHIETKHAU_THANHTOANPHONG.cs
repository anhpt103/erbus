namespace ERBus.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ADDCOLUMN_TIENTRALAI_CHIETKHAU_THANHTOANPHONG : DbMigration
    {
        public override void Up()
        {
            AddColumn("ERBUS.THANHTOAN_DATPHONG_CHITIET", "TYLE_CHIETKHAU", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("ERBUS.THANHTOAN_DATPHONG_CHITIET", "TIEN_CHIETKHAU", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("ERBUS.THANHTOAN_DATPHONG", "TIENKHACH_TRA", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("ERBUS.THANHTOAN_DATPHONG", "TIEN_TRALAI_KHACH", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("ERBUS.THANHTOAN_DATPHONG", "TIEN_TRALAI_KHACH");
            DropColumn("ERBUS.THANHTOAN_DATPHONG", "TIENKHACH_TRA");
            DropColumn("ERBUS.THANHTOAN_DATPHONG_CHITIET", "TIEN_CHIETKHAU");
            DropColumn("ERBUS.THANHTOAN_DATPHONG_CHITIET", "TYLE_CHIETKHAU");
        }
    }
}
