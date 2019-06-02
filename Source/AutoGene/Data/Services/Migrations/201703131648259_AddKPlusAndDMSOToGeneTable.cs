namespace Data.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddKPlusAndDMSOToGeneTable : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.UserRoles", newName: "RoleUsers");
            DropPrimaryKey("dbo.RoleUsers");
            AddColumn("dbo.GeneFragments", "Tm", c => c.Single(nullable: false));
            AddPrimaryKey("dbo.RoleUsers", new[] { "Role_Id", "User_Id" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.RoleUsers");
            DropColumn("dbo.GeneFragments", "Tm");
            AddPrimaryKey("dbo.RoleUsers", new[] { "User_Id", "Role_Id" });
            RenameTable(name: "dbo.RoleUsers", newName: "UserRoles");
        }
    }
}
