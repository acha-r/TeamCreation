using WebApi.DTOs;
using WebApi.Entities;

namespace WebApi.Services.Interface
{
    public interface ITeamSelection
    {
        IEnumerable<Player> GetTeam(List<TeamNeed> request);
    }
}
