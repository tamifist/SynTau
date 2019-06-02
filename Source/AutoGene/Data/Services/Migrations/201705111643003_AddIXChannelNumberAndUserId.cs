namespace Data.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIXChannelNumberAndUserId : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ChannelConfigurations", new[] { "ChannelNumber" });
            DropIndex("dbo.ChannelConfigurations", new[] { "UserId" });
            CreateIndex("dbo.ChannelConfigurations", new[] { "ChannelNumber", "UserId" }, unique: true, name: "IX_ChannelNumberAndUserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ChannelConfigurations", "IX_ChannelNumberAndUserId");
            CreateIndex("dbo.ChannelConfigurations", "UserId");
            CreateIndex("dbo.ChannelConfigurations", "ChannelNumber", unique: true);
        }
    }
}
