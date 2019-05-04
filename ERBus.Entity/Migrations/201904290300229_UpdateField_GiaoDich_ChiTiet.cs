namespace ERBus.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateField_GiaoDich_ChiTiet : DbMigration
    {
        public override void Up()
        {
            AlterColumn("ERBUS.GIAODICH", "LOAI_GIAODICH", c => c.String(nullable: false, maxLength: 15));
        }
        
        public override void Down()
        {
            AlterColumn("ERBUS.GIAODICH", "LOAI_GIAODICH", c => c.String(nullable: false, maxLength: 10));
        }
    }
}
