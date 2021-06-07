using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiRequestManager.Interfaces;
using ApiRequestManager.Models;
using Microsoft.Extensions.Logging;

namespace ApiRequestManager.Services
{
    public class RequestService : IRequestService
    {
        private readonly ILogger _logger;
        private readonly IRequestRepository _requestRepository;
        public RequestService(ILogger<IRequestRepository> logger, IRequestRepository requestRepository)
        {
            _logger = logger;
            _requestRepository = requestRepository;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<Api> GetApiByNameAsync(string name)
        {
            var api = new Api();
            try
            {
                api = await _requestRepository.GetApiByNameAsync(name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return api;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiId"></param>
        /// <returns></returns>
        public async Task<IList<RequestParameter>> GetRequestParametersAsync(int apiId, int userId)
        {
            var parameters = new List<RequestParameter>();
            try
            {

                parameters = (List<RequestParameter>)await _requestRepository.GetRequestParametersAsync(apiId, userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return parameters;
        }
    }
}
