namespace Data.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFragmentNumberColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GeneFragments", "FragmentNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GeneFragments", "FragmentNumber");
        }
    }
}
