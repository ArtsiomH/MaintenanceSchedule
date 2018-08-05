namespace MaintenanceScheduleDataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Schedule : DbMigration
    {
        public override void Up()
        {            
            CreateTable(
                "dbo.Schedules",
                c => new
                    {
                        ScheduleId = c.Int(nullable: false, identity: true),
                        Year = c.Int(nullable: false),
                        Condition = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ScheduleId);                  
        }
        
        public override void Down()
        {
            DropTable("dbo.Schedules");
        }
    }
}
