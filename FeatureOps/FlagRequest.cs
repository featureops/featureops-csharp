using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace FeatureOps
{
    [DataContract]
    public class FlagRequest
    {
        [DataMember(Name = "authKey")]
        public string AuthKey { get; set; }

        [DataMember(Name = "codeToken")]
        public string CodeToken { get; set; }
    }
}
