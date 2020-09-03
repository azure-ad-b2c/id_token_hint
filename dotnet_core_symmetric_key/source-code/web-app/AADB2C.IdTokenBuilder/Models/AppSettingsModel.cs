using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AADB2C.IdTokenBuilder.Models
{
    public class AppSettingsModel
    {
        public string ClientSigningKey { get; set; }
        public string B2CTenant { get; set; }
        public string B2CPolicy { get; set; }
        public string B2CClientId { get; set; }
        public string B2CRedirectUri { get; set; }
        public string B2CAuthorizationUrl { get; set; }
    }
}
