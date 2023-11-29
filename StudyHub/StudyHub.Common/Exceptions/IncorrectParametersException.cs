using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyHub.Common.Exceptions;
public class IncorrectParametersException : Exception
{
    public IncorrectParametersException(string? message) 
        : base(message) { }
}
