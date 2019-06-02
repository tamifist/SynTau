namespace Data.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUniqueIndexOnCycleStep : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.CycleSteps", new[] { "SynthesisCycleId" });
            CreateIndex("dbo.CycleSteps", new[] { "Number", "SynthesisCycleId" }, unique: true, name: "IX_NumberAndSynthesisCycleId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.CycleSteps", "IX_NumberAndSynthesisCycleId");
            CreateIndex("dbo.CycleSteps", "SynthesisCycleId");
        }
    }
}
