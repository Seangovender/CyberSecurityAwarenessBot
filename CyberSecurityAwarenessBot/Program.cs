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
                CyberSecurityBot chatbot = new CyberSecurityBot();
                chatbot.Run();
            }
            catch (Exception error)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("The program could not start properly.");
                Console.WriteLine("Error: " + error.Message);
                Console.ResetColor();
            }
        }
    }
}