namespace Data.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTimeToGeneSynthesisProcess : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GeneSynthesisProcesses", "DenaturationTimeGeneAssembly", c => c.Int(nullable: false));
            AddColumn("dbo.GeneSynthesisProcesses", "AnnealingTimeGeneAssembly", c => c.Int(nullable: false));
            AddColumn("dbo.GeneSynthesisProcesses", "ElongationTimeGeneAssembly", c => c.Int(nullable: false));
            AddColumn("dbo.GeneSynthesisProcesses", "DenaturationTimeGeneAmplification", c => c.Int(nullable: false));
            AddColumn("dbo.GeneSynthesisProcesses", "AnnealingTimeGeneAmplification", c => c.Int(nullable: false));
            AddColumn("dbo.GeneSynthesisProcesses", "ElongationTimeGeneAmplification", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GeneSynthesisProcesses", "ElongationTimeGeneAmplification");
            DropColumn("dbo.GeneSynthesisProcesses", "AnnealingTimeGeneAmplification");
            DropColumn("dbo.GeneSynthesisProcesses", "DenaturationTimeGeneAmplification");
            DropColumn("dbo.GeneSynthesisProcesses", "ElongationTimeGeneAssembly");
            DropColumn("dbo.GeneSynthesisProcesses", "AnnealingTimeGeneAssembly");
            DropColumn("dbo.GeneSynthesisProcesses", "DenaturationTimeGeneAssembly");
        }
    }
}
