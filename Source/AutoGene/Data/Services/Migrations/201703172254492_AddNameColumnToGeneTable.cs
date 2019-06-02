namespace Data.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNameColumnToGeneTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Genes", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Genes", "Name");
        }
    }
}
