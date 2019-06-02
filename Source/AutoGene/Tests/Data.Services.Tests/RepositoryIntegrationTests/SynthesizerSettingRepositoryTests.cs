using Data.Contracts.Entities.GeneEditor;
using Data.Contracts.Entities.Settings;
using Data.Services.Tests.RepositoryIntegrationTests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Data.Services.Tests.RepositoryIntegrationTests
{
    [TestClass]
    public class SynthesizerSettingRepositoryTests : RepositoryIntegrationTests<SynthesizerSetting>
    {
        protected override SynthesizerSetting CreateEntity()
        {
            return EntityCreator.CreateSynthesizerSetting();
        }
    }
}