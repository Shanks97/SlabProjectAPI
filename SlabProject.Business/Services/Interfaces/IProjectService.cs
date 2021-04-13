using SlabProject.Entity.Models;
using SlabProject.Entity.Requests;
using SlabProject.Entity.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SlabProjectAPI.Services.Interfaces
{
    public interface IProjectService
    {
        BaseRequestResponse<Project> CreateProject(CreateProjectRequest createProjectRequest);

        BaseRequestResponse<bool> EditProject(int id, EditProjectRequest createProjectRequest);

        BaseRequestResponse<bool> DeleteProject(int id);

        Task<BaseRequestResponse<bool>> CompleteProject(int id);

        BaseRequestResponse<Project> GetProject(int id);

        BaseRequestResponse<List<Project>> GetProjects();

        BaseRequestResponse<ProjectTask> CreateTask(CreateTaskRequest editTaskRequest);

        BaseRequestResponse<bool> EditTask(int id, EditTaskRequest editTaskRequest);

        BaseRequestResponse<bool> DeleteTask(int id);

        BaseRequestResponse<bool> CompleteTask(int id);

        BaseRequestResponse<ProjectTask> GetTask(int id);

        BaseRequestResponse<List<ProjectTask>> GetTasks();
    }
}