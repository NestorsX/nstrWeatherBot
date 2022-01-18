namespace nstrWeatherBot
{
    public class Program
    {
        static void Main(string[] args)
        {
            var bot = new Bot("Enter your telegram bot API here");
            bot.Start();
        }
    }
}
