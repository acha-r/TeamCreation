using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Numerics;
using WebApi.DTOs;
using WebApi.Entities;
using WebApi.Helpers;

namespace WebApi.Services
{
    public class TeamSelection : ITeamSelection
    {
        private readonly DataContext Context;
        private readonly List<string> AvailableSkills;

        public TeamSelection(DataContext context)
        {
            Context = context;
            AvailableSkills = new List<string> { "defense", "strength", "speed", "stamina", "attack" };
        }
        public IEnumerable<Player> GetTeam(List<TeamNeed> request)
        {
            var isCool = request.DistinctBy(x => x.Position);

            List<Player> selectedPlayers = new List<Player>();
            Dictionary<string, int> positionPlayerNum = new Dictionary<string, int>();
            Dictionary<string, string> positionComb = new Dictionary<string, string>();
            List<Dictionary<string, string>> listofDict = new List<Dictionary<string, string>>();

            var positionPlayers = Context.Players.Include(p => p.PlayerSkills).ToList().GroupBy(p => p.Position)
                .ToDictionary(x => x.Key, x => x.ToList());

            foreach (var need in request)
            {

                if (!positionPlayers.ContainsKey(need.Position))
                    throw new ArgumentException($"Insufficient number of player for position: {need.Position}");

                if (positionPlayers[need.Position].Count < need.NumberOfPlayers)
                    throw new ArgumentException($"Insufficient number of player for position: {need.Position}");

                var sortedPlayers = positionPlayers[need.Position]
                    .OrderByDescending(p => p.PlayerSkills
                    .FirstOrDefault(s => s.Skill == need.MainSkill)?.Value ?? 0)
                    .ToList();

                List<Player> chosen = new List<Player>();

                foreach (var player in sortedPlayers)
                {
                    if (player.PlayerSkills.Any(s =>  s.Skill == need.MainSkill))
                        chosen.Add(player);
                }

                if (chosen.Count == 0)
                    chosen.Add(sortedPlayers[0]);

                selectedPlayers.AddRange(chosen.Take(need.NumberOfPlayers));

                if (positionComb.ContainsKey(need.Position) && (positionComb[need.Position] == need.MainSkill))
                    throw new ArgumentException($"This combination already exists");
                else
                {
                    try
                    {
                        positionComb.Add(need.Position, need.MainSkill);

                    }
                    catch (Exception)
                    {
                        new 
                        listofDict.Add(new Dictionary<string, string> (need.Position, );
                    } 
                }
            begin:
                positionPlayerNum[need.Position] = 0;

                positionPlayerNum[need.Position] += need.NumberOfPlayers;
            }

            foreach (var req in positionPlayerNum)
            {
                if (positionPlayers[req.Key].Count < req.Value)
                    throw new ArgumentException($"Insufficient number of players for position: {req.Key}");
            }

            return selectedPlayers.Distinct();
        }
    }
}
