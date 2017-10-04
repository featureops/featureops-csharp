using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace FeatureOps
{
    public class Options
    {
        public int PollingInterval { get; set; }
        public CancellationToken CancellationToken { get; set; }
    }
}
