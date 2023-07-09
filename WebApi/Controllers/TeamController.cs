// /////////////////////////////////////////////////////////////////////////////
// YOU CAN FREELY MODIFY THE CODE BELOW IN ORDER TO COMPLETE THE TASK
// /////////////////////////////////////////////////////////////////////////////

using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Services.Interface;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamController : ControllerBase
    {
        private readonly DataContext Context;
        private ITeamSelection Team;

        public TeamController(DataContext context, ITeamSelection team)
        {
            Team = team;
            Context = context;
        }

        [HttpPost("process")]
        public IActionResult Process([FromBody] List<TeamNeed> need)
        {
            return Ok(Team.GetTeam(need));
           // await Task.Run(() => Context.Players.FirstOrDefault(x => x.Id == 5));
        }
    }
}
