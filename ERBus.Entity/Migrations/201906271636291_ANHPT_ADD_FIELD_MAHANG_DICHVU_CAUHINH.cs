namespace ERBus.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ANHPT_ADD_FIELD_MAHANG_DICHVU_CAUHINH : DbMigration
    {
        public override void Up()
        {
            AddColumn("ERBUS.CAUHINH_LOAIPHONG", "MAHANG_DICHVU", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            DropColumn("ERBUS.CAUHINH_LOAIPHONG", "MAHANG_DICHVU");
        }
    }
}
