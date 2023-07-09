using WebApi.DTOs;
using WebApi.Entities;

namespace WebApi.Services
{
    public interface ITeamSelection
    {
       IEnumerable<Player> GetTeam(List<TeamNeed> request);
    }
}
