using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace FeatureOps
{
    [DataContract]
    public class Response<T>
    {
        [DataMember(Name = "success")]
        public bool Success { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }

        [DataMember(Name = "value")]
        public T Value { get; set; }
    }
}
