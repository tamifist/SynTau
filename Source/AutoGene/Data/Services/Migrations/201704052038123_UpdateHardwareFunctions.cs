namespace Data.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateHardwareFunctions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HardwareFunctions", "FunctionType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.HardwareFunctions", "FunctionType");
        }
    }
}
