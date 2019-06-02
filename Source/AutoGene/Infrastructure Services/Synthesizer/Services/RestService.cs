using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ModernHttpClient;
using Newtonsoft.Json;

namespace Infrastructure.API.Synthesizer.Services
{
    public class RestService
    {
        public const string InternetConnectionErrorMessage = "There is no good connection to the internet";
        public const string ApiConnectionErrorMessage = "There is no good connection to the {0}";

        public HttpClient Client = null;
        public string BaseUrl = string.Empty;

        private bool validate;

        public RestService(string url, bool validate = true)
        {
            BaseUrl = url;
            Client = new HttpClient(new NativeMessageHandler()); // enable the fast ModernHttpClient
            this.validate = validate;

            if (validate)
            {
                Client.BaseAddress = new Uri(url);
                Client.DefaultRequestHeaders.Accept.Clear();
                Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
        }

        public RestService(bool validate = true)
        {
            //BaseUrl = Url;
            Client = new HttpClient(new NativeMessageHandler()); // enable the fast ModernHttpClient
            this.validate = validate;

            if (validate)
            {
                //Client.BaseAddress = new Uri (Url);
                Client.DefaultRequestHeaders.Accept.Clear();
                Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
        }

        public async Task<TItem> GetAsync<TItem>(string requestUri) where TItem : class
        {
//            bool isValidConnection = await ValidateConnection();
//            if (!isValidConnection)
//            {
//                return null;
//            }
//
//            if (Validate)
//            {
//                await ValidateToken();
//
//                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApplicationContext.CurrentUser.AccesToken);
//            }

            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(BaseUrl + requestUri)
            };

            HttpResponseMessage response =
                Client.SendAsync(request).Result;

            if (!response.IsSuccessStatusCode)
            {
                // Update access token if Unauthorized response
                //await UpdateUserToken();
                //throw new HttpRequestException(response.ToString());
                return null;
            }

            var responseText = await response.Content.ReadAsStringAsync();

            try
            {
                var result = JsonConvert.DeserializeObject<TItem>(responseText);
                return result;
            }
            catch
            {
                return responseText as TItem;
            }
        }

        public async Task<TResult> PostAsync<TResult>(string requestUri, object value) where TResult : class
        {
//            bool isValidConnection = await ValidateConnection();
//            if (!isValidConnection)
//            {
//                return null;
//            }
//
//            await ValidateToken();

            //Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApplicationContext.CurrentUser.AccesToken);

            HttpResponseMessage response =
                await Client.PostAsJsonAsync(BaseUrl + requestUri, value);

            if (!response.IsSuccessStatusCode)
            {
                // Update access token if Unauthorized response
                //await UpdateUserToken();
                //throw new HttpRequestException(response.ToString());
                return null;
            }

            var resultString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TResult>(resultString);
            return result;
        }

//        protected async Task<bool> ValidateConnection()
//        {
//            if (Validate)
//            {
//                if (!CrossConnectivity.Current.IsConnected)
//                {
//                    ApplicationContext.ShowConnectionError(InternetConnectionErrorMessage);
//                    return false;
//                }
//
//
//                bool isIbizaServicesReachable = await CrossConnectivity.Current.IsRemoteReachable(BaseUrl);
//                if (!isIbizaServicesReachable)
//                {
//                    ApplicationContext.ShowConnectionError(string.Format(BeFreeServicesConnectionErrorMessage, BaseUrl));
//                    return false;
//                }
//            }
//
//            return true;
//        }

//        private async Task ValidateToken()
//        {
//            DateTime expires = DateTime.Parse(ApplicationContext.CurrentUser.Expires);
//            if (expires < DateTime.Now)
//            {
//                await UpdateUserToken();
//            }
//        }

//        private async Task UpdateUserToken()
//        {
//            string content = string.Format("username={0}&password={1}&grant_type=password", ApplicationContext.CurrentUser.Email, ApplicationContext.CurrentUser.Password);
//            HttpResponseMessage response = await Client.PostAsync(BaseUrl + "/token", new StringContent(content));
//
//            if (response.StatusCode == HttpStatusCode.OK)
//            {
//                SecurityToken securityToken = await response.Content.ReadAsAsync<SecurityToken>();
//
//                if (securityToken != null)
//                {
//                    IRepository<User> userRepostitory = new Repository<User>();
//                    ApplicationContext.CurrentUser = userRepostitory.GetAll().FirstOrDefault();
//
//                    ApplicationContext.CurrentUser.AccesToken = securityToken.AccesToken;
//                    ApplicationContext.CurrentUser.TokenType = securityToken.TokenType;
//                    ApplicationContext.CurrentUser.Issued = securityToken.Issued;
//                    ApplicationContext.CurrentUser.ExpiresIn = securityToken.ExpiresIn;
//                    ApplicationContext.CurrentUser.Expires = securityToken.Expires;
//
//                    userRepostitory.InsertOrUpdate(ApplicationContext.CurrentUser);
//                }
//            }
//        }
    }
}