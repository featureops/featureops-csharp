using System;
using System.Collections.Generic;
using System.Text;

namespace FeatureOps
{
    public class FeatureFlag
    {
        public string Name { get; set; }
        public string CodeToken { get; set; }
        public bool IsOn { get; set; }
        public bool IsCanary { get; set; }
        public IEnumerable<string> Targets { get; set; }
    }
}
