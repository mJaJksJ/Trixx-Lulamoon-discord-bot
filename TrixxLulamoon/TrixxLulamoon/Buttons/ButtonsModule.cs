using Discord.Commands;
using Discord;
using TrixxLulamoon.Config;
using TrixxLulamoon.Utils;

namespace TrixxLulamoon.Buttons
{
    public class ButtonsModule : ModuleBase<SocketCommandContext>
    {
        private readonly ConfigModel _configModel;

        public ButtonsModule()
        {
            _configModel = ConfigModel.Instance;
        }

        public async Task Response(ButtonIdType yesType, ButtonIdType noType, IEmote emoji)
        {
            var yesButton = new ButtonBuilder()
            {
                Label = "Хочу",
                CustomId = ((int)yesType).ToString(),
                Style = ButtonStyle.Success,
                Emote = emoji,
            };

            var noButton = new ButtonBuilder()
            {
                Label = "Больше нет",
                CustomId = ((int)noType).ToString(),
                Style = ButtonStyle.Danger,
                Emote = emoji,
            };

            await ReplyAsync($"Хотите ли вы иметь роль <@&{_configModel.RolesConfig.GetRoleId(yesType)}>?");
            await ReplyAsync("", components: new ComponentBuilder().WithButton(yesButton).Build());
            await ReplyAsync("", components: new ComponentBuilder().WithButton(noButton).Build());
        }

        [Command("!spawn-fluttershy")]
        [Summary("Spawning Fluttershy buttons")]
        public async Task Fluttershy()
        {
            await Response(ButtonIdType.Fluttershy, ButtonIdType.NotFluttershy, new Emoji("🦄"));
        }

        [Command("!spawn-dell-arte")]
        [Summary("Spawning DellArte buttons")]
        public async Task DellArte()
        {
            await Response(ButtonIdType.DellArte, ButtonIdType.NotDellArte, new Emoji("🎭"));
        }

        [Command("!spawn-zetsu")]
        [Summary("Spawning Zetsu buttons")]
        public async Task Zetsu()
        {
            await Response(ButtonIdType.Zetsu, ButtonIdType.NotZetsu, new Emoji("🃏"));
        }

        [Command("!spawn-mythical")]
        [Summary("Spawning Mythical buttons")]
        public async Task Mythical()
        {
            await Response(ButtonIdType.Mythical, ButtonIdType.NotMythical, new Emoji("❤️‍🔥"));
        }

        [Command("!spawn-musical")]
        [Summary("Spawning Musical buttons")]
        public async Task Musical()
        {
            await Response(ButtonIdType.Musical, ButtonIdType.NotMusical, new Emoji("🎧"));
        }
    }
}
