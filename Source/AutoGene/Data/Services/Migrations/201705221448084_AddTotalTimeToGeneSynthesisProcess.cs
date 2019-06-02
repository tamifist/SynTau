namespace Data.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTotalTimeToGeneSynthesisProcess : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GeneSynthesisActivities", "TotalTime", c => c.Int(nullable: false));
            AddColumn("dbo.GeneSynthesisProcesses", "TotalTime", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GeneSynthesisProcesses", "TotalTime");
            DropColumn("dbo.GeneSynthesisActivities", "TotalTime");
        }
    }
}
