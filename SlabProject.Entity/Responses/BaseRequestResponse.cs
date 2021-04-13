using System.Collections.Generic;

namespace SlabProject.Entity.Responses
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