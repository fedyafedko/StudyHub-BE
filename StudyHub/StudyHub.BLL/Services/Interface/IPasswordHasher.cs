using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyHub.BLL.Services.Interface;
public interface IPasswordHasher
{
    string Hash (string password);
}
