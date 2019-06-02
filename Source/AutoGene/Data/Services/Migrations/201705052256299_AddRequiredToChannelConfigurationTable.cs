namespace Data.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequiredToChannelConfigurationTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ChannelConfigurations", "UserId", "dbo.Users");
            DropIndex("dbo.ChannelConfigurations", new[] { "UserId" });
            AlterColumn("dbo.ChannelConfigurations", "UserId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.ChannelConfigurations", "UserId");
            AddForeignKey("dbo.ChannelConfigurations", "UserId", "dbo.Users", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChannelConfigurations", "UserId", "dbo.Users");
            DropIndex("dbo.ChannelConfigurations", new[] { "UserId" });
            AlterColumn("dbo.ChannelConfigurations", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.ChannelConfigurations", "UserId");
            AddForeignKey("dbo.ChannelConfigurations", "UserId", "dbo.Users", "Id");
        }
    }
}
