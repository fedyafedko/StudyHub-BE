using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyHub.Common.Exceptions;
public class InvalidSecurityAlgorithmException : Exception
{
    public InvalidSecurityAlgorithmException
        (string? message) : base(message) { }
}
