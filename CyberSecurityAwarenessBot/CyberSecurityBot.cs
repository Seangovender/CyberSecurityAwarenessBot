using System;
using System.IO;
using System.Media;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;

namespace CyberSecurityAwarenessBot
{
    public class CyberSecurityBot
    {
        private string userName = string.Empty;
        private string lastTopic = string.Empty;
        private string favoriteTopic = string.Empty;
        private bool isRunning = true;

        // Dictionary of topics with lists of random tips
        private readonly Dictionary<string, List<string>> topicTips = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
        {
            ["phishing"] = new List<string>
            {
                "🎣 Always check the sender's email address – scammers use slight misspellings.",
                "🎣 Never click links in unexpected emails. Type the website address yourself.",
                "🎣 If an email creates urgency ('Your account will be closed'), it's probably a scam."
            },
            ["password"] = new List<string>
            {
                "🔐 Use a different password for every account – don't reuse!",
                "🔐 A strong password has 12+ characters, uppercase, numbers, and symbols.",
                "🔐 Enable two‑factor authentication (2FA) wherever possible."
            },
            ["safe browsing"] = new List<string>
            {
                "🌐 Look for 'https://' and a padlock icon before entering personal info.",
                "🌐 Avoid downloading files from untrusted websites.",
                "🌐 Keep your browser and antivirus software updated automatically."
            },
            ["social engineering"] = new List<string>
            {
                "🧠 Never share OTPs or PINs – even if the caller sounds official.",
                "🧠 Hang up and call the organisation back using a known number.",
                "🧠 Be suspicious of anyone asking for urgent payments or gift cards."
            },
            ["scam"] = new List<string>
            {
                "⚠️ Ignore prizes you never entered – they're always traps.",
                "⚠️ If someone asks for money via social media, verify with a phone call.",
                "⚠️ Report scam messages to your bank or the real company."
            },
            ["privacy"] = new List<string>
            {
                "🛡️ Review your social media privacy settings every few months.",
                "🛡️ Don't share your location or daily routines online.",
                "🛡️ Use a VPN when on public Wi‑Fi (coffee shops, airports)."
            }
        };

        public void Run()
        {
            SetupConsole();
            DisplayAsciiArt();
            DisplayWelcomeMessage();
            PlayVoiceGreeting();
            AskUserName();
            StartChatLoop();
            DisplayGoodbyeMessage();
        }

        private void SetupConsole()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();
        }

        private void PlayVoiceGreeting()
        {
            try
            {
                if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    return;

                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "welcome.wav");
                if (File.Exists(path))
                {
                    using (SoundPlayer player = new SoundPlayer(path))
                    {
                        player.Load();
                        player.Play(); 
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("[Audio file not found, continuing without sound]");
                    Console.ResetColor();
                }
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[Could not play audio]");
                Console.ResetColor();
            }
        }

