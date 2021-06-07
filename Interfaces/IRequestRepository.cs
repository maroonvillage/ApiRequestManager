using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiRequestManager.Models;

namespace ApiRequestManager.Interfaces
{
    public interface IRequestRepository
    {
        Task<Api> GetApiByNameAsync(string name);
        Task<Api> GetApiByIdAsync(int apiId);
        Task<IList<RequestParameter>> GetRequestParametersAsync(int apiId, int userId);
    }
}
