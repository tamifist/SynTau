namespace Data.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReferenceBetweenCodonAndCodonUsage : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Codons", "AminoAcidId", "dbo.AminoAcids");
            DropIndex("dbo.Codons", new[] { "Value" });
            DropIndex("dbo.Codons", new[] { "AminoAcidId" });
            AddColumn("dbo.Codons", "Triplet", c => c.String(nullable: false, maxLength: 3));
            AddColumn("dbo.CodonUsages", "CodonId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Codons", "AminoAcidId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Codons", "Triplet", unique: true);
            CreateIndex("dbo.Codons", "AminoAcidId");
            CreateIndex("dbo.CodonUsages", "CodonId");
            AddForeignKey("dbo.CodonUsages", "CodonId", "dbo.Codons", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Codons", "AminoAcidId", "dbo.AminoAcids", "Id");
            DropColumn("dbo.Codons", "Value");
            DropColumn("dbo.CodonUsages", "Codon");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CodonUsages", "Codon", c => c.String(nullable: false, maxLength: 3));
            AddColumn("dbo.Codons", "Value", c => c.String(nullable: false, maxLength: 3));
            DropForeignKey("dbo.Codons", "AminoAcidId", "dbo.AminoAcids");
            DropForeignKey("dbo.CodonUsages", "CodonId", "dbo.Codons");
            DropIndex("dbo.CodonUsages", new[] { "CodonId" });
            DropIndex("dbo.Codons", new[] { "AminoAcidId" });
            DropIndex("dbo.Codons", new[] { "Triplet" });
            AlterColumn("dbo.Codons", "AminoAcidId", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.CodonUsages", "CodonId");
            DropColumn("dbo.Codons", "Triplet");
            CreateIndex("dbo.Codons", "AminoAcidId");
            CreateIndex("dbo.Codons", "Value", unique: true);
            AddForeignKey("dbo.Codons", "AminoAcidId", "dbo.AminoAcids", "Id", cascadeDelete: true);
        }
    }
}
