using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyHub.Common.Exceptions;
public class IncorrectParametersError : Exception
{
    public IncorrectParametersError(string? message) 
        : base(message) { }
}
