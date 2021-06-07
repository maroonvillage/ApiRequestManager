using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using ApiRequestManager.Enums;
using ApiRequestManager.Interfaces;
using ApiRequestManager.Models;
using apirequestmanager.Enums;

namespace ApiRequestManager.Services
{
    public class BasicIgApiService : IBasicIgApiService
    {
        private string _baseUriIgBasicApiAuthorize;
        private string _baseUriIgBasicApiQuery;
        private string _appId;
        private string _secretId;
        private readonly IConfiguration _configuration;
        public HttpClient Client { get; }

        private readonly IRequestRepository _requestRepository;



        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _client;


        public BasicIgApiService(HttpClient httpClient, IConfiguration configuration, IRequestRepository requestRepository)
        {
            _configuration = configuration;
            _appId = _configuration.GetValue<string>("AppId__InstagramBasicApi");
            _secretId = _configuration.GetValue<string>("SecretId__InstagramBasicApi");
            _baseUriIgBasicApiAuthorize = _configuration.GetValue<string>("BaseUrlAuthorize_InstagramBasicApi");
            _baseUriIgBasicApiQuery = _configuration.GetValue<string>("BaseUrlQuery_InstagramBasicApi");
            //_baseUriIgBasicApi = "https://api.instagram.com/oauth/authorize/";
            //_httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));

            Client = httpClient;
            Client.BaseAddress = new Uri(_baseUriIgBasicApiAuthorize);

            _requestRepository = requestRepository;
        }


        public async Task RequestAuthorization(string redirectUri)
        {

            var uri = BuildAuthorizeUri(redirectUri);

            var response = await Client.GetAsync(uri);

            response.EnsureSuccessStatusCode();

            var respStr = response.Content.ReadAsStringAsync();

        }

