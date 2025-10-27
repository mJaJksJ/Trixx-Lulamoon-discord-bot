using Microsoft.AspNetCore.Mvc;
using Trixx.Cartoons.Database;
using Trixx.Cartoons.Services.Pack;
using Trixx.Cartoons.Services.Pack.Models;
using Trixx.Common.Models;
using Trixx.Database.Enums;
using TrixxDiscordBot.Server.Startup.Auth;

namespace TrixxDiscordBot.Server.Controllers.Cartoons.Packs
{
    [TrixxClaimsAuthorize(Permission.CartoonsPack_Read)]
    public class CartoonPackController(CartoonsPackService cartoonsPackService) : ApiController
    {
        private readonly CartoonsPackService _cartoonsPackService = cartoonsPackService;

        [HttpGet]
        public async Task<IReadOnlyCollection<CartoonsPackListItem>> GetCartoonPacksAsync()
        {
            return await _cartoonsPackService.GetCartoonPacksAsync();
        }

        [HttpGet("{id}")]
        public async Task<SelectItem> GetCartoonPackAsync(int id)
        {
            return await _cartoonsPackService.GetCartoonPackAsync(id);
        }

        [HttpGet("card/{id}")]
        public async Task<CartoonsPackModel> GetCartoonPackCardAsync(int id)
        {
            return await _cartoonsPackService.GetCartoonPackCardAsync(id);
        }

        [HttpPost("edit-cartoons-pack")]
        [TrixxClaimsAuthorize(Permission.CartoonsPack_Edit)]
        public async Task EditCartoonPackAsync(CartoonsPackEditModel model)
        {
            await _cartoonsPackService.EditCartoonPackAsync(model, User.GetId());
        }

        [HttpPost("edit-label-type")]
        [TrixxClaimsAuthorize(Permission.CartoonsPack_Edit)]
        public async Task EditLabelTypeAsync(CartoonsPackLabelTypeEditModel model)
        {
            await _cartoonsPackService.EditLabelTypeAsync(model, User.GetId());
        }

        [HttpDelete("delete-label-type/{id}")]
        [TrixxClaimsAuthorize(Permission.CartoonsPack_Edit)]
        public async Task DeleteLabelTypeAsync(int id)
        {
            await _cartoonsPackService.DeleteLabelTypeAsync(id);
        }

        [HttpPost("replace-cartoon/{packId}/{labelTypeId}/{dictionaryCartoonId}")]
        [TrixxClaimsAuthorize(Permission.CartoonsPack_Edit)]
        public async Task ReplaceCartoon(int packId, int labelTypeId, int dictionaryCartoonId)
        {
            await _cartoonsPackService.ReplaceCartoon(packId, labelTypeId, dictionaryCartoonId, User.GetId());
        }
    }
}
