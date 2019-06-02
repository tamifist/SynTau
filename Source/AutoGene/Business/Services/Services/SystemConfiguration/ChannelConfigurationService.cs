using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Business.Contracts.Services.ChannelConfiguration;
using Business.Contracts.ViewModels.Common;
using Business.Contracts.ViewModels.SystemConfiguration;
using Data.Contracts;
using Data.Contracts.Entities.CycleEditor;
using Data.Contracts.Entities.SystemConfiguration;
using Services.Services.Common;
using Shared.Enum;
using Shared.Framework.Dependency;
using Shared.Framework.Security;

namespace Services.Services.SystemConfiguration
{
    public class ChannelConfigurationService: BaseService, IChannelConfigurationService, IDependency
    {
        public ChannelConfigurationService(IUnitOfWork unitOfWork, IIdentityStorage identityStorage)
            : base(unitOfWork, identityStorage)
        {
        }

        public async Task<IEnumerable<ChannelConfigurationItemViewModel>> GetChannelConfigurations()
        {
            var currentPrincipal = identityStorage.GetPrincipal();
            IEnumerable<ChannelConfiguration> allChannelConfigurations =
                await unitOfWork.GetAll<ChannelConfiguration>()
                .Include(x => x.HardwareFunction)
                .Where(x => x.UserId == currentPrincipal.UserId).ToListAsync();

            var allChannelConfigurationItems = new List<ChannelConfigurationItemViewModel>();
            foreach (ChannelConfiguration channelConfiguration in allChannelConfigurations.OrderByDescending(x => x.ChannelNumber))
            {
                allChannelConfigurationItems.Add(new ChannelConfigurationItemViewModel()
                {
                    Id = channelConfiguration.Id,
                    ChannelNumber = channelConfiguration.ChannelNumber,
                    StartNucleotide = channelConfiguration.StartNucleotide,
                    HardwareFunction = new HardwareFunctionItemViewModel()
                    {
                        Id = channelConfiguration.HardwareFunction.Id,
                        Name = channelConfiguration.HardwareFunction.Name,
                    }
                });
            }

            return allChannelConfigurationItems;
        }

        public async Task CreateOrUpdateChannelConfiguration(ChannelConfigurationItemViewModel item)
        {
            ChannelConfiguration entity;
            if (string.IsNullOrWhiteSpace(item.Id))
            {
                entity = new ChannelConfiguration();
                entity.User = GetCurrentUser();
            }
            else
            {
                entity = await unitOfWork.GetAll<ChannelConfiguration>()
                    .Include(x => x.User)
                    .Include(x => x.HardwareFunction)
                    .SingleAsync(x => x.Id == item.Id);
            }

            entity.ChannelNumber = item.ChannelNumber > 0 ? item.ChannelNumber : 1;
            entity.StartNucleotide = !string.IsNullOrWhiteSpace(item.StartNucleotide) ? item.StartNucleotide : "A";

            if (item.HardwareFunction != null && !string.IsNullOrWhiteSpace(item.HardwareFunction.Id))
            {
                entity.HardwareFunction = unitOfWork.GetById<HardwareFunction>(item.HardwareFunction.Id);
            }
            else
            {
                entity.HardwareFunction =
                    await unitOfWork.GetAll<HardwareFunction>().Where(x => x.FunctionType == HardwareFunctionType.ActivateChannel).FirstAsync();
            }

            unitOfWork.InsertOrUpdate(entity);
            unitOfWork.Commit();
        }

        public Task DeleteChannelConfiguration(string channelConfigurationId)
        {
            return unitOfWork.DeleteWhere<ChannelConfiguration>(x => x.Id == channelConfigurationId);
        }
    }
}