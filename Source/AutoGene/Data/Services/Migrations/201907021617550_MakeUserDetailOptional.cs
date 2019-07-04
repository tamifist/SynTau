namespace Data.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeUserDetailOptional : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "Organization", c => c.String(maxLength: 255));
            AlterColumn("dbo.Users", "LabGroup", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "LabGroup", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Users", "Organization", c => c.String(nullable: false, maxLength: 255));
        }
    }
}
