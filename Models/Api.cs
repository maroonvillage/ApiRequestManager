using System;
namespace ApiRequestManager.Models
{
    public class Api
    {
        public Api()
        {
        }

        public int ApiId { get; set; }
        public string ApiName { get; set; }
        public string ApiDescription { get; set; }
        public string BaseUri { get; set; }

    }
}
