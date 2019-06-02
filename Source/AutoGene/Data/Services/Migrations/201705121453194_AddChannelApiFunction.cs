namespace Data.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddChannelApiFunction : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OligoSynthesisActivities", "ChannelApiFunctionId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.OligoSynthesisActivities", "ChannelApiFunctionId");
            AddForeignKey("dbo.OligoSynthesisActivities", "ChannelApiFunctionId", "dbo.HardwareFunctions", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OligoSynthesisActivities", "ChannelApiFunctionId", "dbo.HardwareFunctions");
            DropIndex("dbo.OligoSynthesisActivities", new[] { "ChannelApiFunctionId" });
            DropColumn("dbo.OligoSynthesisActivities", "ChannelApiFunctionId");
        }
    }
}
