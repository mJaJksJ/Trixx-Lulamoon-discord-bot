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
        CartoonsDatabaseContext StudiosDatabaseContext) : ApiController
    {
        private readonly CartoonsDatabaseContext _studiosDatabaseContext = StudiosDatabaseContext;

        [HttpGet]
        public async Task<IReadOnlyList<StudiosListSelectItem>> GetStudiosListAsync()
        {
            return await _studiosDatabaseContext.DictionaryStudios
                .Select(ds => new StudiosListSelectItem
                {
                    Id = ds.Id,
                    Label = ds.Name
                })
                .OrderBy(ds => ds.Label)
                .ToListAsync();
        }

        [HttpDelete("id")]
        [TrixxClaimsAuthorize(Permission.DictionaryStudios_Delete)]
        public async Task DeleteStudioAsync(int id)
        {
            var studio = await _studiosDatabaseContext.DictionaryStudios.FirstAsync(ds => ds.Id == id);

            _studiosDatabaseContext.DictionaryStudios.Remove(studio);
            await _studiosDatabaseContext.SaveChangesAsync();
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
                studio = await _studiosDatabaseContext.DictionaryStudios.FirstAsync(dc => dc.Id == model.Id);
            }

            studio.Name = model.Name;

            await _studiosDatabaseContext.SaveChangesAsync();
        }
    }
}
