using AutoMapper;
using SlabProjectAPI.Domain.Requests;
using SlabProjectAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
