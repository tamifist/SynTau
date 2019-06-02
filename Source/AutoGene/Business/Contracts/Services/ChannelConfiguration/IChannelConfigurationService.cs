using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Contracts.ViewModels.SystemConfiguration;

namespace Business.Contracts.Services.ChannelConfiguration
{
    public interface IChannelConfigurationService
    {
        Task<IEnumerable<ChannelConfigurationItemViewModel>> GetChannelConfigurations();

        Task CreateOrUpdateChannelConfiguration(ChannelConfigurationItemViewModel channelConfigurationItemViewModel);

        Task DeleteChannelConfiguration(string channelConfigurationId);
    }
}