        public async Task<string> RequestAccessToken(string temporaryCode, string returnUri)
        {

           
            var uri = BuildAccessTokenRequestUri(returnUri);

            _baseUriIgBasicApiAuthorize = $"{_baseUriIgBasicApiAuthorize}{uri}";
            var grantTypeValue = HelperService.GetEnumDescription(GrantTypeValues.AuthorizationCode);

            var requestModel = new AccessTokenRequest
            {

                ClientId = _appId,
                ClientSecret = _secretId,
                GrantType = grantTypeValue,
                RedirectUri = returnUri,
                Code = temporaryCode

            };
            var httpContent = JsonConvert.SerializeObject(requestModel);
            var buffer = Encoding.UTF8.GetBytes(httpContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await Client.PostAsync(_baseUriIgBasicApiAuthorize, byteContent).ConfigureAwait(false);

            var resultStr = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();

            return await Task.FromResult<string>(resultStr);
        }

        public async Task<QueryMediaResponse> QueryIgUserMedia(string accessToken)
        {
            Client.BaseAddress = new Uri(_baseUriIgBasicApiQuery);

            var fields = $"{HelperService.GetEnumDescription(QueryableFields.Id)}," +
                         $"{HelperService.GetEnumDescription(QueryableFields.MediaType)}," +
                         $"{HelperService.GetEnumDescription(QueryableFields.MediaUrl)}," +
                         $"{HelperService.GetEnumDescription(QueryableFields.UserName)}," +
                         $"{HelperService.GetEnumDescription(QueryableFields.Timestamp)}";

            var uri = $"?{HelperService.GetEnumDescription(InstagramBasicApiParams.Fields)}=" +
                         $"{fields}&" +
                         $"{HelperService.GetEnumDescription(InstagramBasicApiParams.AccessToken)}=" +
                         $"{accessToken}";

            var requestUrl = $"{_baseUriIgBasicApiQuery}{uri}";
            var response = await Client.GetAsync(requestUrl).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            var resultString = await response.Content.ReadAsStringAsync();
            var results = JsonConvert.DeserializeObject<QueryMediaResponse>(resultString);

            return await Task.FromResult<QueryMediaResponse>(results);
        }

        public async Task<string> GetAuthorizationUrl(string returnUrl)
        {
            var baseUrl = _configuration.GetValue<string>("BaseUrlAuthorize_InstagramBasicApi"); ;
            Client.BaseAddress = new Uri(baseUrl);
            var uri = BuildAuthorizeUri(returnUrl);

            var authUri = $"{baseUrl}{uri}";
            return await Task.FromResult<string>(authUri);
        }

        private string BuildAccessTokenRequestUri(string returnUri)
        {
            var scopeVal = $"{HelperService.GetEnumDescription(ScopeValues.UserProfile)},{HelperService.GetEnumDescription(ScopeValues.UserMedia)}";

            var responseTypeVal = HelperService.GetEnumDescription(ResponseTypeValues.Code);
            var grantTypeValue = HelperService.GetEnumDescription(GrantTypeValues.AuthorizationCode);

            var uriSegment = HelperService.GetEnumDescription(RequestUriSegment.AccessToken);


            var uri = $"{uriSegment}?{HelperService.GetEnumDescription(InstagramBasicApiParams.ClientId)}=" +
                        $"{_appId}&" +
                         $"{HelperService.GetEnumDescription(InstagramBasicApiParams.ClientSecretId)}=" +
                         $"{_secretId}&" +
                         $"{HelperService.GetEnumDescription(InstagramBasicApiParams.GrantType)}=" +
                         $"{grantTypeValue}&" +
                         $"{HelperService.GetEnumDescription(InstagramBasicApiParams.RedirectUri)}=" +
                         $"{returnUri}&" +
                         $"{HelperService.GetEnumDescription(InstagramBasicApiParams.ResponseType)}=" +
                         $"{responseTypeVal}";
            return uri;
        }

        private string BuildAuthorizeUri(string redirectUri)
        {

            var uriSegment = HelperService.GetEnumDescription(RequestUriSegment.Authorize);
            var scopeVal = $"{HelperService.GetEnumDescription(ScopeValues.UserProfile)},{HelperService.GetEnumDescription(ScopeValues.UserMedia)}";
            var responseTypeVal = HelperService.GetEnumDescription(ResponseTypeValues.Code);


            var uri = $"{uriSegment}?{HelperService.GetEnumDescription(InstagramBasicApiParams.ClientId)}=" +
             $"{_appId}&" +
             $"{HelperService.GetEnumDescription(InstagramBasicApiParams.RedirectUri)}=" +
             $"{redirectUri}&" +
             $"{HelperService.GetEnumDescription(InstagramBasicApiParams.Scope)}=" +
             $"{scopeVal}&" +
             $"{HelperService.GetEnumDescription(InstagramBasicApiParams.ResponseType)}=" +
             $"{responseTypeVal}";

            return uri;
        }


        public async Task<AccessTokenRequest> RequestLongLivedAccessTokenAsync(int apiId, string grantType)
        {

            //_requestRepository.GetApiByNameAsync("name");
            //var httpResponse = await _client.GetAsync(BaseUrl);

            //if (!httpResponse.IsSuccessStatusCode)
            //{
            //    throw new Exception("Cannot retrieve tasks");
            //}

            //var content = await httpResponse.Content.ReadAsStringAsync();
            //var tasks = JsonConvert.DeserializeObject<List<Todo>>(content);

            //return tasks;

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiId"></param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public async Task<QueryMediaResponse> QueryUserMediaEdgeAsync(int apiId, int userId)
        {
            var requestUri = new StringBuilder();
            //get the api
            var api = await _requestRepository.GetApiByIdAsync(apiId);

            if (string.IsNullOrEmpty(api.BaseUri)) throw new ArgumentException("Base uri cannot be null or empty.");

            var baseUri = $"{api.BaseUri}?";

            Client.BaseAddress = new Uri(baseUri);
            //retreive request parameters for the api
            var parameters = await _requestRepository.GetRequestParametersAsync(apiId, userId);


            //iterate through the parameters to build the API request
            foreach(var param in parameters)
            {
                //Check the parameter type
                var segment = buildUriSegment(param);
                requestUri.Append(segment);

            }

            //now, send request
            var resultString = await sendGetAsync(requestUri.ToString());

            return JsonConvert.DeserializeObject<QueryMediaResponse>(resultString);
        }
        public Task<QueryMediaResponse> QueryIgMediaNodeAsync(int apiId, string accessToken)
        {
            return null;
        }


        private static string buildUriSegment(RequestParameter requestParameter)
        {
            var requestUri = string.Empty;
            var paramTypeStr = requestParameter.Type.TypeName;

            var paramType = HelperService.GetValueFromDescription<RequestParamTypes>(paramTypeStr);

            switch (paramType)
            {
                case RequestParamTypes.NameValuePair:
                    requestUri += requestParameter.Name;
                    requestUri += "=";
                    var value = !string.IsNullOrEmpty(requestParameter.ParameterValue) ? requestParameter.ParameterValue : requestParameter.UserParameterValue;
                    requestUri += value;
                    requestUri += "&";
                    break;
                case RequestParamTypes.FileName:
                    break;
                case RequestParamTypes.Path:
                    break;
                default:
                    break;
            }

            return requestUri;
        }

        private async Task<string> sendGetAsync(string uri)
        {

            if (Client.BaseAddress == null) return null;

            var requestUrl = $"{Client.BaseAddress}{uri}";

            var response = await Client.GetAsync(requestUrl).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            var resultString = await response.Content.ReadAsStringAsync();

            return resultString;
        }


    }

}