using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiRequestManager.Models;

namespace ApiRequestManager.Interfaces
{
    public interface IRequestService
    {
        Task<Api> GetApiByNameAsync(string name);
        Task<IList<RequestParameter>> GetRequestParametersAsync(int apiId, int userId);
    }
}
