using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trixx.Cartoons.Database;
using Trixx.Cartoons.Database.Models.Dictionary;
using Trixx.Common.Models;
using Trixx.Common.Utils;
using Trixx.Database;
using Trixx.Database.Enums;
using TrixxDiscordBot.Server.Controllers.Cartoons.DictionaryCartoons.Models;
using TrixxDiscordBot.Server.Startup.Auth;

namespace TrixxDiscordBot.Server.Controllers.Cartoons.Cartoons
{
    [TrixxClaimsAuthorize(Permission.DictionaryCartoons_Read)]
    public class CartoonsController(
        CartoonsDatabaseContext cartoonsDatabaseContext,
        DatabaseContext databaseContext) : ApiController
    {
        private readonly CartoonsDatabaseContext _cartoonsDatabaseContext = cartoonsDatabaseContext;
        private readonly DatabaseContext _databaseContext = databaseContext;

        [HttpGet]
        public async Task<IReadOnlyList<CartoonsListSelectItem>> GetCartoonsListAsync()
        {
            return (await _cartoonsDatabaseContext.DictionaryCartoons
                .OrderByDescending(ds => ds.SystemObject.CreateDateTime)
                .Select(dc => new
                {
                    Id = dc.Id,
                    Label = dc.Name,
                    AlternativeNames = dc.AlternativeNames,
                    Sources = dc.Sources,
                    Studios = dc.Studios.Select(x => new SelectItem { Id = x.Id, Label = x.DictionaryStudio.Name }).ToList(),
                    Year = dc.Year,
                })
                .ToListAsync()) // TODO: разобраться почему падает у Sources если сразу выгружать из базы
                .Select(dc => new CartoonsListSelectItem
                {
                    Id = dc.Id,
                    Label = dc.Label,
                    AlternativeNames = dc.AlternativeNames,
                    Sources = dc.Sources.Select(x => new RefItem { Ref = x, Label = x }).ToList(),
                    Studios = dc.Studios,
                    Year = dc.Year,
                })
                .ToList();
        }

        [HttpGet("{id}")]
        public async Task<CartoonsListSelectItem?> GetCartoonAsync(int id)
        {
            var item = await _cartoonsDatabaseContext.DictionaryCartoons
                .Select(dc => new
                {
                    Id = dc.Id,
                    Label = dc.Name,
                    AlternativeNames = dc.AlternativeNames,
                    Sources = dc.Sources,
                    Studios = dc.Studios.Select(x => new SelectItem { Id = x.Id, Label = x.DictionaryStudio.Name }).ToList(),
                    Year = dc.Year,
                })
                .FirstOrDefaultAsync(x => x.Id == id);
            // TODO: разобраться почему падает у Sources если сразу выгружать из базы
            return item != null ? new CartoonsListSelectItem
            {
                Id = item.Id,
                Label = item.Label,
                AlternativeNames = item.AlternativeNames,
                Sources = item.Sources.Select(x => new RefItem { Ref = x, Label = x }).ToList(),
                Studios = item.Studios,
                Year = item.Year,
            } : null;
        }


        [HttpDelete("{id}")]
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
            var userId = User.GetId();
            var user = await _databaseContext.Users
                .Where(x => x.Id == userId)
                .Select(x => x.FullName)
                .FirstAsync();

            DictionaryCartoon cartoon;
            if (model.Id is null)
            {
                cartoon = new DictionaryCartoon
                {
                    SystemObject = new Trixx.Cartoons.Database.Models.CartoonSystemObject
                    {
                        CreateDateTime = DateTime.Now,
                        Type = Trixx.Cartoons.Database.Enums.SystemObjectType.DictionaryCartoon,
                        Creator = user
                    }
                };
            }
            else
            {
                cartoon = await _cartoonsDatabaseContext.DictionaryCartoons
                    .Include(x => x.SystemObject)
                    .FirstAsync(dc => dc.Id == model.Id);
            }

            cartoon.Name = model.Name;
            cartoon.Year = model.Year;
            cartoon.AlternativeNames = model.AlternativeNames;
            cartoon.Sources = model.Sources;

            var studiosMerger = new ManyToManyDbMerger<DictionaryCatroonStudio>(_cartoonsDatabaseContext);

            if (model.Id is null)
            {
                _cartoonsDatabaseContext.DictionaryCartoons.Add(cartoon);
            }
            await _cartoonsDatabaseContext.SaveChangesAsync();

            await studiosMerger.MergeAsync(
                model.Studios,
                x => x.CartoonId == model.Id,
                (x, i) => x.CartoonId == i,
                i => new DictionaryCatroonStudio
                {
                    CartoonId = cartoon.Id,
                    DictionaryStudioId = i,
                }
                );
            await _cartoonsDatabaseContext.SaveChangesAsync();
        }

        [HttpGet("studios")]
        public async Task<IReadOnlyList<SelectItem>> GetStudiosAsync()
        {
            return await _cartoonsDatabaseContext.DictionaryStudios
                .Select(dc => new SelectItem
                {
                    Id = dc.Id,
                    Label = dc.Name,
                })
                .OrderBy(dc => dc.Label)
                .ToListAsync();
        }
    }
}
