using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Runtime.Serialization.Json;

namespace FeatureOps
{
    public class Client
    {
        private const string FEATURE_OPS_API_URL = "https://app.featureops.com/api/";
        private readonly string _authKey;
        private readonly Options _options;
        private readonly IEnumerable<FeatureFlag> _cache;
       
        public Client(string authKey, Options options)
        {
            _authKey = authKey;
            _options = options;
        }

        public async Task<Response<bool>> Init()
        {
            var response = new Response<bool>();

            using (HttpClient client = new HttpClient())
            {
                var serializer = new DataContractJsonSerializer(typeof(Response<List<FeatureFlag>>));
                client.DefaultRequestHeaders.Add("x-featureops-auth-token", _authKey);

                try
                {
                    var responseStream = await client.GetStreamAsync(FEATURE_OPS_API_URL + "flags");
                    var flags = serializer.ReadObject(responseStream) as Response<List<FeatureFlag>>;
                    if(!flags.Success)
                    {
                        var errorMessage = "Feature Ops failed to initialize with the API: " + flags.Message;
                        response.Message = errorMessage;
                    }
                    else
                    {
                        response.Success = true;
                        response.Value = true;
                    }
                }
                catch(Exception ex)
                {
                    var errorMessage = "Feature Ops failed to initialize with the API: " + ex.ToString();
                    response.Message = errorMessage;
                }
                return response;
            }
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
