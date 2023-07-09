using Microsoft.EntityFrameworkCore;
using WebApi.DTOs;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Services.Interface;

namespace WebApi.Services.Implementation
{
    public class PlayerService : IPlayerService
    {
        private readonly DataContext Context;
        private readonly List<string> AvailablePositions;
        private readonly List<string> AvailableSkills;

        public PlayerService(DataContext context)
        {
            Context = context;
            AvailablePositions = new List<string> { "defender", "midfielder", "forward" };
            AvailableSkills = new List<string> { "defense", "strength", "speed", "stamina", "attack" };
        }


        public async Task<Player> CreatePlayer(PlayerDTO request)
        {

            if (request == null) throw new ArgumentNullException();

            if (!AvailablePositions.Contains(request.Position.ToLower())) throw new ArgumentException($"Invalid value for position: {request.Position}");

            Player newPlayer = new()
            {
                Name = request.Name,
                Position = request.Position,
                PlayerSkills = new List<PlayerSkill>()
            };

            foreach (var item in request.PlayerSkills)
            {
                var newSkill = AddSkill(item.Skill, item.Value);
                newPlayer.PlayerSkills.Add(newSkill);
            }

            await Context.AddAsync(newPlayer);
            await Context.SaveChangesAsync();
            return newPlayer;

        }

        public async Task<Player> UpdatePlayer(int playerId, PlayerDTO request)
        {
            var player = await Context.Players.Where(x => x.Id == playerId).Include(x => x.PlayerSkills).FirstOrDefaultAsync()
                ?? throw new KeyNotFoundException($"Invalid value for player id {playerId}");

            player.Position = request.Position;
            player.Name = request.Name;

            foreach (var item in request.PlayerSkills)
            {
                foreach (var skill in player.PlayerSkills)
                {
                    if (skill.Skill == item.Skill)
                    {
                        skill.Value = item.Value;
                    }
                }
                var newSkill = AddSkill(item.Skill, item.Value);
                player.PlayerSkills.Add(newSkill);
            }

            Context.Update(player);
            await Context.SaveChangesAsync();
            return player;
        }

        public async Task<IEnumerable<Player>> GetAllPlayers()
        {
            return await Context.Players.Include(x => x.PlayerSkills).ToListAsync();
        }


        public async Task<Player> DeletePlayer(int playerId)
        {
            var player = await Context.Players.FindAsync(playerId);

            Context.Players.Remove(player);
            await Context.SaveChangesAsync();

            return player;
        }


        //reusable method for CreatePlayer and UpdatePlayer
        private PlayerSkill AddSkill(string skill, int value)
        {
            if (!AvailableSkills.Contains(skill.Trim().ToLower())) throw new ArgumentException($"Invalid value for skill: {skill}");

            if (value <= 0 || value > 100) throw new ArgumentException($"Invalid number for skill: {skill}");

            PlayerSkill newPlayerSkill = new()
            {
                Skill = skill,
                Value = value
            };
            return newPlayerSkill;
        }

    }
}
