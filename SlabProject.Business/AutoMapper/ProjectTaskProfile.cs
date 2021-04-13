using AutoMapper;
using SlabProject.Entity.Models;
using SlabProject.Entity.Requests;

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