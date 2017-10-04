using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using FeatureOps;

namespace Example
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                var client = new Client("{ENVIRONMENT AUTH KEY}", new Options { PollingInterval = 5 });
                var response = await client.InitAsync();

                if (response.Success)
                {
                    while (true)
                    {
                        if (await client.EvalFlagAsync("{CODE TOKEN}"))
                        {
                            // Feature Is On
                        }
                        else
                        {
                            // Feature Is Off
                        }
                    }
                }
            }).GetAwaiter().GetResult();
        }
    }
}
