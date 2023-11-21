using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyHub.Common.Exceptions;
public class UserManagerException : Exception
{
    public UserManagerException(string message, IEnumerable<IdentityError> errors)
        : base($"{message} {string.Join("\n", errors.Select(e => e.Description))}")
    {
        Errors = errors;
    }

    public IEnumerable<IdentityError> Errors { get; }
}
