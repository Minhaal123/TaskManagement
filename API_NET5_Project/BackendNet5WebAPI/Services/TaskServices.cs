using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendNet5WebAPI.Authentication;
using BackendNet5WebAPI.ResponseModels;

namespace BackendNet5WebAPI.Services
{
    public class TaskServices
    {

      public static List<TaskLst> GetTaskList(List<TblTask> tblTasks)
      {
            TaskViewModel lsttasks = new TaskViewModel();
            List<TaskLst> taskViewModels = new List<TaskLst>();
            foreach (var item in tblTasks)
	        {
                TaskLst task = new TaskLst();
                task.id = item.Id;
                task.name = item.Name;
                taskViewModels.Add(task);
	        }

            lsttasks.tasks = taskViewModels;

            return lsttasks.tasks;
      }

      public static TblTask CreateTaskList(CreateTask createTask)
      {
            TblTask tblTask = new TblTask();
            tblTask.Name = createTask.name;
            tblTask.CreatedDate = DateTime.UtcNow;
            tblTask.IsDeleted = false;
            return tblTask;
      }  



    }
}
