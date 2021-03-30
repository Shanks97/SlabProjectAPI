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
            var project = _dbContext.Projects.Include(x => x.Tasks).FirstOrDefault(x => x.Id == id);
            if (project != null)
            {
                if(project.Tasks.Any(x => x.ExecutionDate > DateTime.Now))
                {
                    return new BaseRequestResponse<bool>()
                    {
                        Errors = new List<string>()
                        {
                            "Some tasks didn't executed yet"
                        }
                    };
                }

                if (project.Status == StatusConstants.InProcess)
                {
                    project.Status = StatusConstants.Done;
                    _dbContext.Update(project);
                    _dbContext.SaveChanges();
                    return new BaseRequestResponse<bool>()
                    {
                        Success = true,
                        Data = true,
                    };
                 
                }
                else
                {
                    return new BaseRequestResponse<bool>()
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Project is already completed"
                        }
                    };
                }
            }
            return new BaseRequestResponse<bool>()
            {
                Success = false,
                Errors = new List<string>()
                {
                    "Cannot complete project with id: " + id
                }
            };
        }

        public BaseRequestResponse<Project> CreateProject(CreateProjectRequest createProjectRequest)
        {
            try
            {
                var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                if (createProjectRequest.StartDate < date)
                    return new BaseRequestResponse<Project>()
                    {
                        Data = null,
                        Success = false,
                        Errors = new List<string>()
                        {
                            "StartDate is less than the actual DateTime"
                        }
                    };
                if (createProjectRequest.FinishDate <= createProjectRequest.StartDate)
                    return new BaseRequestResponse<Project>()
                    {
                        Data = null,
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Finish Date must be greater than Start Date"
                        }
                    };

                var project = _mapper.Map<Project>(createProjectRequest);
                project.Status = StatusConstants.InProcess;
                var entity = _dbContext.Projects.Add(project).Entity;
                _dbContext.SaveChanges();
                return new BaseRequestResponse<Project>()
                {
                    Success = true,
                    Data = entity,
                };
            }
            catch
            {
                return new BaseRequestResponse<Project>()
                {
                    Success = false,
                    Data = null,
                    Errors = new List<string>()
                    {
                        "Error while creating the project"
                    }
                };
            }
        }

        public BaseRequestResponse<ProjectTask> CreateTask(CreateTaskRequest createTaskRequest)
        {
            try
            {
                var project = _dbContext.Projects.FirstOrDefault(x => x.Id == createTaskRequest.ProjectId);
                if(project == null)
                {
                    return new BaseRequestResponse<ProjectTask>()
                    {
                        Errors = new List<string>()
                        {
                            $"No projects with id: ${createTaskRequest.ProjectId} "
                        }
                    };
                }
                if (createTaskRequest.ExecutionDate < project.StartDate ||
                    createTaskRequest.ExecutionDate > project.FinishDate)
                        return new BaseRequestResponse<ProjectTask>()
                        {
                            Errors = new List<string>()
                            {
                                "The Execution date is out of the range between Project's Start and Finish Date "
                            }
                        };


                var task = _mapper.Map<ProjectTask>(createTaskRequest);
                var entity = _dbContext.Tasks.Add(task).Entity;
                _dbContext.SaveChanges();
                return new BaseRequestResponse<ProjectTask>()
                {
                    Success = true,
                    Data = entity,
                };
            }
            catch
            {
                return new BaseRequestResponse<ProjectTask>()
                {
                    Success = false,
                    Data = null,
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
            var project = _dbContext.Projects.Include(x => x.Tasks).FirstOrDefault(x => x.Id == editProjectRequest.Id);

            if(project != null)
            {
                if(project.Tasks.Any(x => x.ExecutionDate > editProjectRequest.FinishDate))
                {
                    return new BaseRequestResponse<bool>()
                    {
                        Errors = new List<string>()
                        {
                            "Some Tasks have an execution time greater than the new Finish Date"
                        }
                    };
                }

                project.Name = string.IsNullOrEmpty(editProjectRequest.Name) ? project.Name : editProjectRequest.Name;
                project.Description = string.IsNullOrEmpty(editProjectRequest.Description) ? project.Description : editProjectRequest.Description;
                project.FinishDate = editProjectRequest.FinishDate.HasValue ? editProjectRequest.FinishDate.Value : project.FinishDate;
                _dbContext.Update(project);
                _dbContext.SaveChanges();
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
            var task = _dbContext.Tasks.Include(x => x.Project).FirstOrDefault(x => x.Id == editTaskRequest.Id);
            if (task != null)
            {
                var project = task.Project;
                if (editTaskRequest.ExecutionDate.HasValue)
                {
                    if (editTaskRequest.ExecutionDate < project.StartDate ||
                        editTaskRequest.ExecutionDate > project.FinishDate)
                        return new BaseRequestResponse<bool>()
                        {
                            Errors = new List<string>()
                            {
                                "The Execution date is out of the range between Project's Start and Finish Date "
                            }
                        };
                    task.ExecutionDate = editTaskRequest.ExecutionDate.Value;
                }

                task.Name = string.IsNullOrEmpty(editTaskRequest.Name) ? task.Name : editTaskRequest.Name;
                task.Description = string.IsNullOrEmpty(editTaskRequest.Description) ? task.Description : editTaskRequest.Description;
                
                _dbContext.Update(task);
                _dbContext.SaveChanges();
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
            var project = _dbContext.Projects.Include(x => x.Tasks).AsNoTracking().FirstOrDefault(x => x.Id == id);
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
                    Data = _dbContext.Projects.Include(x => x.Tasks).AsNoTracking().ToList()
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
            var task = _dbContext.Tasks.Include(x => x.Project).AsNoTracking().FirstOrDefault(x => x.Id == id);
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
                    Data = _dbContext.Tasks.Include(x => x.Project).AsNoTracking().ToList()
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