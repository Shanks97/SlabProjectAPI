using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SlabProjectAPI.Constants;
using SlabProjectAPI.Data;
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
        private ProjectDbContext _dbContext;
        private IMapper _mapper;

        public ProjectService(
            ProjectDbContext dbContext,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public BaseRequestResponse<bool> CompleteProject(int id)
        {
            var project = _dbContext.Projects.FirstOrDefault(x => x.Id == id);
            if (project != null)
            {
                if (project.Status == StatusConstants.InProcess)
                {
                    //TODO: check with tasks
                    return new BaseRequestResponse<bool>()
                    {
                        Success = true,
                    };
                }
            }
            return new BaseRequestResponse<bool>()
            {
                Success = false,
            };
        }

        public BaseRequestResponse<bool> CompleteTask(int id)
        {
            throw new NotImplementedException();
        }

        public BaseRequestResponse<bool> CreateProject(CreateProjectRequest createProjectRequest)
        {
            try
            {
                var project = _mapper.Map<Project>(createProjectRequest);

                _dbContext.Projects.Add(project);
                _dbContext.SaveChanges();
                return new BaseRequestResponse<bool>()
                {
                    Success = true,
                    Data = true,
                };
            }
            catch
            {
                return new BaseRequestResponse<bool>()
                {
                    Success = false,
                    Data = false,
                    Errors = new List<string>()
                    {
                        "Error while creating the project"
                    }
                };
            }
        }

        public BaseRequestResponse<bool> CreateTask(CreateTaskRequest createTaskRequest)
        {
            try
            {
                var project = _mapper.Map<ProjectTask>(createTaskRequest);
                _dbContext.Tasks.Add(project);
                _dbContext.SaveChanges();
                return new BaseRequestResponse<bool>()
                {
                    Success = true,
                    Data = true,
                };
            }
            catch
            {
                return new BaseRequestResponse<bool>()
                {
                    Success = false,
                    Data = false,
                    Errors = new List<string>()
                    {
                        "Error while creating the task"
                    }
                };
            }
        }

        public BaseRequestResponse<bool> DeleteProject(int id)
        {
            var project = _dbContext.Projects.FirstOrDefault(x => x.Id == id);
            if(project != null)
            {
                _dbContext.Projects.Remove(project);
                _dbContext.SaveChanges();
                return new BaseRequestResponse<bool>()
                {
                    Data = true,
                    Success = true,
                };
            }
            else
            {
                return new BaseRequestResponse<bool>()
                {
                    Data = false,
                    Success = false,
                    Errors = new List<string>()
                    {
                        "Cannot find project with id: " + id
                    }
                };
            }
        }

        public BaseRequestResponse<bool> DeleteTask(int id)
        {
            var task = _dbContext.Tasks.FirstOrDefault(x => x.Id == id);
            if (task != null)
            {
                _dbContext.Tasks.Remove(task);
                _dbContext.SaveChanges();
                return new BaseRequestResponse<bool>()
                {
                    Data = true,
                    Success = true,
                };
            }
            else
            {
                return new BaseRequestResponse<bool>()
                {
                    Data = false,
                    Success = false,
                    Errors = new List<string>()
                    {
                        "Cannot find task with id: " + id
                    }
                };
            }
        }

        public BaseRequestResponse<bool> EditProject(EditProjectRequest editProjectRequest)
        {
            var project = _dbContext.Projects.FirstOrDefault(x => x.Id == editProjectRequest.Id);
            if(project == null)
            {
                project.Name = string.IsNullOrEmpty(editProjectRequest.Name) ? project.Name : editProjectRequest.Name;
                project.Description = string.IsNullOrEmpty(editProjectRequest.Description) ? project.Description : editProjectRequest.Description;
                project.FinishDate = editProjectRequest.FinishDate.HasValue ? editProjectRequest.FinishDate.Value : project.FinishDate;
                _dbContext.Update(project);
                return new BaseRequestResponse<bool>()
                {
                    Data = true,
                    Success = true
                };
            }
            else
            {
                return new BaseRequestResponse<bool>()
                {
                    Data = false,
                    Success = false,
                    Errors = new List<string>()
                    {
                        "Cannot update Project with id: " + editProjectRequest.Id
                    }
                };
            }
        }

        public BaseRequestResponse<bool> EditTask(EditTaskRequest editTaskRequest)
        {
            var task = _dbContext.Tasks.FirstOrDefault(x => x.Id == editTaskRequest.Id);
            if (task == null)
            {
                task.Name = string.IsNullOrEmpty(editTaskRequest.Name) ? task.Name : editTaskRequest.Name;
                task.Description = string.IsNullOrEmpty(editTaskRequest.Description) ? task.Description : editTaskRequest.Description;
                task.ExecutionDate = editTaskRequest.ExecutionDate.HasValue ? editTaskRequest.ExecutionDate.Value : task.ExecutionDate;
                _dbContext.Update(task);
                return new BaseRequestResponse<bool>()
                {
                    Data = true,
                    Success = true
                };
            }
            else
            {
                return new BaseRequestResponse<bool>()
                {
                    Data = false,
                    Success = false,
                    Errors = new List<string>()
                    {
                        "Cannot update Project with id: " + editTaskRequest.Id
                    }
                };
            }
        }

        public BaseRequestResponse<Project> GetProject(int id)
        {
            var project = _dbContext.Projects.FirstOrDefault(x => x.Id == id);
            if(project != null)
            {
                return new BaseRequestResponse<Project>()
                {
                    Data = project,
                    Success = true
                };
            }

            return new BaseRequestResponse<Project>()
            {
                Data = null,
                Success = false,
                Errors = new List<string>()
                {
                    "Cannot find a project with Id: " + id
                }
            };
        }

        public BaseRequestResponse<List<Project>> GetProjects()
        {
            try
            {
                return new BaseRequestResponse<List<Project>>
                {
                    Success = true,
                    Data = _dbContext.Projects.AsNoTracking().ToList()
                };
            }
            catch (Exception ex)
            {
                return new BaseRequestResponse<List<Project>>
                {
                    Data = null,
                    Success = false,
                    Errors = new List<string>()
                    {
                        "Error while fetching all projects"
                    }
                };
            }
        }

        public BaseRequestResponse<ProjectTask> GetTask(int id)
        {
            var task = _dbContext.Tasks.FirstOrDefault(x => x.Id == id);
            if (task != null)
            {
                return new BaseRequestResponse<ProjectTask>()
                {
                    Data = task,
                    Success = true
                };
            }

            return new BaseRequestResponse<ProjectTask>()
            {
                Data = null,
                Success = false,
                Errors = new List<string>()
                {
                    "Cannot find a task with Id: " + id
                }
            };
        }

        public BaseRequestResponse<List<ProjectTask>> GetTasks()
        {
            try
            {
                return new BaseRequestResponse<List<ProjectTask>>
                {
                    Success = true,
                    Data = _dbContext.Tasks.AsNoTracking().ToList()
                };
            }
            catch
            {
                return new BaseRequestResponse<List<ProjectTask>>
                {
                    Data = null,
                    Success = false,
                    Errors = new List<string>()
                    {
                        "Error while fetching all tasks"
                    }
                };
            }
        }
    }
}