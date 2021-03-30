using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlabProjectAPI.Domain.Responses
{
    public class BaseRequestResponse<T> where T : new()
    {
        public BaseRequestResponse()
        {

        }

        public T Data { get; set; }
        public bool Success { get; set; }
        public List<string> Errors { get; set; }

    }
}
