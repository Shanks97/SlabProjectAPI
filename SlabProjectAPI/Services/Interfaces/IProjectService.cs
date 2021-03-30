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
        Task<BaseRequestResponse<bool>> CreateProject(CreateProjectRequest createprojectrequest);
        Task<BaseRequestResponse<bool>> EditProject(EditProjectRequest createprojectrequest);
        Task<BaseRequestResponse<bool>> DeleteProject(int id);
        Task<BaseRequestResponse<bool>> CompleteProject(int id);
        Task<BaseRequestResponse<Project>> GetProject(int id);
        Task<BaseRequestResponse<List<Project>>> GetProjects();
    }
}
