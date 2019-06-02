using System;
using System.Diagnostics;
using System.Threading.Tasks;
using AutoGene.Mobile.Abstractions;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Shared.DTO.Responses;
using Shared.DTO.Responses.Common;
using Shared.DTO.Responses.OligoSynthesizer;
using Shared.DTO.Responses.Diagnostic;
using Shared.DTO.Responses.GeneSynthesizer;
using Xamarin.Forms;

namespace AutoGene.Mobile.Services
{
    public class AzureCloudService : ICloudService
    {
        /// <summary>
        /// Constructor: Create a new Cloud Service connection.
        /// </summary>
        public AzureCloudService()
        {
            Client = new MobileServiceClient(Shared.Resources.Identifiers.Environment.MobileServiceUrl);

            PlatformProvider = DependencyService.Get<IPlatform>();
            if (PlatformProvider == null)
            {
                throw new InvalidOperationException("No Platform Provider");
            }
        }

        /// <summary>
        /// The Client reference to the Azure Mobile App
        /// </summary>
        public MobileServiceClient Client { get; set; }

        private IPlatform PlatformProvider { get; set; }
        
        #region Offline Sync
        private async Task InitializeAsync()
        {
            // Short circuit - local database is already initialized
            if (Client.SyncContext.IsInitialized)
            {
                Debug.WriteLine("InitializeAsync: Short Circuit");
                return;
            }

            // Create a reference to the local sqlite store
            Debug.WriteLine("InitializeAsync: Initializing store");
            var store = new MobileServiceSQLiteStore(PlatformProvider.GetSyncStore());

            // Define the database schema
            Debug.WriteLine("InitializeAsync: Defining Datastore");
            store.DefineTable<Log>();
            store.DefineTable<SynthesizerSetting>();
            store.DefineTable<HardwareFunction>();
            store.DefineTable<OligoSynthesisProcess>();
            store.DefineTable<GeneSynthesisProcess>();

            // Actually create the store and update the schema
            Debug.WriteLine("InitializeAsync: Initializing SyncContext");
            await Client.SyncContext.InitializeAsync(store);

            // Do the sync
            Debug.WriteLine("InitializeAsync: Syncing Offline Cache");
            await SyncOfflineCacheAsync();
        }

        public async Task SyncOfflineCacheAsync()
        {
            Debug.WriteLine("SyncOfflineCacheAsync: Initializing...");
            await InitializeAsync();

            // Push the Operations Queue to the mobile backend
            Debug.WriteLine("SyncOfflineCacheAsync: Pushing Changes");
            await Client.SyncContext.PushAsync();

            // Pull each sync table

            Debug.WriteLine("SyncOfflineCacheAsync: Pulling Logs table");
            var logsTable = await GetTableAsync<Log>();
            await logsTable.PullAsync();

            Debug.WriteLine("SyncOfflineCacheAsync: Pulling SynthesizerSettings table");
            var synthesizerSettingsTable = await GetTableAsync<SynthesizerSetting>();
            await synthesizerSettingsTable.PullAsync();

            Debug.WriteLine("SyncOfflineCacheAsync: Pulling HardwareFunctions table");
            var hardwareFunctionsTable = await GetTableAsync<HardwareFunction>();
            await hardwareFunctionsTable.PullAsync();

            Debug.WriteLine("SyncOfflineCacheAsync: Pulling OligoSynthesisProcess table");
            var oligoSynthesisProcessTable = await GetTableAsync<OligoSynthesisProcess>();
            await oligoSynthesisProcessTable.PullAsync();

            Debug.WriteLine("SyncOfflineCacheAsync: Pulling GeneSynthesisProcess table");
            var geneSynthesisProcessTable = await GetTableAsync<GeneSynthesisProcess>();
            await geneSynthesisProcessTable.PullAsync();
        }
        #endregion
        
        /// <summary>
        /// Returns a link to the specific table.
        /// </summary>
        /// <typeparam name="T">The model</typeparam>
        /// <returns>The table reference</returns>
        public async Task<ICloudTable<T>> GetTableAsync<T>() where T : BaseDTO
        {
            await InitializeAsync();
            return new AzureCloudTable<T>(Client);
        }
        
    }
}