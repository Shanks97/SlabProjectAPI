using SlabProjectAPI.Domain.Requests;
using SlabProjectAPI.Domain.Responses;
using SlabProjectAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlabProjectAPI.Services.Interfaces
{
    public interface IProjectService
    {
        Task<BaseRequestResponse<bool>> CreateProject(CreateProjectRequest createProjectRequest);
        Task<BaseRequestResponse<bool>> EditProject(EditProjectRequest createProjectRequest);
        Task<BaseRequestResponse<bool>> DeleteProject(int id);
        Task<BaseRequestResponse<bool>> CompleteProject(int id);
        Task<BaseRequestResponse<Project>> GetProject(int id);
        Task<BaseRequestResponse<List<Project>>> GetProjects();

        Task<BaseRequestResponse<bool>> CreateTask(CreateTaskRequest createTaskRequest);
        Task<BaseRequestResponse<bool>> EditTask(EditTaskRequest createTaskRequest);
        Task<BaseRequestResponse<bool>> DeleteTask(int id);
        Task<BaseRequestResponse<bool>> CompleteTask(int id);
        Task<BaseRequestResponse<ProjectTask>> GetTask(int id);
        Task<BaseRequestResponse<List<ProjectTask>>> GetTasks();


    }
}
