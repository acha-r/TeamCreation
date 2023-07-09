// /////////////////////////////////////////////////////////////////////////////
// YOU CAN FREELY MODIFY THE CODE BELOW IN ORDER TO COMPLETE THE TASK
// /////////////////////////////////////////////////////////////////////////////

namespace WebApi.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApi.DTOs;
using WebApi.Entities;
using WebApi.Extensions;
using WebApi.Helpers;
using WebApi.Services.Interface;

[ApiController]
[Route("api/[controller]")]
public class PlayerController : ControllerBase
{
    private readonly IPlayerService Player;

    public PlayerController(IPlayerService player)
    {
        Player = player;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Player>>> GetAll()
    {
        return Ok(await Player.GetAllPlayers());
    }

    [HttpPost]
    public async Task<ActionResult<Player>> PostPlayer([FromBody] PlayerDTO player)
    {
        return Ok(await Player.CreatePlayer(player));
        // await Task.Run(() => Context.Players.FirstOrDefault(x => x.Id == 2));

    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutPlayer(int id, [FromBody] PlayerDTO player)
    {
        return Ok(await Player.UpdatePlayer(id, player));
        // await Task.Run(() => Context.Players.FirstOrDefault(x => x.Id == 3));

    }

    [TokenRequirement]
    [HttpDelete("{id}")]
    public async Task<ActionResult<Player>> DeletePlayer(int id)
    {
            return Ok(await Player.DeletePlayer(id));
      
        //await Task.Run(() => Context.Players.FirstOrDefault(x => x.Id == 4));
    }
}