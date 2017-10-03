using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace FeatureOps
{
    [DataContract]
    public class FeatureFlag
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "codeToken")]
        public string CodeToken { get; set; }

        [DataMember(Name = "isOn")]
        public bool IsOn { get; set; }

        [DataMember(Name = "isCanary")]
        public bool IsCanary { get; set; }

        [DataMember(Name = "targets")]
        public IEnumerable<string> Targets { get; set; }
    }
}
