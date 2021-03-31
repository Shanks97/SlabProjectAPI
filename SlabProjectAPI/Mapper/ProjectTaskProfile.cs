using AutoMapper;
using SlabProjectAPI.Domain.Requests;
using SlabProjectAPI.Models;

namespace SlabProjectAPI.Mapper
{
    public class ProjectTaskProfile : Profile
    {
        public ProjectTaskProfile()
        {
            CreateMap<ProjectTask, ProjectTaskInfo>();
            CreateMap<ProjectTaskInfo, ProjectTask>();
            CreateMap<CreateTaskRequest, ProjectTask>();
        }
    }
}