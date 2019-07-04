namespace Data.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeUserCountryOptional : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "Country", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "Country", c => c.Int(nullable: false));
        }
    }
}
