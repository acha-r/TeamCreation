using WebApi.DTOs;
using WebApi.Entities;

namespace WebApi.Services.Interface
{
    public interface IPlayerService
    {
        Task<Player> CreatePlayer(PlayerDTO request);
        Task<Player> UpdatePlayer(int playerId, PlayerDTO request);
        Task<IEnumerable<Player>> GetAllPlayers();
        Task<Player> DeletePlayer(int playerId);
    }
}
