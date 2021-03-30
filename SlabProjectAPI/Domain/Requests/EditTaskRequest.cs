using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlabProjectAPI.Domain.Requests
{
    public class EditTaskRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? ExecutionTime { get; set; }
    }
}
