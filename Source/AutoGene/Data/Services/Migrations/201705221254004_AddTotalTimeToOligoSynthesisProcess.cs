namespace Data.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTotalTimeToOligoSynthesisProcess : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OligoSynthesisActivities", "TotalTime", c => c.Int(nullable: false));
            AddColumn("dbo.OligoSynthesisProcesses", "TotalTime", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OligoSynthesisProcesses", "TotalTime");
            DropColumn("dbo.OligoSynthesisActivities", "TotalTime");
        }
    }
}
