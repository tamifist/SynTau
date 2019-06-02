using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Business.Contracts.Services.ChannelConfiguration;
using Business.Contracts.Services.CycleEditor;
using Business.Contracts.ViewModels.CycleEditor;
using Business.Contracts.ViewModels.SystemConfiguration;

namespace AutoGene.Presentation.Host.Controllers.SystemConfiguration
{
    public class ChannelConfigurationController : ApiController
    {
        private readonly IChannelConfigurationService channelConfigurationService;

        public ChannelConfigurationController(IChannelConfigurationService channelConfigurationService)
        {
            this.channelConfigurationService = channelConfigurationService;
        }

        public async Task<IEnumerable<ChannelConfigurationItemViewModel>> Get()
        {
            var channelConfigurationItems = await channelConfigurationService.GetChannelConfigurations();
            return channelConfigurationItems;
        }

        public async Task Put(IEnumerable<ChannelConfigurationItemViewModel> items)
        {
            foreach (ChannelConfigurationItemViewModel item in items)
            {
                await channelConfigurationService.CreateOrUpdateChannelConfiguration(item);
            }
        }

        public async Task Delete(IEnumerable<ChannelConfigurationItemViewModel> itemsToDelete)
        {
            foreach (ChannelConfigurationItemViewModel item in itemsToDelete)
            {
                await channelConfigurationService.DeleteChannelConfiguration(item.Id);
            }
        }
    }
}