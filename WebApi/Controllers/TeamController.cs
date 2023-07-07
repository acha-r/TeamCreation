// /////////////////////////////////////////////////////////////////////////////
// YOU CAN FREELY MODIFY THE CODE BELOW IN ORDER TO COMPLETE THE TASK
// /////////////////////////////////////////////////////////////////////////////

using Microsoft.AspNetCore.Mvc;
using WebApi.Entities;
using WebApi.Helpers;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamController
    {
        private readonly DataContext Context;

        public TeamController(DataContext context)
        {
            Context = context;
        }

        [HttpPost("process")]
        public async Task<ActionResult<List<Player>>> Process()
        {
            await Task.Run(() => Context.Players.FirstOrDefault(x => x.Id == 5));
            throw new NotImplementedException();
        }
    }
}
