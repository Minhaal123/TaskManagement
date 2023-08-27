using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendNet5WebAPI.ResponseModels
{
    public class TaskViewModel
    {
        public List<TaskLst> tasks { get; set; }
    }


    public class CreateTaskResponse
    {
        public TaskLst task { get; set; }
    }

    public class TaskLst
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}
