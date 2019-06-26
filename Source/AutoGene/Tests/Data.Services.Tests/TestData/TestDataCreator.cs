using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using Data.Contracts.Entities;
using Data.Contracts.Entities.CycleEditor;
using Data.Contracts.Entities.GeneEditor;
using Data.Contracts.Entities.GeneOrder;
using Data.Contracts.Entities.GeneSynthesizer;
using Data.Contracts.Entities.Identity;
using Data.Contracts.Entities.OligoSynthesizer;
using Data.Contracts.Entities.Settings;
using Data.Contracts.Entities.SystemConfiguration;
using Data.Contracts.Entities.SystemMonitor;
using Microsoft.Azure.Mobile.Server;
using Shared.Enum;

namespace Data.Services.Tests.TestData
{
    public class TestDataCreator
    {
        private readonly EntityContext dbContext;
        private readonly Dictionary<Type, object> identityMap = new Dictionary<Type, object>();
        private readonly Dictionary<Type, Action> initializers = new Dictionary<Type, Action>();

        public TestDataCreator(EntityContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void InitializeGraph()
        {
            initializers.Values.ToList().ForEach(i => i());
        }

        public bool HasObjectOfType<T>()
        {
            return identityMap.ContainsKey(typeof(T));
        }

        public void Attach<T>(T entity)
        {
            Console.WriteLine("Attaching instance of {0}", typeof(T).Name);
            identityMap[typeof(T)] = entity;
        }

        protected T GetOrCreate<T>(Func<T> entityCreator, Action<T> navigationInitializer = null)
        {
            if (identityMap.ContainsKey(typeof(T)))
            {
                return (T)identityMap[typeof(T)];
            }
            Console.WriteLine("Creating instance of {0}", typeof(T).Name);
            T entity = entityCreator();
            identityMap.Add(typeof(T), entity);
            if (navigationInitializer != null)
            {
                initializers[typeof(T)] = () => navigationInitializer(entity);
            }
            return entity;
        }

        protected T GetAndInitialize<T>(Action<T> initialize) where T : new()
        {
            T instance = GetOrCreate(() => new T());
            initializers[typeof(T)] = () => initialize(instance);
            return instance;
        }

        public void DeleteCreatedEntities()
        {
            foreach (var entry in identityMap)
            {
                Console.WriteLine("Removing instance of {0} from DbContext", entry.Key.Name);
                DbSet dbSet = dbContext.Set(entry.Key);
                bool isEntityExists = dbSet.Find(((Entity)entry.Value).Id) != null;

                if (isEntityExists)
                {
                    dbSet.Remove(entry.Value);
                }
            }
            dbContext.SaveChanges();
        }

        public T Create<T>()
        {
            string name = "Create" + typeof(T).Name;
            MethodInfo methodInfo = GetType().GetMethod(name);
            if (methodInfo == null)
            {
                throw new NotImplementedException("Implement " + methodInfo + " method in TestDataCreator");
            }
            return (T)methodInfo.Invoke(this, null);
        }

        public User CreateUser(string email, string firstName, string lastName, string password, string roleName, string roleDescription)
        {
            var user = new User
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                Password = password,
                PasswordSalt = "test"
            };

            user.Roles.Add(CreateRole(roleName, roleDescription));

            return GetOrCreate(() => user);
        }

        public User CreateTestUser()
        {
            var user = new User
            {
                Email = "test@test.com",
                FirstName = "Test",
                LastName = "User",
                Password = "12345678",
                PasswordSalt = "test",
                Organization = "Test Organization",
                LabGroup = "Test Lab",
                Country = CountryEnum.Belarus,
            };

            return GetOrCreate(() => user);
        }

        public User CreateTestUserWithRole(string roleName)
        {
            User user = CreateTestUser();

            user.Roles.Add(CreateRole(roleName, null));

            return GetOrCreate(() => user);
        }

        public ChannelConfiguration CreateChannelConfiguration()
        {
            var channelConfiguration = new ChannelConfiguration();

            channelConfiguration.ChannelNumber = 999;
            channelConfiguration.StartNucleotide = "A";

            channelConfiguration.User = CreateTestUser();
            channelConfiguration.HardwareFunction = CreateHardwareFunction();

            return GetOrCreate(() => channelConfiguration);
        }

        public Role CreateRole(string roleName, string roleDescription)
        {
            var role = new Role
            {
                Name = roleName,
                Description = roleDescription
            };

            return GetOrCreate(() => role);
        }

        public AminoAcid CreateAminoAcid()
        {
            var aminoAcid = new AminoAcid()
            {
                Name = "TestAminoAcid",
                Code = "Z"
            };

            aminoAcid.Codons.Add(CreateCodon());

            return GetOrCreate(() => aminoAcid);
        }

