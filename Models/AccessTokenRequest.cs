using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace ApiRequestManager.Models
{
    [Serializable]
    [JsonObject]
    public class AccessTokenRequest
    {

        [JsonProperty("client_id")]
        public string ClientId { get; set; }
        [JsonProperty("client_secret")]
        public string ClientSecret { get; set; }
        [JsonProperty("grant_tupe")]
        public string GrantType { get; set; }
        [JsonProperty("redirect_uri")]
        public string RedirectUri { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }


    }
}