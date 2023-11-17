using AutoMapper;
using StudyHub.Common.DTO.AssignmentTaskOption;
using StudyHub.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyHub.BLL.Profiles;
public class OptionsProfile : Profile
{
    public OptionsProfile()
    {
        CreateMap<AssignmentTaskOptionDTO, ChoiceOption>().ReverseMap();
        CreateMap<AssignmentTaskOptionDTO, OpenEndedOption>().ReverseMap();
    }
}
