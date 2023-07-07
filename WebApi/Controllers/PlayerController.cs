// /////////////////////////////////////////////////////////////////////////////
// YOU CAN FREELY MODIFY THE CODE BELOW IN ORDER TO COMPLETE THE TASK
// /////////////////////////////////////////////////////////////////////////////

namespace WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Helpers;
using WebApi.Entities;

[ApiController]
[Route("api/[controller]")]
public class PlayerController : ControllerBase
{
  private readonly DataContext Context;

  public PlayerController(DataContext context)
  {
    Context = context;
  }

  [HttpGet]
  public async Task<ActionResult<IEnumerable<Player>>> GetAll()
  {
    await Task.Run(() => Context.Players.FirstOrDefault( x => x.Id == 1));
    throw new NotImplementedException();
  }

  [HttpPost]
  public async Task<ActionResult<Player>> PostPlayer()
  {
    await Task.Run(() => Context.Players.FirstOrDefault(x => x.Id == 2));
    throw new NotImplementedException();
  }

  [HttpPut("{id}")]
  public async Task<IActionResult> PutPlayer(int id, Player player)
  {
    await Task.Run(() => Context.Players.FirstOrDefault(x => x.Id == 3));
    throw new NotImplementedException();
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult<Player>> DeletePlayer(int id)
  {
    await Task.Run(() => Context.Players.FirstOrDefault(x => x.Id == 4));
    throw new NotImplementedException();
  }
}