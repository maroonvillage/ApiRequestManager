using System.Threading.Tasks;
using ApiRequestManager.Models;

namespace ApiRequestManager.Interfaces
{
    public interface IBasicIgApiService
    {
        Task RequestAuthorization(string redirectUri);
        Task<string> RequestAccessToken(string temporaryCode, string returnUrl);

        Task<QueryMediaResponse> QueryIgUserMedia(string accessToken);

        Task<string> GetAuthorizationUrl(string returnUrl);

    }

}