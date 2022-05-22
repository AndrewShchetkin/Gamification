using Gamification.Models;
using System.Threading.Tasks;

namespace Gamification.Hubs.Clients
{
    public interface IMapClient
    {
        Task UpdateCell(Cell cell);
    }
}
