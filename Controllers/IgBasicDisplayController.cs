using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiRequestManager.Interfaces;
using ApiRequestManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace apirequestmanager.Controllers
{
    [Route("api/[controller]")]
    public class IgBasicDisplayController : ControllerBase
    {
        private readonly ILogger<IgBasicDisplayController> _logger;
        private readonly IRequestService _requestService;
        private readonly IBasicIgApiService _basicIgApiService;

        public IgBasicDisplayController(ILogger<IgBasicDisplayController> logger, IRequestService requestService
            , IBasicIgApiService basicIgApiService)
        {
            _logger = logger;
            _basicIgApiService = basicIgApiService;
            _requestService = requestService;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {

            return new string[] { "value1", "value2" };
        }

        [Route("api/[controller]/QueryUser")]
        // GET: api/values
        [HttpGet]
        public async Task<string> GetApi(string name)
        {
            var api =  await _requestService.GetApiByNameAsync(name);

            Newtonsoft.Json.JsonSerializer json = new Newtonsoft.Json.JsonSerializer();
            var jsonResult = JsonConvert.SerializeObject(api, Formatting.Indented);

            return jsonResult;
        }


        [Route("api/[controller]/GetAccessToken")]
        // GET: api/values
        [HttpGet]
        public async Task<string> GetAccessToken(string name)
        {
            var api = await _requestService.GetApiByNameAsync(name);

            Newtonsoft.Json.JsonSerializer json = new Newtonsoft.Json.JsonSerializer();
            var jsonResult = JsonConvert.SerializeObject(api, Formatting.Indented);

            return jsonResult;
        }


        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }


        [Route("api/[controller]/QueryMediaEdge")]
        // GET: api/values
        [HttpGet]
        public async Task<string> QueryMediaEdge(int apiId, int userId)
        {
            var api = await _basicIgApiService.QueryUserMediaEdgeAsync(apiId, userId);

            Newtonsoft.Json.JsonSerializer json = new Newtonsoft.Json.JsonSerializer();
            var jsonResult = JsonConvert.SerializeObject(api, Formatting.Indented);

            return jsonResult;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
