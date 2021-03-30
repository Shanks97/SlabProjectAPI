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
        BaseRequestResponse<bool> CreateProject(CreateProjectRequest createProjectRequest);
        BaseRequestResponse<bool> EditProject(EditProjectRequest createProjectRequest);
        BaseRequestResponse<bool> DeleteProject(int id);
        BaseRequestResponse<bool> CompleteProject(int id);
        BaseRequestResponse<Project> GetProject(int id);
        BaseRequestResponse<List<Project>> GetProjects();

        BaseRequestResponse<bool> CreateTask(CreateTaskRequest editTaskRequest);
        BaseRequestResponse<bool> EditTask(EditTaskRequest editTaskRequest);
        BaseRequestResponse<bool> DeleteTask(int id);
        BaseRequestResponse<bool> CompleteTask(int id);
        BaseRequestResponse<ProjectTask> GetTask(int id);
        BaseRequestResponse<List<ProjectTask>> GetTasks();


    }
}
