using StudyHub.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyHub.BLL.Services.Interfaces;
public interface IUserInvitedService
{
    Task CreateRegistrationUrl(string email, string role);
}
