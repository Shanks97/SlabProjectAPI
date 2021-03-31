using AutoMapper;
using SlabProjectAPI.Domain.Requests;
using SlabProjectAPI.Models;

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