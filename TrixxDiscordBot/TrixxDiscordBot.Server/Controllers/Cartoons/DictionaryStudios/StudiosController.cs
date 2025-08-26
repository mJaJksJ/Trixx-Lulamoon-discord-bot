using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trixx.Cartoons.Database;
using Trixx.Cartoons.Database.Models.Dictionary;
using Trixx.Database.Enums;
using TrixxDiscordBot.Server.Controllers.Cartoons.DictionaryStudios.Models;

namespace TrixxDiscordBot.Server.Controllers.Studios.DictionaryStudios
{
    [TrixxClaimsAuthorize(Permission.DictionaryStudios_Read)]
    public class StudiosController(
        CartoonsDatabaseContext CartoonsDatabaseContext) : ApiController
    {
        private readonly CartoonsDatabaseContext _cartoonsDatabaseContext = CartoonsDatabaseContext;

        [HttpGet]
        public async Task<IReadOnlyList<StudiosListSelectItem>> GetStudiosListAsync()
        {
            return await _cartoonsDatabaseContext.DictionaryStudios
                .Select(ds => new StudiosListSelectItem
                {
                    Id = ds.Id,
                    Label = ds.Name
                })
                .OrderBy(ds => ds.Label)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<StudiosListSelectItem?> GetStudioAsync(int id)
        {
            return await _cartoonsDatabaseContext.DictionaryStudios
                .Select(ds => new StudiosListSelectItem
                {
                    Id = ds.Id,
                    Label = ds.Name
                })
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        [HttpDelete("{id}")]
        [TrixxClaimsAuthorize(Permission.DictionaryStudios_Delete)]
        public async Task DeleteStudioAsync(int id)
        {
            var studio = await _cartoonsDatabaseContext.DictionaryStudios.FirstAsync(ds => ds.Id == id);

            _cartoonsDatabaseContext.DictionaryStudios.Remove(studio);
            await _cartoonsDatabaseContext.SaveChangesAsync();
        }

        [HttpPost]
        [TrixxClaimsAuthorize(Permission.DictionaryStudios_Edit)]
        public async Task CreateUpdateStudioAsync(StudioUpdateModel model)
        {
            DictionaryStudio studio;
            if (model.Id is null)
            {
                studio = new DictionaryStudio();
            }
            else
            {
                studio = await _cartoonsDatabaseContext.DictionaryStudios.FirstAsync(dc => dc.Id == model.Id);
            }

            studio.Name = model.Name;

            if (model.Id is null)
            {
                _cartoonsDatabaseContext.DictionaryStudios.Add(studio);
            }
            await _cartoonsDatabaseContext.SaveChangesAsync();
        }
    }
}
