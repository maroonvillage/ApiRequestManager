using System.ComponentModel;

namespace ApiRequestManager.Enums
{
    public enum RequestUriSegment
    {

        [Description("access_token")]
        AccessToken,
        [Description("authorize")]
        Authorize,
    }
    public enum InstagramBasicApiParams
    {
        [Description("client_id")]
        ClientId,
        [Description("redirect_uri")]
        RedirectUri,
        [Description("scope")]
        Scope,
        [Description("response_type")]
        ResponseType,
        [Description("client_secret_id")]
        ClientSecretId,
        [Description("grant_type")]
        GrantType,
        [Description("fields")]
        Fields,
        [Description("access_token")]
        AccessToken

    }

    public enum ScopeValues
    {

        [Description("user_profile")]
        UserProfile,
        [Description("user_media")]
        UserMedia
    }

    public enum ResponseTypeValues
    {
        [Description("code")]
        Code
    }

    public enum GrantTypeValues
    {


        [Description("authorization_code")]
        AuthorizationCode
    }

    public enum QueryableFields{
        [Description("id")]
        Id,
        [Description("media_type")]
        MediaType,
        [Description("media_url")]
        MediaUrl,
        [Description("username")]
        UserName,
        [Description("timestamp")]
        Timestamp
    }
}