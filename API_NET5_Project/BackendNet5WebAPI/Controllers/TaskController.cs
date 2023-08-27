using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendNet5WebAPI.Authentication;
using BackendNet5WebAPI.ResponseModels;
using BackendNet5WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BackendNet5WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext _context;

        public TaskController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            this.userManager = userManager;
            _context = context;
        }

        [Route("user")]
        [HttpGet]
        public IActionResult GetUsers()
        {
            try
            {
                var users = userManager.Users;
                return Ok(users);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        [Route("list-tasks")]
        [HttpGet]
        public IActionResult GetTasks()
        {
            try
            {
                var getTasks = _context.TblTasks.Where(x => x.IsDeleted == false).ToList();
                var response = TaskServices.GetTaskList(getTasks);
                return Ok(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [Route("create-task")]
        [HttpPost]

        public async Task<IActionResult> CreateTasks([FromBody] CreateTask createTask)
        {
            try
            {
                var createtask = TaskServices.CreateTaskList(createTask);
                await _context.AddAsync(createtask);
                await _context.SaveChangesAsync();
                TaskLst taskLst = new TaskLst();
                taskLst.id = createtask.Id;
                taskLst.name = createtask.Name;
                return Ok(new CreateTaskResponse { task = taskLst });
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        //[Route("BulkDelete")]    
        //[HttpPost]
        //public IActionResult BulkDelete(int[] TaskIds)
        //{
        //    foreach (int taskId in TaskIds)
        //    {
        //        TblTask task = _context.TblTasks.Where(x => x.Id == taskId).FirstOrDefault();
        //        task.IsDeleted = true;
        //    }
        //    _context.SaveChanges();
        //    return Ok();
        //}

        [Route("delete-task/{id}")]
        [HttpDelete]
        public IActionResult DeleteTask(int id)
        {
            TblTask task = _context.TblTasks.Where(x => x.Id == id).FirstOrDefault();
            if (task != null)
            {
                task.IsDeleted = true;
                _context.SaveChanges();
                return Ok(new
                {
                    statusCode = 200,
                    statusMessage = $"{task.Name} has been deleted successfully."
                });
            }
            else
            {
                return Ok(new
                {
                    statusCode = 200,
                    statusMessage = $"Record not found!"
                });
            }
        }
    }
}
