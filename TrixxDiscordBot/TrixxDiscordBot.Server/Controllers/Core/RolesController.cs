using Microsoft.AspNetCore.Mvc;
using Trixx.Core.Services.Roles.Models;
using Trixx.Core.Services.Roles;
using Trixx.Database.Enums;
using Trixx.Database;

namespace TrixxDiscordBot.Server.Controllers.Core
{
    public class RolesController(DatabaseContext databaseContext, RolesService rolesService) : ApiController
    {
        private readonly DatabaseContext _databaseContext = databaseContext;
        private readonly RolesService _rolesService = rolesService;

        [HttpPost("edit-role")]
        [TrixxClaimsAuthorize(Permission.TrixxRoles_Edit)]
        public async Task EditRoleAsync(RoleEditModel model)
        {
            await _rolesService.EditRoleAsync(model);
        }

        [HttpGet("roles")]
        [TrixxClaimsAuthorize(Permission.TrixxRoles_Read)]
        public async Task<IEnumerable<RoleListItem>> GetRolesAsync()
        {
            return await _rolesService.GetRolesAsync();
        }

        [HttpGet("{id}")]
        [TrixxClaimsAuthorize(Permission.TrixxRoles_Read)]
        public async Task<RoleModel> GetRoleAsync(int id)
        {
            return await _rolesService.GetRoleAsync(id);
        }
    }
}
