using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Threading.Tasks;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading;

namespace FeatureOps
{
    public class Client
    {
        private readonly string _authKey;
        private readonly Options _options;
        private List<FeatureFlag> _cache;
        private readonly ApiRequest _apiRequest;
       
        public Client(string authKey, Options options)
        {
            _apiRequest = new ApiRequest();
            _authKey = authKey;
            _options = options;

            if(_options.CancellationToken == null)
            {
                _options.CancellationToken = new CancellationToken();
            }
            if(_options.PollingInterval <= 0)
            {
                _options.PollingInterval = 5;
            }
        }

        public async Task<InitResponse> InitAsync()
        {
            var response = new InitResponse();
            var flagResponse = await _apiRequest.GetFlags(_authKey);

            if(flagResponse.Success)
            {
                _cache = flagResponse.Value ?? new List<FeatureFlag>();
                response.Success = true;

                Task.Run(async () =>
                {
                    while (true)
                    {
                        await Task.Delay(_options.PollingInterval * 60 * 1000, _options.CancellationToken);
                        if (_options.CancellationToken.IsCancellationRequested)
                        {
                            break;
                        }
                        else
                        {
                            RefreshCacheAsync();
                        }
                    }
                });
            }
            else
            {
                var errorMessage = "Feature Ops failed to initialize with the API: " + flagResponse.Message;
                response.Message = errorMessage;
            }

            return response;
        }

        public async Task<bool> EvalFlagAsync(string codeToken)
        {
            return await EvalFlagAsync(codeToken, new List<string>());
        }

        public async Task<bool> EvalFlagAsync(string codeToken, IEnumerable<string> targets)
        {
            var isOn = false;
            var targetList = targets == null ? new List<string>() : targets.ToList();

            for (var i = 0; i < _cache.Count; i++)
            {
                if (_cache[i].CodeToken.ToLower() == codeToken.ToLower())
                {
                    if (_cache[i].IsOn && (_cache[i].Targets == null || _cache[i].Targets.Count == 0))
                    {
                        isOn = true;
                    }
                    else if (_cache[i].IsOn && (_cache[i].Targets != null || _cache[i].Targets.Count >= 0))
                    {
                        for (var targetIdx = 0; targetIdx < targetList.Count; targetIdx++)
                        {
                            for (var cacheTargetIdx = 0; cacheTargetIdx < _cache[i].Targets.Count; cacheTargetIdx++)
                            {
                                if (targetList[targetIdx].ToLower() == _cache[i].Targets[cacheTargetIdx].ToLower())
                                {
                                    isOn = true;
                                }
                            }
                        }
                    }
                    var flagUpdateRequest = await _apiRequest.UpdateFlag(new FlagRequest { AuthKey = _authKey, CodeToken = codeToken });
                    break;
                }
            }

            return isOn;
        }

        private async void RefreshCacheAsync()
        {
            var flagResponse = await _apiRequest.GetFlags(_authKey);
            if (flagResponse.Success && flagResponse.Value != null)
            {
                for (var i = 0; i < flagResponse.Value.Count; i++)
                {
                    var isInCache = false;
                    for (var i2 = 0; i2 < _cache.Count; i2++)
                    {
                        if (flagResponse.Value[i].CodeToken == _cache[i2].CodeToken)
                        {
                            isInCache = true;

                            if (_cache[i2].IsCanary && flagResponse.Value[i].IsCanary)
                            {
                                // do nothing, persist original canary request for duration of user session
                            }
                            else
                            {
                                _cache[i2] = flagResponse.Value[i];
                            }
                            break;
                        }
                    }

                    if (!isInCache)
                    {
                        _cache.Add(flagResponse.Value[i]);
                    }
                }
            }
        }
    }
}
