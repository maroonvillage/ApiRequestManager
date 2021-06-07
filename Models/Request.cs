using System;
using System.Collections.Generic;

namespace ApiRequestManager.Models
{
    public class Request
    {
        public Request()
        {


        }

        public Api Api { get; set; }
        public IList<RequestParameter> RequestParameters { get; set; }
    }
}
