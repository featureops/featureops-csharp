using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;

namespace FeatureOps
{
    public class ApiRequest
    {
        private const string FEATURE_OPS_API_URL = "https://app.featureops.com/api/";

        public async Task<Response<List<FeatureFlag>>> GetFlags(string authKey)
        {
            var response = new Response<List<FeatureFlag>>();

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var serializer = new DataContractJsonSerializer(typeof(Response<List<FeatureFlag>>));

                    client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue { NoCache = true };
                    client.DefaultRequestHeaders.Add("x-featureops-auth-token", authKey);

                    var responseStream = await client.GetStreamAsync(FEATURE_OPS_API_URL + "flags");
                    var flags = serializer.ReadObject(responseStream) as Response<List<FeatureFlag>>;

                    if (!flags.Success)
                    {
                        var errorMessage = flags.Message;
                        response.Message = errorMessage;
                    }
                    else
                    {
                        response.Success = true;
                        response.Value = flags.Value;
                    }
                }
                catch (Exception ex)
                {
                    var errorMessage = ex.ToString();
                    response.Message = errorMessage;
                }
                return response;
            }
        }

        public async Task<Response<bool>> UpdateFlag(FlagRequest flagRequest)
        {
            var response = new Response<bool>();

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string jsonString = JsonSerializer(flagRequest);
                    var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    var httpResponse = await client.PostAsync(FEATURE_OPS_API_URL + "flags", httpContent);

                    if(httpResponse.IsSuccessStatusCode)
                    {
                        var flagResponse = JsonDeserialize<Response<bool>>(await httpResponse.Content.ReadAsStringAsync());

                        if(flagResponse.Success)
                        {
                            response.Success = true;
                            response.Value = true;
                        }
                        else
                        {
                            response.Message = flagResponse.Message;
                        }
                    }
                    else
                    {
                        response.Message = httpResponse.StatusCode.ToString();
                    }
                }
                catch (Exception ex)
                {
                    var errorMessage = ex.ToString();
                    response.Message = errorMessage;
                }
                return response;
            }
        }

        public static string JsonSerializer<T>(T t)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, t);
            string jsonString = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            return jsonString;
        }

        public static T JsonDeserialize<T>(string jsonString)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            T obj = (T)ser.ReadObject(ms);
            return obj;
        }
    }
}
