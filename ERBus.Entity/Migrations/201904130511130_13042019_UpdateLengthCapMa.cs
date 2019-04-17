namespace ERBus.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _13042019_UpdateLengthCapMa : DbMigration
    {
        public override void Up()
        {
            AlterColumn("ERBUS.CAPMA", "LOAIMA", c => c.String(maxLength: 18));
            AlterColumn("ERBUS.CAPMA", "NHOMMA", c => c.String(maxLength: 18));
            AlterColumn("ERBUS.CAPMA", "GIATRI", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            AlterColumn("ERBUS.CAPMA", "GIATRI", c => c.String(maxLength: 8));
            AlterColumn("ERBUS.CAPMA", "NHOMMA", c => c.String(maxLength: 10));
            AlterColumn("ERBUS.CAPMA", "LOAIMA", c => c.String(maxLength: 10));
        }
    }
}
