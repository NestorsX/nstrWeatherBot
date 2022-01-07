namespace nstrWeatherBot
{
    public class Program
    {
        static void Main(string[] args)
        {
            var bot = new Bot("Paste your telegram-bot API key");
            bot.Start();
        }
    }
}
