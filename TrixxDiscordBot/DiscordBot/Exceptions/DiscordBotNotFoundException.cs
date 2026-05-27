namespace DiscordBot.Exceptions
{
    public class DiscordBotNotFoundException : DiscordBotException
    {
        public DiscordBotNotFoundException(string message) : base(message)
        {
        }
    }
}
