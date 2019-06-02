namespace Data.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSensorListingAddTimeTicks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SensorListings", "TimeTicks", c => c.Long(nullable: false));
            DropColumn("dbo.SensorListings", "Time");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SensorListings", "Time", c => c.Time(nullable: false, precision: 7));
            DropColumn("dbo.SensorListings", "TimeTicks");
        }
    }
}
