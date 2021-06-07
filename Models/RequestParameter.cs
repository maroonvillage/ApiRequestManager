using System;
using Newtonsoft.Json;

namespace ApiRequestManager.Models
{
    [Serializable]
    [JsonObject]
    public class RequestParameter
    {
        public RequestParameter()
        {
        }

        public int ApiId { get; set; }
        public int ParameterId { get; set; }
        public string Name { get; set; }
        public string ParameterValue { get; set; }
        public bool IsOptional { get; set; }
        public ParameterType Type { get; set; }
        public string UserParameterValue { get; set; }
    }


    public class ParameterType
    {
        public ParameterType()
        {

        }

        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public string TypeDescription { get; set; }


    }
}
