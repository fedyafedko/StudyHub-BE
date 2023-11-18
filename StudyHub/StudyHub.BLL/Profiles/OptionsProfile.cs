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
        CreateMap<CreateAssignmentTaskOptionDTO, ChoiceOption>();
        CreateMap<CreateAssignmentTaskOptionDTO, OpenEndedOption>();
        CreateMap<ChoiceOption, AssignmentTaskOptionDTO>();
        CreateMap<OpenEndedOption, AssignmentTaskOptionDTO>();
    }
}
