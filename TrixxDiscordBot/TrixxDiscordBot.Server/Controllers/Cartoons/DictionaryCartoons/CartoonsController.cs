using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trixx.Cartoons.Database;
using Trixx.Cartoons.Database.Models.Dictionary;
using Trixx.Common.Utils;
using Trixx.Database.Enums;
using TrixxDiscordBot.Server.Controllers.Cartoons.DictionaryCartoons.Models;

namespace TrixxDiscordBot.Server.Controllers.Cartoons.Cartoons
{
    [TrixxClaimsAuthorize(Permission.DictionaryCartoons_Read)]
    public class CartoonsController(
        CartoonsDatabaseContext cartoonsDatabaseContext) : ApiController
    {
        private readonly CartoonsDatabaseContext _cartoonsDatabaseContext = cartoonsDatabaseContext;

        [HttpGet]
        public async Task<IReadOnlyList<CartoonsListSelectItem>> GetCartoonsListAsync()
        {
            return await _cartoonsDatabaseContext.DictionaryCartoons
                .Select(dc => new CartoonsListSelectItem
                {
                    Id = dc.Id,
                    Label = dc.Name,
                    AlternativeNames = dc.AlternativeNames,
                    Sources = dc.Sources,
                    Studios = dc.Studios.Select(x => x.DictionaryStudio.Name).ToList(),
                    Year = dc.Year,
                })
                .OrderBy(dc => dc.Id)
                .ToListAsync();
        }

        [HttpDelete("id")]
        [TrixxClaimsAuthorize(Permission.DictionaryCartoons_Delete)]
        public async Task DeleteCartoonAsync(int id)
        {
            var cartoon = await _cartoonsDatabaseContext.DictionaryCartoons.FirstAsync(dc => dc.Id == id);

            _cartoonsDatabaseContext.DictionaryCartoons.Remove(cartoon);
            await _cartoonsDatabaseContext.SaveChangesAsync();
        }

        [HttpPost]
        [TrixxClaimsAuthorize(Permission.DictionaryCartoons_Edit)]
        public async Task CreateUpdateCartoonAsync(CartoonUpdateModel model)
        {
            DictionaryCartoon cartoon;
            if (model.Id is null)
            {
                cartoon = new DictionaryCartoon();
            }
            else
            {
                cartoon = await _cartoonsDatabaseContext.DictionaryCartoons.FirstAsync(dc => dc.Id == model.Id);
            }

            cartoon.Name = model.Name;
            cartoon.Year = model.Year;
            cartoon.AlternativeNames = model.AlternativeNames;
            cartoon.Sources = model.Sources;

            var studiosMerger = new ManyToManyDbMerger<DictionaryCatroonStudio>(_cartoonsDatabaseContext);
            await studiosMerger.MergeAsync(
                model.Studios,
                x => x.CartoonId == model.Id,
                (x, i) => x.CartoonId == i.Id,
                i => new DictionaryCatroonStudio
                {
                    CartoonId = cartoon.Id,
                    DictionaryStudioId = i.Id,
                }
                );

            await _cartoonsDatabaseContext.SaveChangesAsync();
        }
    }
}
