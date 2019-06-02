using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text.RegularExpressions;
using Data.Contracts.Entities.CycleEditor;
using Data.Contracts.Entities.Diagnostic;
using Data.Contracts.Entities.GeneEditor;
using Data.Contracts.Entities.GeneSynthesizer;
using Data.Contracts.Entities.Identity;
using Data.Contracts.Entities.OligoSynthesizer;
using Data.Contracts.Entities.Settings;
using Data.Contracts.Entities.SystemConfiguration;
using Data.Contracts.Entities.SystemMonitor;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Tables;

namespace Data.Services
{
    public class CommonDbContext : EntityContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<AminoAcid> AminoAcids { get; set; }

        public DbSet<Codon> Codons { get; set; }

        public DbSet<Organism> Organisms { get; set; }

        public DbSet<CodonUsage> CodonUsages { get; set; }

        public DbSet<Sensor> Sensors { get; set; }

        public DbSet<SensorListing> SensorListings { get; set; }

        public DbSet<Gene> Genes { get; set; }

        public DbSet<GeneFragment> GeneFragments { get; set; }

        public DbSet<HardwareFunction> HardwareFunctions { get; set; }

        public DbSet<SynthesisCycle> SynthesisCycles { get; set; }

        public DbSet<CycleStep> CycleSteps { get; set; }

        public DbSet<SynthesizerSetting> SynthesizerSettings { get; set; }

        public DbSet<ChannelConfiguration> ChannelConfigurations { get; set; }

        public DbSet<OligoSynthesisProcess> OligoSynthesisProcesses { get; set; }

        public DbSet<OligoSynthesisActivity> OligoSynthesisActivities { get; set; }

        public DbSet<GeneSynthesisProcess> GeneSynthesisProcesses { get; set; }

        public DbSet<GeneSynthesisActivity> GeneSynthesisActivities { get; set; }

        public DbSet<Log> Logs { get; set; }

        public static CommonDbContext Create()
        {
            return new CommonDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Configuration.ProxyCreationEnabled = false;
            //            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            //            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}