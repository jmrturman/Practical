using Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainService.Interfaces
{
    public interface IPracticalService
    {
        Task<IEnumerable<PracticalResult>> CallServices(string ipAddress, IEnumerable<string> service);
    }
}
