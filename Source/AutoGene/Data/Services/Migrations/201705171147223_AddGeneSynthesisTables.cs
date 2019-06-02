namespace Data.Services.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class AddGeneSynthesisTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GeneSynthesisActivities",
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
                        ChannelApiFunctionId = c.String(nullable: false, maxLength: 128),
                        DNASequence = c.String(nullable: false),
                        Status = c.Int(nullable: false),
                        SynthesisCycleId = c.String(nullable: false, maxLength: 128),
                        GeneSynthesisProcessId = c.String(nullable: false, maxLength: 128),
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
                .ForeignKey("dbo.HardwareFunctions", t => t.ChannelApiFunctionId, cascadeDelete: true)
                .ForeignKey("dbo.GeneSynthesisProcesses", t => t.GeneSynthesisProcessId, cascadeDelete: true)
                .ForeignKey("dbo.SynthesisCycles", t => t.SynthesisCycleId, cascadeDelete: true)
                .Index(t => new { t.ChannelNumber, t.GeneSynthesisProcessId }, unique: true, name: "IX_ChannelNumberAndGeneSynthesisProcessId")
                .Index(t => t.ChannelApiFunctionId)
                .Index(t => t.SynthesisCycleId)
                .Index(t => t.CreatedAt, clustered: true);
            
            CreateTable(
                "dbo.GeneSynthesisProcesses",
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
                        DenaturationTempGeneAssembly = c.Single(nullable: false),
                        AnnealingTempGeneAssembly = c.Single(nullable: false),
                        ElongationTempGeneAssembly = c.Single(nullable: false),
                        DenaturationTempGeneAmplification = c.Single(nullable: false),
                        AnnealingTempGeneAmplification = c.Single(nullable: false),
                        ElongationTempGeneAmplification = c.Single(nullable: false),
                        NumberOfGeneAssemblyCycles = c.Int(nullable: false),
                        NumberOfGeneAmplificationCycles = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        GeneId = c.String(nullable: false, maxLength: 128),
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
                .ForeignKey("dbo.Genes", t => t.GeneId, cascadeDelete: true)
                .Index(t => t.GeneId)
                .Index(t => t.CreatedAt, clustered: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GeneSynthesisActivities", "SynthesisCycleId", "dbo.SynthesisCycles");
            DropForeignKey("dbo.GeneSynthesisActivities", "GeneSynthesisProcessId", "dbo.GeneSynthesisProcesses");
            DropForeignKey("dbo.GeneSynthesisProcesses", "GeneId", "dbo.Genes");
            DropForeignKey("dbo.GeneSynthesisActivities", "ChannelApiFunctionId", "dbo.HardwareFunctions");
            DropIndex("dbo.GeneSynthesisProcesses", new[] { "CreatedAt" });
            DropIndex("dbo.GeneSynthesisProcesses", new[] { "GeneId" });
            DropIndex("dbo.GeneSynthesisActivities", new[] { "CreatedAt" });
            DropIndex("dbo.GeneSynthesisActivities", new[] { "SynthesisCycleId" });
            DropIndex("dbo.GeneSynthesisActivities", new[] { "ChannelApiFunctionId" });
            DropIndex("dbo.GeneSynthesisActivities", "IX_ChannelNumberAndGeneSynthesisProcessId");
            DropTable("dbo.GeneSynthesisProcesses",
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
            DropTable("dbo.GeneSynthesisActivities",
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
