using AutoMapper;
using SlabProject.Entity.Models;
using SlabProject.Entity.Requests;

namespace SlabProjectAPI.Mapper
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            CreateMap<Project, ProjectInfo>();
            CreateMap<ProjectInfo, Project>();
            CreateMap<CreateProjectRequest, Project>();
        }
    }
}