using System;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoGene.Mobile.Helpers;
using Microsoft.AspNet.SignalR.Client;
using Shared.DTO.Responses;
using Shared.Resources;

namespace AutoGene.Mobile.Services
{
    public class SignalService
    {
        private IHubProxy _hubProxy;
        private readonly HubConnection _hubConnection;

        public SignalService(string backendApiUrl)
        {
            _hubConnection = new HubConnection(backendApiUrl)
            {
                TraceLevel = TraceLevels.All,
                TraceWriter = new DebugTextWriter("SignalR"),
            };
        }

        public event EventHandler<SignalMessage> MessageReceived;

        public async Task<string> Connect()
        {
            try
            {
                if (_hubProxy == null)
                {
                    _hubProxy = _hubConnection.CreateHubProxy(Identifiers.SignalHubName);

                    _hubProxy.On<SignalMessage>("addMessage", message =>
                    {
                        MessageReceived?.Invoke(this, message);
                    });
                }

                if (_hubConnection.State == ConnectionState.Disconnected)
                {
                    await _hubConnection.Start();
                }
            }
            catch (Exception ex)
            {
                return $"[SignalR] Failed to connect to Hub: {Identifiers.SignalHubName}. Error: {ex.Message}";
            }
            
            return $"[SignalR] Connected to Hub: {Identifiers.SignalHubName}";
        }

        public async Task<string> JoinGroup(string groupId)
        {
            try
            {
                await _hubProxy.Invoke("JoinGroup", groupId);
            }
            catch (Exception ex)
            {
                return $"[SignalR] Failed to join to group: {groupId}. Error: {ex.Message}";
                //_insightsService.ReportException(ex);
            }

            return $"[SignalR] Joined to group: {groupId}";
        }

        //public async Task SendMessage(SignalMessage message)
        //{
        //    try
        //    {
        //        if (_hubConnection.State == ConnectionState.Disconnected)
        //        {
        //            await Connect();
        //        }

        //        await _hubProxy.Invoke("Send", message);
        //    }
        //    catch (InvalidOperationException ex)
        //    {
        //        //_insightsService.ReportException(ex);
        //    }
        //}
        
        //public async Task LeaveGroup(string groupId)
        //{
        //    try
        //    {
        //        await _hubProxy.Invoke("LeaveGroup", groupId);
        //    }
        //    catch (InvalidOperationException ex)
        //    {
        //        //_insightsService.ReportException(ex);
        //    }
        //}

        //public void Disconnect()
        //{
        //    if (_hubConnection.State != ConnectionState.Disconnected)
        //        _hubConnection.Stop();
        //}
    }
}