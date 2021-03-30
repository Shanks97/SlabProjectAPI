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

        public Task<BaseRequestResponse<bool>> CreateProject(CreateProjectRequest createprojectrequest)
        {
            throw new NotImplementedException();
        }

        public Task<BaseRequestResponse<bool>> DeleteProject(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseRequestResponse<bool>> EditProject(EditProjectRequest createprojectrequest)
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
    }
}
