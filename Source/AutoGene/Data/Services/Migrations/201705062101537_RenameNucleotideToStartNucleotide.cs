namespace Data.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameNucleotideToStartNucleotide : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChannelConfigurations", "StartNucleotide", c => c.String(nullable: false, maxLength: 1));
            DropColumn("dbo.ChannelConfigurations", "Nucleotide");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ChannelConfigurations", "Nucleotide", c => c.String(nullable: false, maxLength: 1));
            DropColumn("dbo.ChannelConfigurations", "StartNucleotide");
        }
    }
}
