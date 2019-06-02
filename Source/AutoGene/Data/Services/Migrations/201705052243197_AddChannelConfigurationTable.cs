namespace Data.Services.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class AddChannelConfigurationTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChannelConfigurations",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "Id")
                                },
                            }),
                        ChannelNumber = c.Int(nullable: false),
                        Nucleotide = c.String(nullable: false, maxLength: 1),
                        UserId = c.String(maxLength: 128),
                        HardwareFunctionId = c.String(nullable: false, maxLength: 128),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion",
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "Version")
                                },
                            }),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "CreatedAt")
                                },
                            }),
                        UpdatedAt = c.DateTimeOffset(precision: 7,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "UpdatedAt")
                                },
                            }),
                        Deleted = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "Deleted")
                                },
                            }),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.HardwareFunctions", t => t.HardwareFunctionId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.ChannelNumber, unique: true)
                .Index(t => t.UserId)
                .Index(t => t.HardwareFunctionId)
                .Index(t => t.CreatedAt, clustered: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChannelConfigurations", "UserId", "dbo.Users");
            DropForeignKey("dbo.ChannelConfigurations", "HardwareFunctionId", "dbo.HardwareFunctions");
            DropIndex("dbo.ChannelConfigurations", new[] { "CreatedAt" });
            DropIndex("dbo.ChannelConfigurations", new[] { "HardwareFunctionId" });
            DropIndex("dbo.ChannelConfigurations", new[] { "UserId" });
            DropIndex("dbo.ChannelConfigurations", new[] { "ChannelNumber" });
            DropTable("dbo.ChannelConfigurations",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "CreatedAt",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "CreatedAt" },
                        }
                    },
                    {
                        "Deleted",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "Deleted" },
                        }
                    },
                    {
                        "Id",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "Id" },
                        }
                    },
                    {
                        "UpdatedAt",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "UpdatedAt" },
                        }
                    },
                    {
                        "Version",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "Version" },
                        }
                    },
                });
        }
    }
}