        public Codon CreateCodon()
        {
            var codon = new Codon()
            {
                Triplet = "ZZZ"
            };

            return GetOrCreate(() => codon);
        }

        public Organism CreateOrganism()
        {
            var organism = new Organism()
            {
                Name = "TestOrganism"
            };

            return GetOrCreate(() => organism);
        }

        public CodonUsage CreateCodonUsage()
        {
            var codonUsage = new CodonUsage()
            {
                Codon = CreateCodon(),
                Frequency = 0.57f
            };

            return GetOrCreate(() => codonUsage);
        }

        public SensorListing CreateSensorListing()
        {
            var sensorListing = new SensorListing()
            {
                Time = DateTimeOffset.UtcNow.TimeOfDay
            };

            sensorListing.Sensors.Add(CreateSensor());

            return GetOrCreate(() => sensorListing);
        }

        public Sensor CreateSensor()
        {
            var sensor = new Sensor()
            {
                Type = SensorType.WasteAbsorbanceDetector1,
                Value = 1.1f,
            };

            return GetOrCreate(() => sensor);
        }

        public Gene CreateGene()
        {
            var gene = new Gene()
            {
                Name = "TestGene",
                DNASequence = "TTTTTT",
                Organism = CreateOrganism(),
                User = CreateTestUser(),
            };

            gene.GeneFragments.Add(CreateGeneFragment());

            return GetOrCreate(() => gene);
        }

        public GeneOrder CreateGeneOrder()
        {
            var geneOrder = new GeneOrder()
            {
                Name = "TestGene",
                Sequence = "TTTTTT",
                User = CreateTestUser(),
            };

            return GetOrCreate(() => geneOrder);
        }

        public GeneFragment CreateGeneFragment()
        {
            GeneFragment geneFragment = new GeneFragment()
            {
                OligoSequence = "TTT",
                OverlappingLength = 1,
            };

            return GetOrCreate(() => geneFragment);
        }

        public HardwareFunction CreateHardwareFunction()
        {
            HardwareFunction hardwareFunction = new HardwareFunction()
            {
                Number = 999,
                Name = "test",
                Description = "test description",
                ApiUrl = "/macro/f999",
                HttpMethodType = HttpMethodType.POST
            };

            return GetOrCreate(() => hardwareFunction);
        }

        public SynthesisCycle CreateSynthesisCycle()
        {
            SynthesisCycle synthesisCycle = new SynthesisCycle();
            synthesisCycle.Name = "test cycle";

            return GetOrCreate(() => synthesisCycle);
        }

        public CycleStep CreateCycleStep()
        {
            CycleStep cycleStep = new CycleStep()
            {
                Number = 1,
                StepTime = 3,
                SynthesisCycle = CreateSynthesisCycle(),
                HardwareFunction = CreateHardwareFunction()
            };

            return GetOrCreate(() => cycleStep);
        }

        public OligoSynthesisProcess CreateOligoSynthesisProcess()
        {
            OligoSynthesisProcess oligoSynthesisProcess = new OligoSynthesisProcess();
            oligoSynthesisProcess.User = CreateTestUser();

            return GetOrCreate(() => oligoSynthesisProcess);
        }

        public OligoSynthesisActivity CreateOligoSynthesisActivity()
        {
            OligoSynthesisActivity oligoSynthesisActivity = new OligoSynthesisActivity()
            {
                DNASequence = "GG",
                OligoSynthesisProcess = CreateOligoSynthesisProcess(),
                SynthesisCycle = CreateSynthesisCycle(),
                ChannelApiFunction = CreateHardwareFunction(),
            };

            return GetOrCreate(() => oligoSynthesisActivity);
        }

        public GeneSynthesisProcess CreateGeneSynthesisProcess()
        {
            var geneSynthesisProcess = new GeneSynthesisProcess();
            geneSynthesisProcess.Gene = CreateGene();

            return GetOrCreate(() => geneSynthesisProcess);
        }

        public GeneSynthesisActivity CreateGeneSynthesisActivity()
        {
            var geneSynthesisActivity = new GeneSynthesisActivity()
            {
                DNASequence = "GG",
                GeneSynthesisProcess = CreateGeneSynthesisProcess(),
                SynthesisCycle = CreateSynthesisCycle(),
                ChannelApiFunction = CreateHardwareFunction(),
            };

            return GetOrCreate(() => geneSynthesisActivity);
        }

        public SynthesizerSetting CreateSynthesizerSetting()
        {
            SynthesizerSetting synthesizerSetting = new SynthesizerSetting()
            {
                AppServiceUrl = "http://test.com",
                SynthesizerApiUrl = "http://100.100.100.100"
            };

            return GetOrCreate(() => synthesizerSetting);
        }
    }
}