        private void DisplayAsciiArt()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==============================================================");
            Console.WriteLine("   ____      _                              _ _         ");
            Console.WriteLine("  / ___|   _| |__   ___ _ __ ___  ___  ___ | (_)_ __    ");
            Console.WriteLine(" | |  | | | | '_ \\ / _ \\ '__/ __|/ _ \\/ _ \\| | | '_ \\   ");
            Console.WriteLine(" | |__| |_| | |_) |  __/ |  \\__ \\  __/ (_) | | | | | |  ");
            Console.WriteLine("  \\____\\__, |_.__/ \\___|_|  |___/\\___|\\___/|_|_|_| |_|  ");
            Console.WriteLine("       |___/                                            ");
            Console.WriteLine("        CYBERSECURITY AWARENESS ASSISTANT               ");
            Console.WriteLine("==============================================================");
            Console.ResetColor();
            Console.WriteLine();
        }

        private void DisplayWelcomeMessage()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Welcome to CYBERBOT – your personal cybersecurity guide.");
            Console.WriteLine("Type 'exit' or 'quit' to leave at any time.");
            Console.ResetColor();
            Console.WriteLine();
        }

        private void AskUserName()
        {
            while (true)
            {
                Console.Write("What is your name? ");
                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    ShowError("Name cannot be empty.");
                    continue;
                }
                if (input.Trim().Length < 2)
                {
                    ShowError("Name must be at least 2 characters long.");
                    continue;
                }

                userName = input.Trim();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nNice to meet you, {userName}! I can help you with topics like:");
                Console.WriteLine("phishing, passwords, safe browsing, social engineering, scams, or privacy.");
                Console.WriteLine("Just ask me anything – for example: 'Tell me about passwords'");
                Console.ResetColor();
                Console.WriteLine();
                break;
            }
        }

        private void StartChatLoop()
        {
            while (isRunning)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"{userName}> ");
                Console.ResetColor();
                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    ShowError("Please type something – I'm here to help.");
                    continue;
                }

                string lowerInput = input.ToLower().Trim();
                if (lowerInput == "exit" || lowerInput == "quit")
                {
                    isRunning = false;
                    break;
                }

                // 1. Sentiment detection (returns null if none)
                string? sentimentReply = DetectSentiment(lowerInput);
                if (sentimentReply != null)
                {
                    Console.WriteLine($"\n💬 {sentimentReply}");
                    Console.WriteLine();
                    continue;
                }

                // 2. Follow-up / "tell me more"
                if (IsRequestingMore(lowerInput) && !string.IsNullOrEmpty(lastTopic))
                {
                    string moreTip = GetRandomTip(lastTopic);
                    Console.WriteLine($"\n💬 Here's another tip about {lastTopic}: {moreTip}");
                    Console.WriteLine();
                    continue;
                }

                // 3. Keyword matching
                string? matchedTopic = topicTips.Keys.FirstOrDefault(t => lowerInput.Contains(t));
                if (matchedTopic != null)
                {
                    lastTopic = matchedTopic;
                    if (string.IsNullOrEmpty(favoriteTopic))
                    {
                        favoriteTopic = matchedTopic;
                        Console.WriteLine($"\n💬 Great! I'll remember that you're interested in {matchedTopic}.");
                    }
                    string tip = GetRandomTip(matchedTopic);
                    Console.WriteLine($"\n💬 {tip}");
                    Console.WriteLine("\n(You can say 'tell me more' or 'another tip' for more on this topic.)");
                    Console.WriteLine();
                    continue;
                }

                // 4. Default / unknown input
                Console.WriteLine("\n💬 I'm not sure I understand. You can ask about phishing, passwords, safe browsing, social engineering, scams, or privacy.");
                Console.WriteLine();
            }
        }

        private string GetRandomTip(string topic)
        {
            if (topicTips.ContainsKey(topic))
            {
                var tips = topicTips[topic];
                Random rand = new Random();
                return tips[rand.Next(tips.Count)];
            }
            return "Stay alert – and always verify before you trust.";
        }

        private bool IsRequestingMore(string input)
        {
            return input.Contains("more") || input.Contains("another") ||
                   input.Contains("tell me again") || input.Contains("explain more") ||
                   input.Contains("another tip");
        }

        private string? DetectSentiment(string input)
        {
            if (input.Contains("worried") || input.Contains("scared") || input.Contains("nervous"))
            {
                return $"It's completely normal to feel worried, {userName}. Cyber threats are real, but you're taking the right step. {GetRandomTip("phishing")}";
            }
            if (input.Contains("frustrated") || input.Contains("confused"))
            {
                return $"I understand it can be frustrating, {userName}. Let's start simple: {GetRandomTip("password")}";
            }
            if (input.Contains("curious") || input.Contains("interesting"))
            {
                return $"Great curiosity, {userName}! Did you know that 90% of data breaches start with a phishing email? {GetRandomTip("phishing")}";
            }
            return null;
        }

        private void DisplayGoodbyeMessage()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nGoodbye, {userName}! Stay alert, stay informed, and stay safe online.");
            if (!string.IsNullOrEmpty(favoriteTopic))
                Console.WriteLine($"Remember what you learned about {favoriteTopic} – practice it every day.");
            Console.ResetColor();
        }

        private void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
            Console.WriteLine();
        }
    }
}