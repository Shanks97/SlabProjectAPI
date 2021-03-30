using SlabProjectAPI.Domain.Requests;
using SlabProjectAPI.Domain.Responses;
using SlabProjectAPI.Models;
using SlabProjectAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlabProjectAPI.Services
{
    public class ProjectService : IProjectService
    {
        public Task<BaseRequestResponse<bool>> CompleteProject(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseRequestResponse<bool>> CompleteTask(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseRequestResponse<bool>> CreateProject(CreateProjectRequest createProjectRequest)
        {
            throw new NotImplementedException();
        }

        public Task<BaseRequestResponse<bool>> CreateTask(CreateTaskRequest createTaskRequest)
        {
            throw new NotImplementedException();
        }

        public Task<BaseRequestResponse<bool>> DeleteProject(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseRequestResponse<bool>> DeleteTask(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseRequestResponse<bool>> EditProject(EditProjectRequest createProjectRequest)
        {
            throw new NotImplementedException();
        }

        public Task<BaseRequestResponse<bool>> EditTask(EditTaskRequest createTaskRequest)
        {
            throw new NotImplementedException();
        }

        public Task<BaseRequestResponse<Project>> GetProject(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseRequestResponse<List<Project>>> GetProjects()
        {
            throw new NotImplementedException();
        }

        public Task<BaseRequestResponse<ProjectTask>> GetTask(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseRequestResponse<List<ProjectTask>>> GetTasks()
        {
            throw new NotImplementedException();
        }
    }
}
