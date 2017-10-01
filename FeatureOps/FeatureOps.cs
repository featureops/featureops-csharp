using System.Collections.Generic;
using System.Threading.Tasks;

namespace FeatureOps
{
    public class FeatureOps
    {
        private readonly string _authKey;
        private readonly Options _options;
        private readonly IEnumerable<FeatureFlag> _cache;
       
        public FeatureOps(string authKey, Options options)
        {
            _authKey = authKey;
            _options = options;
        }

        public async Task<Response<bool>> Init()
        {
            return null;
        }

        public async Task<Response<bool>> EvalFlag(string codeToken, IEnumerable<string> targets)
        {
            return null;
        }

        private async void RefreshCache()
        {

        }
    }
}
