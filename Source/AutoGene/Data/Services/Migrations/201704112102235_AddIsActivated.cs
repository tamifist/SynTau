namespace Data.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsActivated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HardwareFunctions", "IsActivated", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.HardwareFunctions", "IsActivated");
        }
    }
}
