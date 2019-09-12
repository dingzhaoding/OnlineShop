using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopFilip.Helpers
{
    public class DataPayU
    {
        public class RootObject2
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public string refresh_token { get; set; }
            public int expires_in { get; set; }
            public string scope { get; set; }
            public string jti { get; set; }
        }
        public class Status
        {
            public string statusCode { get; set; }
        }

        public class RootObject
        {
            public string orderId { get; set; }
            public Status status { get; set; }
            public string redirectUri { get; set; }
        }
    }
}
