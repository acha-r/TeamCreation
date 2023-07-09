using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WebApi.DTOs;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Services.Interface;

namespace WebApi.Services.Implementation
{
    public class TeamSelection : ITeamSelection
    {
        private readonly DataContext Context;

        public TeamSelection(DataContext context)
        {
            Context = context;
        }

        public IEnumerable<Player> GetTeam(List<TeamNeed> request)
        {
            var distinctPositions = request.DistinctBy(x => x.Position);
            var positionPlayers = GetPositionPlayers();

            var team = new List<Player>();
            var requiredNumOfPlayers = new Dictionary<string, int>();
            var positionSkillComb = new HashSet<string>();
            var selectedPlayerNames = new HashSet<string>();

            foreach (var need in request)
            {
                ValidatePositionAvailability(need.Position, positionPlayers);
                ValidateSufficientPlayers(need.Position, need.NumberOfPlayers, positionPlayers);
                ValidateDuplicatePositionSkillCombination(need.Position, need.MainSkill, positionSkillComb);

                positionSkillComb.Add($"{need.Position}-{need.MainSkill}");

                var sortedPlayers = GetSortedPlayersBySkill(need.Position, need.MainSkill, positionPlayers);

                var chosen = GetChosenPlayers(sortedPlayers, need.MainSkill, selectedPlayerNames);
                AddFallbackPlayerIfNoSkills(chosen, sortedPlayers, selectedPlayerNames);

                team.AddRange(chosen.Take(need.NumberOfPlayers));

                UpdateRequiredNumOfPlayers(need.Position, need.NumberOfPlayers, requiredNumOfPlayers);
            }

            EnsureAllPositionRequirementsMet(positionPlayers, requiredNumOfPlayers);

            return team;
        }

        private Dictionary<string, List<Player>> GetPositionPlayers()
        {
            return Context.Players.Include(p => p.PlayerSkills)
                                  .ToList()
                                  .GroupBy(p => p.Position)
                                  .ToDictionary(x => x.Key, x => x.ToList());
        }

        private void ValidatePositionAvailability(string position, Dictionary<string, List<Player>> positionPlayers)
        {
            if (!positionPlayers.ContainsKey(position))
                throw new ArgumentException($"Insufficient number of players for position: {position}");
        }

        private void ValidateSufficientPlayers(string position, int numOfPlayers, Dictionary<string, List<Player>> positionPlayers)
        {
            if (positionPlayers[position].Count < numOfPlayers)
                throw new ArgumentException($"Insufficient number of players for position: {position}");
        }

        private void ValidateDuplicatePositionSkillCombination(string position, string skill, HashSet<string> positionSkillComb)
        {
            if (positionSkillComb.Contains($"{position}-{skill}"))
                throw new ArgumentException($"Duplicate position and skill combination: {position} - {skill}");
        }

        private List<Player> GetSortedPlayersBySkill(string position, string skill, Dictionary<string, List<Player>> positionPlayers)
        {
            return positionPlayers[position]
                .OrderByDescending(p => p.PlayerSkills.FirstOrDefault(s => s.Skill == skill)?.Value ?? 0)
                .ToList();
        }

        private List<Player> GetChosenPlayers(List<Player> sortedPlayers, string skill, HashSet<string> selectedPlayerNames)
        {
            var chosen = new List<Player>();
            foreach (var player in sortedPlayers)
            {
                if (player.PlayerSkills.Any(s => s.Skill == skill) && !selectedPlayerNames.Contains(player.Name))
                {
                    chosen.Add(player);
                    selectedPlayerNames.Add(player.Name);
                }
            }
            return chosen;
        }

        private void AddFallbackPlayerIfNoSkills(List<Player> chosen, List<Player> sortedPlayers, HashSet<string> selectedPlayerNames)
        {
            if (chosen.Count == 0 && !selectedPlayerNames.Contains(sortedPlayers[0].Name))
            {
                chosen.Add(sortedPlayers[0]);
                selectedPlayerNames.Add(sortedPlayers[0].Name);
            }
        }

        private void UpdateRequiredNumOfPlayers(string position, int numOfPlayers, Dictionary<string, int> requiredNumOfPlayers)
        {
            if (!requiredNumOfPlayers.ContainsKey(position))
                requiredNumOfPlayers[position] = 0;
            requiredNumOfPlayers[position] += numOfPlayers;
        }

        private void EnsureAllPositionRequirementsMet(Dictionary<string, List<Player>> positionPlayers, Dictionary<string, int> requiredNumOfPlayers)
        {
            foreach (var req in requiredNumOfPlayers)
            {
                if (positionPlayers[req.Key].Count < req.Value)
                    throw new ArgumentException($"Insufficient number of players for position: {req.Key}");
            }
        }
    }
}
