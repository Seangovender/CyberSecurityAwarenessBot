using System;

namespace CyberSecurityAwarenessBot
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "CYBERBOT";

            try
            {
                CyberSecurityBot bot = new CyberSecurityBot();
                bot.Run();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("The program could not start.");
                Console.WriteLine("Error: " + ex.Message);
                Console.ResetColor();
            }
        }
    }
}