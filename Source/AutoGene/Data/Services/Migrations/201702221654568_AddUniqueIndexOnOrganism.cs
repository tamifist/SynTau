namespace Data.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUniqueIndexOnOrganism : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Organism", "Name", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Organism", new[] { "Name" });
        }
    }
}
