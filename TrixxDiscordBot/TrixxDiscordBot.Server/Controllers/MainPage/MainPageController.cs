using Microsoft.AspNetCore.Mvc;
using Trixx.Database.Enums;

namespace TrixxDiscordBot.Server.Controllers.MainPage
{
    [TrixxClaimsAuthorize(Permission.MainPage_Read)]
    public class MainPageController : ApiController
    {
        [HttpGet]
        public string Get()
        {
            return "Hello by Trixx Servise!";
        }
    }
}
