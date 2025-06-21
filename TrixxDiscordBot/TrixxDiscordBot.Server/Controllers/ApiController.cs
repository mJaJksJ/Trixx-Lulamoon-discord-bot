using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TrixxDiscordBot.Server.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    [Authorize]
    public abstract class ApiController : Controller
    {

    }
}
