﻿using AutoMapper;
using StudyHub.Common.DTO.AssignmentTaskOption;
using StudyHub.Entities;

namespace StudyHub.BLL.Profiles;

public class OptionsProfile : Profile
{
    public OptionsProfile()
    {
        CreateMap<TaskOption, TaskOptionDTO>();
        CreateMap<CreateTaskOptionDTO, TaskOption>();
        CreateMap<UpdateTaskOptionDTO, TaskOption>();
    }
}