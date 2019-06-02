namespace Data.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrganismIdToGeneTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Genes", "OrganismId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Genes", "OrganismId");
            AddForeignKey("dbo.Genes", "OrganismId", "dbo.Organism", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Genes", "OrganismId", "dbo.Organism");
            DropIndex("dbo.Genes", new[] { "OrganismId" });
            DropColumn("dbo.Genes", "OrganismId");
        }
    }
}
