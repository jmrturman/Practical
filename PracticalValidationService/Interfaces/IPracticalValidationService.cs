using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PracticalValidation.Interfaces
{
    public interface IPracticalValidationService
    {
        string Validate(string ipAddress, out bool isValid);

        string Validate(IEnumerable<string> serviceList, out bool isValid);
    }
}
