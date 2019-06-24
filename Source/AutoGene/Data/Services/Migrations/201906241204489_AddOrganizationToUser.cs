namespace Data.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrganizationToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Organization", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.Users", "LabGroup", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.Users", "Country", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Country");
            DropColumn("dbo.Users", "LabGroup");
            DropColumn("dbo.Users", "Organization");
        }
    }
}
