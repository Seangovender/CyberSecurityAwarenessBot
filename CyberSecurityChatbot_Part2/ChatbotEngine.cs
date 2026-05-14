 // Cybersecurity Awareness Chatbot - Part 2 (WPF)
 // Module: PROG6221
 // ST10491364
 // Description: Chatbot engine with keyword recognition, random responses,
 //              memory, sentiment detection, and follow-up handling.
 

using System;
using System.Collections.Generic;
using System.Linq;

namespace CyberSecurityChatbot_Part2
{
    public class ChatbotEngine
    {
        // Public properties for memory (name and favourite topic)
        public string UserName { get; private set; } = string.Empty;
        public string FavoriteTopic { get; private set; } = string.Empty;

        // Private fields for conversation flow
        private string lastTopic = string.Empty;
        private bool waitingForName = true;

        // Dictionary mapping topics to lists of random tips
        private readonly Dictionary<string, List<string>> topicTips;

        public ChatbotEngine()
        {
            // Initialise topics and their respective tip lists
            topicTips = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
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
        }

       
        // Processes user input and returns the chatbot's response.
        // Handles name capture, sentiment, follow-up, keyword matching, and default messages.
        
        public string ProcessInput(string userInput)
        {
            if (waitingForName)
            {
                if (string.IsNullOrWhiteSpace(userInput))
                    return "Please tell me your name.";

                UserName = userInput.Trim();
                waitingForName = false;
                return $"Nice to meet you, {UserName}! I can help you with topics like:\n" +
                       "phishing, passwords, safe browsing, social engineering, scams, or privacy.\n" +
                       "Just ask me anything – for example: 'Tell me about passwords'";
            }

            string lower = userInput.ToLower().Trim();

            // Sentiment detection (returns null if none)
            string? sentiment = DetectSentiment(lower);
            if (sentiment != null)
                return sentiment;

            // Follow-up / "tell me more"
            if (IsRequestingMore(lower) && !string.IsNullOrEmpty(lastTopic))
            {
                return $"Here's another tip about {lastTopic}: {GetRandomTip(lastTopic)}";
            }

            // Keyword matching
            string? matchedTopic = topicTips.Keys.FirstOrDefault(t => lower.Contains(t));
            if (matchedTopic != null)
            {
                lastTopic = matchedTopic;
                if (string.IsNullOrEmpty(FavoriteTopic))
                    FavoriteTopic = matchedTopic;

                string tip = GetRandomTip(matchedTopic);
                return $"{tip}\n\n(You can say 'tell me more' or 'another tip' for more on this topic.)";
            }

            // Default / unknown input
            return "I'm not sure I understand. You can ask about phishing, passwords, safe browsing, social engineering, scams, or privacy.";
        }

        
        // Returns a random tip from the list for the given topic.
        private string GetRandomTip(string topic)
        {
            if (topicTips.TryGetValue(topic, out List<string>? tips) && tips.Count > 0)
            {
                Random rand = new Random();
                return tips[rand.Next(tips.Count)];
            }
            return "Stay alert – and always verify before you trust.";
        }

        
        // Detects if the user is asking for more information on the current topic.
       
        private bool IsRequestingMore(string input)
        {
            return input.Contains("more") || input.Contains("another") ||
                   input.Contains("tell me again") || input.Contains("explain more") ||
                   input.Contains("another tip");
        }

       
        // Detects sentiment keywords (worried, frustrated, curious) and returns an empathetic response.
        // Returns null if no sentiment is detected.
        
        private string? DetectSentiment(string input)
        {
            if (input.Contains("worried") || input.Contains("scared") || input.Contains("nervous"))
            {
                return $"It's completely normal to feel worried, {UserName}. Cyber threats are real, but you're taking the right step. {GetRandomTip("phishing")}";
            }
            if (input.Contains("frustrated") || input.Contains("confused"))
            {
                return $"I understand it can be frustrating, {UserName}. Let's start simple: {GetRandomTip("password")}";
            }
            if (input.Contains("curious") || input.Contains("interesting"))
            {
                return $"Great curiosity, {UserName}! Did you know that 90% of data breaches start with a phishing email? {GetRandomTip("phishing")}";
            }
            return null;
        }
    }
}

//GeeksforGeeks. (2022). C# – KeyDown event in WPF.
//Available at: https://www.geeksforgeeks.org/wpf-keydown-event-in-c-sharp/ (Accessed:13 May 2026)