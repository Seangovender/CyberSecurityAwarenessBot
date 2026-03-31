using System;
using System.IO;
using System.Media;
using System.Runtime.InteropServices;

namespace CyberSecurityAwarenessBot
{
    public class CyberSecurityBot
    {
        private string userName = string.Empty;
        private bool isRunning = true;

        public void Run()
        {
            SetupConsole();
            DisplayAsciiArt();
            DisplayWelcomeMessage();
            PlayVoiceGreeting(); // now plays in background
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
                    SoundPlayer player = new SoundPlayer(path);
                    player.Load();
                    player.Play();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Audio file not found, continuing without sound...");
                    Console.ResetColor();
                }
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Could not play audio.");
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
            Console.WriteLine("Welcome to CYBERBOT.");
            Console.WriteLine("This program will help you stay safe online.");
            Console.ResetColor();
            Console.WriteLine();
        }

        private void AskUserName()
        {
            while (true)
            {
                Console.Write("Enter your name: ");
                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    ShowError("Name cannot be empty.");
                    continue;
                }

                if (input.Trim().Length < 2)
                {
                    ShowError("Name is too short, Name must be at least 2 characters long.");
                    continue;
                }

                userName = input.Trim();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nHi {userName}, welcome!\n");
                Console.ResetColor();

                break;
            }
        }

        private void StartChatLoop()
        {
            while (isRunning)
            {
                DisplayMenu();

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Select an option (1-7): ");
                Console.ResetColor();

                string? choice = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(choice))
                {
                    ShowError("Input cannot be empty. Please choose a number from 1 to 7.");
                    continue;
                }

                switch (choice.Trim())
                {
                    case "1":
                        ExplainPhishing();
                        break;
                    case "2":
                        ExplainPasswords();
                        break;
                    case "3":
                        ExplainSafeBrowsing();
                        break;
                    case "4":
                        ExplainSocialEngineering();
                        break;
                    case "5":
                        ScamSafetyTips();
                        break;
                    case "6":
                        QuickQuiz();
                        break;
                    case "7":
                        isRunning = false;
                        break;
                    default:
                        ShowError("Invalid option. Please select a number from 1 to 7.");
                        break;
                }
            }
        }

        private void DisplayMenu()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine($"Cybersecurity Menu for {userName}");
            Console.WriteLine("--------------------------------------------------------------");
            Console.ResetColor();

            Console.WriteLine("1. Learn about phishing");
            Console.WriteLine("2. Learn about password safety");
            Console.WriteLine("3. Learn about safe browsing");
            Console.WriteLine("4. Learn about social engineering");
            Console.WriteLine("5. View scam safety tips");
            Console.WriteLine("6. Take a quick quiz");
            Console.WriteLine("7. Exit");
            Console.WriteLine();
        }

        private void ExplainPhishing()
        {
            PrintSectionTitle("Phishing Awareness");

            Console.WriteLine("Phishing is when attackers try to trick you into giving away");
            Console.WriteLine("personal information such as passwords, banking details, or OTPs.");
            Console.WriteLine();
            Console.WriteLine("Common warning signs:");
            Console.WriteLine("- Urgent language such as 'Act now' or 'Your account will be blocked'");
            Console.WriteLine("- Suspicious links or email addresses");
            Console.WriteLine("- Requests for passwords or banking details");
            Console.WriteLine();
            Console.WriteLine($"{userName}, always double-check the sender and never click suspicious links.");
            Pause();
        }

        private void ExplainPasswords()
        {
            PrintSectionTitle("Password Safety");

            Console.WriteLine("A strong password should be:");
            Console.WriteLine("- Long");
            Console.WriteLine("- Unique");
            Console.WriteLine("- Hard to guess");
            Console.WriteLine();
            Console.WriteLine("Good practice:");
            Console.WriteLine("- Use uppercase and lowercase letters");
            Console.WriteLine("- Include numbers and special characters");
            Console.WriteLine("- Do not reuse the same password everywhere");
            Console.WriteLine("- Use multi-factor authentication where possible");
            Console.WriteLine();
            Console.WriteLine($"{userName}, a password manager can also help you store secure passwords safely.");
            Pause();
        }

        private void ExplainSafeBrowsing()
        {
            PrintSectionTitle("Safe Browsing");

            Console.WriteLine("Safe browsing means using the internet carefully to avoid scams,");
            Console.WriteLine("malware and fake websites.");
            Console.WriteLine();
            Console.WriteLine("Tips:");
            Console.WriteLine("- Check if the website uses HTTPS");
            Console.WriteLine("- Do not download files from unknown websites");
            Console.WriteLine("- Avoid public Wi-Fi for sensitive transactions");
            Console.WriteLine("- Keep your browser and antivirus updated");
            Console.WriteLine();
            Console.WriteLine($"{userName}, always verify a website before entering personal information.");
            Pause();
        }

        private void ExplainSocialEngineering()
        {
            PrintSectionTitle("Social Engineering");

            Console.WriteLine("Social engineering happens when attackers manipulate people");
            Console.WriteLine("into giving away confidential information.");
            Console.WriteLine();
            Console.WriteLine("Examples include:");
            Console.WriteLine("- Someone pretending to be from your bank");
            Console.WriteLine("- Fake tech support calls");
            Console.WriteLine("- Messages asking for urgent payments or OTP codes");
            Console.WriteLine();
            Console.WriteLine("The best defence is to stay calm, verify identities, and never");
            Console.WriteLine("share private information under pressure.");
            Pause();
        }

        private void ScamSafetyTips()
        {
            PrintSectionTitle("Quick Scam Safety Tips");

            Console.WriteLine("1. Never share your OTP or banking PIN.");
            Console.WriteLine("2. Verify links before clicking on them.");
            Console.WriteLine("3. Ignore prizes you did not enter.");
            Console.WriteLine("4. Be careful with attachments from unknown senders.");
            Console.WriteLine("5. Report suspicious activity immediately.");
            Console.WriteLine();
            Console.WriteLine($"{userName}, being cautious online is one of the best ways to protect yourself.");
            Pause();
        }

        private void QuickQuiz()
        {
            PrintSectionTitle("Quick Quiz");

            Console.WriteLine("Question: Should you share your OTP with someone claiming to be from the bank?");
            Console.Write("Enter yes or no: ");

            string? answer = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(answer))
            {
                ShowError("Quiz answer cannot be empty.");
                return;
            }

            answer = answer.Trim().ToLower();

            if (answer == "no")
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Correct! You should never share your OTP with anyone.");
            }
            else if (answer == "yes")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Incorrect. Banks do not ask for your OTP in that way.");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Please answer with 'yes' or 'no' next time.");
            }

            Console.ResetColor();
            Pause();
        }

        private void DisplayGoodbyeMessage()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine();
            Console.WriteLine($"Goodbye, {userName}! Thank you for using the Cybersecurity Awareness Bot.");
            Console.WriteLine("Stay alert, stay informed, and stay safe online.");
            Console.ResetColor();
        }

        private void PrintSectionTitle(string title)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("==============================================================");
            Console.WriteLine(title);
            Console.WriteLine("==============================================================");
            Console.ResetColor();
        }

        private void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
            Console.WriteLine();
        }

        private void Pause()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\nPress Enter to go back to the menu...");
            Console.ResetColor();

            Console.ReadLine();
            Console.WriteLine();
        }
    }
}