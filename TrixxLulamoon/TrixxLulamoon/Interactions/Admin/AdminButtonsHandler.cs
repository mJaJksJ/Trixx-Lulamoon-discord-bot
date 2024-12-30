using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using TrixxLulamoon.Config;
using TrixxLulamoon.Utils;
using TrixxLulamoon.Utils.Excel;

namespace TrixxLulamoon.Interactions.Admin
{
    public class AdminButtonsHandler
    {
        private readonly DiscordShardedClient _discordShardedClient;
        private readonly Serilog.ILogger _logger;

        public AdminButtonsHandler(
            DiscordShardedClient discordShardedClient,
            Serilog.ILogger logger
        )
        {
            _discordShardedClient = discordShardedClient;
            _logger = logger;
        }

        public async Task ButtonExecuted(SocketMessageComponent component)
        {
            var customId = component.Data.CustomId;
            var context = new ShardedInteractionContext(_discordShardedClient, component);
            switch (customId)
            {
                case Consts.ADMIN_SPAWN_MODAL_ROLE_BUTTON_ID:
                    await SendSpawnRoleButtons(context);
                    break;

                case Consts.ADMIN_SPAWN_MODAL_MESSAGE_BUTTON_ID:
                    await SpawnSendMessageModal(context);
                    break;

                case Consts.ADMIN_SPAWN_ROLES_TABLE_BUTTON_ID:
                    await SendSpawnRolesTablesButtons(context);
                    break;

                case Consts.ADMIN_SPAWN_FIX_ROLES_BUTTON_ID:
                    break;

                default:
                    break;
            }
        }

        public async Task SendSpawnRoleButtons(ShardedInteractionContext context)
        {
            var mb = new ModalBuilder()
                .WithTitle("Создать кнопки смены роли")
                .WithCustomId(Consts.ROLE_BUTTON_MODAL_ID)
                .AddTextInput("Канал", Consts.ROLE_BUTTON_MODAL_CHANNEL_ID, required: true)
                .AddTextInput("Id роли", Consts.ROLE_BUTTON_MODAL_ROLE_ID, required: true);

            await context.Interaction.RespondWithModalAsync(mb.Build());
        }

        public async Task SpawnSendMessageModal(ShardedInteractionContext context)
        {
            var mb = new ModalBuilder()
                .WithTitle("Отправить сообщение")
                .WithCustomId(Consts.MESSAGE_MODAL_ID)
                .AddTextInput("Канал", Consts.MESSAGE_MODAL_CHANNEL_ID, required: true, value: ConfigModel.Instance.ChannelsConfig.DefaultChatChannelId.ToString())
                .AddTextInput("Ответ на (необязательно)", Consts.MESSAGE_MODAL_RESPONSABLE_MESSAGE_ID, required: false)
                .AddTextInput("Сообщение", Consts.MESSAGE_MODAL_TEXT_ID, required: true, style: TextInputStyle.Paragraph);

            await context.Interaction.RespondWithModalAsync(mb.Build());
        }

        public async Task SendSpawnRolesTablesButtons(ShardedInteractionContext context)
        {
            var filePath = Path.Combine(Path.GetDirectoryName(typeof(AdminButtonsHandler).Assembly.Location), "Utils", "Excel", "Data", "Roles.xlsx");
            var workbook = new Workbook(filePath);
            var table = new Table(Style.Default(workbook.Instance));

            var roles = context.Guild.Roles;
            table.Tr(row =>
            {
                row.Td(string.Empty);
                row.Td(string.Empty);
                foreach (var role in roles)
                {
                    row.Td(role.Name, Style.DefaultWithBorder(workbook.Instance));
                }
                row.Td(string.Empty, Style.Default(workbook.Instance, Style.Border.Left));
            });

            foreach (var permission in Enum.GetValues<GuildPermission>())
            {
                table.Tr(row =>
                {
                    row.Td(permission.RusName(), Style.Default(workbook.Instance, Style.Border.Right));
                    row.Td(permission.RusDescription(), Style.Default(workbook.Instance, Style.Border.Right));
                    foreach (var role in roles)
                    {
                        var has = role.Permissions.Has(permission);
                        row.Td(string.Empty, Style.Default(workbook.Instance, color: new NPOI.XSSF.UserModel.XSSFColor([(byte)(has ? 0 : 255), (byte)(has ? 176 : 143), (byte)(has ? 80 : 143)])));
                    }
                    row.Td(string.Empty, Style.Default(workbook.Instance, Style.Border.Left));
                });
            }

            table.Tr(row =>
            {
                row.Td(string.Empty, Style.Default(workbook.Instance, Style.Border.Top));
                row.Td(string.Empty, Style.Default(workbook.Instance, Style.Border.Top));
                foreach (var role in roles)
                {
                    row.Td(string.Empty, Style.Default(workbook.Instance, Style.Border.Top));
                }
            });

            table.SaveChanges(workbook.GetSheetAt(0));

            var ms = new NpoiMemoryStream();
            ms.AllowClose = false;
            workbook.Instance.Write(ms);
            ms.Flush();
            ms.Seek(0, SeekOrigin.Begin);
            ms.AllowClose = true;

            await context.Interaction.RespondWithFileAsync(ms, "roles.xlsx", ephemeral: true);
            workbook.Instance.Close();
        }
    }
}
