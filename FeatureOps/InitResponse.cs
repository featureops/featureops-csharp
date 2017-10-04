using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace FeatureOps
{
    [DataContract]
    public class InitResponse
    {
        [DataMember(Name = "success")]
        public bool Success { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }
    }
}
