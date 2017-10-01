using System;
using System.Collections.Generic;
using System.Text;

namespace FeatureOps
{
    public class Response<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Value { get; set; }
    }
}
