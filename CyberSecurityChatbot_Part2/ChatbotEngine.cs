// Cybersecurity Awareness Chatbot - Part 2 (WPF) + Part 3
// Module: PROG6221
// ST10491364
// Date: 22 June 2026

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;

namespace CyberSecurityChatbot_Part2
{
    public class ChatbotEngine
    {
        // Properties 
        public string UserName { get; private set; } = string.Empty;
        public string FavoriteTopic { get; private set; } = string.Empty;
        private string lastTopic = string.Empty;
        private bool waitingForName = true;

        // Topic tips dictionary 
        private readonly Dictionary<string, List<string>> topicTips;

        //  Activity log 
        private readonly List<string> activityLog = new List<string>();

         
        private readonly DatabaseHelper db = new DatabaseHelper();
        private readonly QuizHelper quiz = new QuizHelper();

        public ChatbotEngine()
        {
            topicTips = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
            {
                ["phishing"] = new List<string>
                {
                    "🎣 Always check the sender's email address – scammers use slight misspellings.",
                    "🎣 Never click links in unexpected emails. Type the website address yourself.",
                    "🎣 If an email creates urgency ('Your account will be closed'), it's probably a scam.",
                    "🎣 Hover over links before clicking to see the real URL – scammers hide fake links.",
                    "🎣 Report phishing emails to the real company – they use reports to block scammers.",
                    "🎣 Legitimate banks never ask for your password via email – stay alert!"
                },
                ["password"] = new List<string>
                {
                    "🔐 Use a different password for every account – don't reuse!",
                    "🔐 A strong password has 12+ characters, uppercase, numbers, and symbols.",
                    "🔐 Enable two‑factor authentication (2FA) wherever possible.",
                    "🔐 Use a password manager to generate and store complex passwords.",
                    "🔐 Change passwords immediately if you suspect a breach.",
                    "🔐 Avoid using personal info like birthdates or pet names in passwords."
                },
                ["safe browsing"] = new List<string>
                {
                    "🌐 Look for 'https://' and a padlock icon before entering personal info.",
                    "🌐 Avoid downloading files from untrusted websites.",
                    "🌐 Keep your browser and antivirus software updated automatically.",
                    "🌐 Use private/incognito mode when using public computers.",
                    "🌐 Clear cookies and cache regularly to protect your privacy.",
                    "🌐 Be cautious of pop-ups – never click 'Allow' on suspicious requests."
                },
                ["social engineering"] = new List<string>
                {
                    "🧠 Never share OTPs or PINs – even if the caller sounds official.",
                    "🧠 Hang up and call the organisation back using a known number.",
                    "🧠 Be suspicious of anyone asking for urgent payments or gift cards.",
                    "🧠 Scammers create fake urgency – always take a moment to verify.",
                    "🧠 Trust your instincts – if it feels off, it probably is.",
                    "🧠 Verify the identity of anyone asking for sensitive information."
                },
                ["scam"] = new List<string>
                {
                    "⚠️ Ignore prizes you never entered – they're always traps.",
                    "⚠️ If someone asks for money via social media, verify with a phone call.",
                    "⚠️ Report scam messages to your bank or the real company.",
                    "⚠️ Scammers use fake invoices – always verify payment requests.",
                    "⚠️ Never send money to someone you've only met online.",
                    "⚠️ Check for spelling errors in emails – scammers often make mistakes."
                },
                ["privacy"] = new List<string>
                {
                    "🛡️ Review your social media privacy settings every few months.",
                    "🛡️ Don't share your location or daily routines online.",
                    "🛡️ Use a VPN when on public Wi‑Fi (coffee shops, airports).",
                    "🛡️ Limit what apps can access – check permissions regularly.",
                    "🛡️ Use encrypted messaging apps for sensitive conversations.",
                    "🛡️ Turn off 'Show Password' visibility on websites."
                }
            };
        }

        // Main processing method 
        public string ProcessInput(string userInput)
        {
            if (waitingForName)
            {
                if (string.IsNullOrWhiteSpace(userInput))
                    return "Please tell me your name.";

                UserName = userInput.Trim();
                waitingForName = false;
                AddToLog($"User registered: {UserName}");
                return $"Nice to meet you, {UserName}! I can help you with topics like:\n" +
                       "phishing, passwords, safe browsing, social engineering, scams, or privacy.\n" +
                       "Just ask me anything – for example: 'Tell me about passwords'";
            }

            string lower = userInput.ToLower().Trim();

            // Memory recall
            if (lower.Contains("what is my name") || lower.Contains("what's my name") || lower.Contains("do you remember me"))
                return $"Your name is {UserName}. You asked me to remember that!";

            if (lower.Contains("what is my favourite topic") || lower.Contains("what's my favourite topic") ||
                lower.Contains("what do i like") || lower.Contains("what do you remember about me"))
            {
                if (!string.IsNullOrEmpty(FavoriteTopic))
                    return $"I remember you're interested in {FavoriteTopic}. That's a great topic to learn about!";
                else
                    return "You haven't told me your favourite topic yet. Try saying 'I'm interested in [topic]'.";
            }

            // Greeting
            if (lower.Contains("hello") || lower.Contains("hi") || lower.Contains("hey") ||
                lower.Contains("good morning") || lower.Contains("good afternoon") || lower.Contains("good evening"))
                return $"Hello {UserName}! How can I help you with cybersecurity today?";

            // Part 3: Task, Quiz, Activity Log commands 
            // Task commands
            if (lower.Contains("add task") || lower.Contains("create task") || lower.Contains("new task") ||
                lower.Contains("remind me to") ||
                lower.Contains("list tasks") || lower.Contains("show tasks") || lower.Contains("view tasks") ||
                lower.Contains("complete task") || lower.Contains("delete task"))
            {
                string? taskResponse = ProcessTaskCommand(userInput);
                if (taskResponse != null)
                    return taskResponse;
            }

            // Quiz commands – trigger if quiz is active OR user types start/take/play/quiz
            if (lower.Contains("start quiz") || lower.Contains("take quiz") || lower.Contains("play quiz") ||
                lower.Contains("quiz") || quiz.IsQuizActive())
            {
                string? quizResponse = ProcessQuizCommand(userInput);
                if (quizResponse != null)
                    return quizResponse;
            }

            // Activity log
            if (lower.Contains("show activity log") || lower.Contains("what have you done") ||
                lower.Contains("show log") || lower.Contains("activity log") || lower.Contains("recent actions"))
                return GetActivityLogSummary();

            if (lower.Contains("show full log") || lower.Contains("full activity log") || lower.Contains("complete log"))
                return GetFullActivityLog();

            // Sentiment
            string? sentiment = DetectSentiment(lower);
            if (sentiment != null)
                return sentiment;

            // Follow‑up
            if (IsRequestingMore(lower) && !string.IsNullOrEmpty(lastTopic))
            {
                string tip = GetRandomTip(lastTopic);
                AddToLog($"Follow-up: another tip on {lastTopic}");
                return $"Here's another tip about {lastTopic}: {tip}";
            }

            // Keyword matching
            string? matchedTopic = topicTips.Keys.FirstOrDefault(t => lower.Contains(t));
            if (matchedTopic != null)
            {
                lastTopic = matchedTopic;
                if (string.IsNullOrEmpty(FavoriteTopic))
                {
                    FavoriteTopic = matchedTopic;
                    AddToLog($"Favourite topic set: {matchedTopic}");
                }
                string tip = GetRandomTip(matchedTopic);
                AddToLog($"Keyword detected: {matchedTopic}");
                return $"{tip}\n\n(You can say 'tell me more' or 'another tip' for more on this topic.)";
            }

            return "I'm not sure I understand. You can ask about phishing, passwords, safe browsing, social engineering, scams, or privacy. Or say 'hello' to greet me!";
        }

        // Helper methods
        private string GetRandomTip(string topic)
        {
            if (topicTips.TryGetValue(topic, out List<string>? tips) && tips.Count > 0)
            {
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
                AddToLog("Sentiment detected: worried");
                return $"It's completely normal to feel worried, {UserName}. Cyber threats are real, but you're taking the right step. {GetRandomTip("phishing")}";
            }
            if (input.Contains("frustrated") || input.Contains("confused"))
            {
                AddToLog("Sentiment detected: frustrated");
                return $"I understand it can be frustrating, {UserName}. Let's start simple: {GetRandomTip("password")}";
            }
            if (input.Contains("curious") || input.Contains("interesting"))
            {
                AddToLog("Sentiment detected: curious");
                return $"Great curiosity, {UserName}! Did you know that 90% of data breaches start with a phishing email? {GetRandomTip("phishing")}";
            }
            return null;
        }

        //  PART 3:Task Assistant 
        private string? ProcessTaskCommand(string input)
        {
            string lower = input.ToLower().Trim();

            // Add task
            if (lower.Contains("add task") || lower.Contains("create task") || lower.Contains("new task"))
            {
                string taskText = input.Substring(input.IndexOf("task") + 4).Trim();
                if (string.IsNullOrEmpty(taskText))
                    return "Please specify what task you want to add. Example: 'Add task - Review my passwords'";

                bool success = db.AddTask(taskText, $"User-created task: {taskText}", null);
                if (success)
                {
                    AddToLog($"Task added: {taskText}");
                    return $"✅ Task added: '{taskText}'\nWould you like to set a reminder? (say 'remind me in X days')";
                }
                return "❌ Could not add task. Please check your database connection.";
            }

            // Reminder
            if (lower.Contains("remind me to"))
            {
                string taskText = input.Substring(input.IndexOf("to") + 2).Trim();
                if (string.IsNullOrEmpty(taskText))
                    return "Please specify what you want to be reminded about.";

                string title = taskText;
                string description = taskText;
                DateTime? reminderDate = null;

                // Parse date phrases
                if (taskText.Contains("tomorrow"))
                {
                    reminderDate = DateTime.Now.AddDays(1);
                    title = taskText.Replace("tomorrow", "").Trim();
                }
                else if (taskText.Contains("in ") && taskText.Contains(" day"))
                {
                    int days = 0;
                    string[] parts = taskText.Split(' ');
                    for (int i = 0; i < parts.Length; i++)
                    {
                        if (parts[i] == "in" && i + 1 < parts.Length && int.TryParse(parts[i + 1], out days))
                        {
                            reminderDate = DateTime.Now.AddDays(days);
                            title = taskText.Replace($"in {days} days", "").Replace($"in {days} day", "").Trim();
                            break;
                        }
                    }
                }
                else if (taskText.Contains("in ") && taskText.Contains(" week"))
                {
                    int weeks = 0;
                    string[] parts = taskText.Split(' ');
                    for (int i = 0; i < parts.Length; i++)
                    {
                        if (parts[i] == "in" && i + 1 < parts.Length && int.TryParse(parts[i + 1], out weeks))
                        {
                            reminderDate = DateTime.Now.AddDays(weeks * 7);
                            title = taskText.Replace($"in {weeks} weeks", "").Replace($"in {weeks} week", "").Trim();
                            break;
                        }
                    }
                }
                else if (taskText.Contains("in ") && taskText.Contains(" month"))
                {
                    int months = 0;
                    string[] parts = taskText.Split(' ');
                    for (int i = 0; i < parts.Length; i++)
                    {
                        if (parts[i] == "in" && i + 1 < parts.Length && int.TryParse(parts[i + 1], out months))
                        {
                            reminderDate = DateTime.Now.AddMonths(months);
                            title = taskText.Replace($"in {months} months", "").Replace($"in {months} month", "").Trim();
                            break;
                        }
                    }
                }

                if (reminderDate == null)
                    reminderDate = DateTime.Now.AddDays(7);

                bool success = db.AddTask(title, description, reminderDate);
                if (success)
                {
                    string dateStr = reminderDate.Value.ToString("dd MMM yyyy");
                    AddToLog($"Task added with reminder: {title} for {dateStr}");
                    return $"✅ Task added: '{title}'\n📅 Reminder set for {dateStr}.\nI'll remind you then!";
                }
                return "❌ Could not add task with reminder.";
            }

            // List tasks
            if (lower.Contains("list tasks") || lower.Contains("show tasks") || lower.Contains("view tasks") || lower.Contains("my tasks"))
            {
                var tasks = db.GetTasks();
                if (tasks.Count == 0)
                    return "📋 You have no active tasks. Create one by saying 'Add task - [your task]'";

                string response = "📋 Your tasks:\n";
                int count = 1;
                foreach (var task in tasks)
                {
                    string status = task.IsCompleted ? "✅" : "⏳";
                    string reminder = task.ReminderDate.HasValue ? $" 🔔 {task.ReminderDate.Value.ToString("dd MMM yyyy")}" : "";
                    response += $"{count}. {status} {task.Title}{reminder}\n";
                    count++;
                }
                return response + "\nSay 'complete task [number]' to mark as done, or 'delete task [number]' to remove it.";
            }

            // Complete task
            if (lower.Contains("complete task"))
            {
                string[] parts = input.Split(' ');
                if (parts.Length >= 3 && int.TryParse(parts[2], out int taskId))
                {
                    var tasks = db.GetTasks();
                    if (taskId <= tasks.Count && taskId > 0)
                    {
                        int id = tasks[taskId - 1].Id;
                        if (db.MarkTaskComplete(id))
                        {
                            AddToLog($"Task {taskId} marked as complete");
                            return $"✅ Task #{taskId} marked as complete! Great job!";
                        }
                    }
                }
                return "❌ Could not complete task. Please check the number. Example: 'complete task 1'";
            }

            // Delete task
            if (lower.Contains("delete task"))
            {
                string[] parts = input.Split(' ');
                if (parts.Length >= 3 && int.TryParse(parts[2], out int taskId))
                {
                    var tasks = db.GetTasks();
                    if (taskId <= tasks.Count && taskId > 0)
                    {
                        int id = tasks[taskId - 1].Id;
                        if (db.DeleteTask(id))
                        {
                            AddToLog($"Task {taskId} deleted");
                            return $"🗑️ Task #{taskId} deleted successfully.";
                        }
                    }
                }
                return "❌ Could not delete task. Example: 'delete task 1'";
            }

            return null;
        }

        // PART 3: Quiz
        private string? ProcessQuizCommand(string input)
        {
            string lower = input.ToLower().Trim();

            // Start quiz
            if (lower.Contains("start quiz") || lower.Contains("take quiz") || lower.Contains("play quiz") || lower.Contains("quiz"))
            {
                if (quiz.StartQuiz())
                {
                    var question = quiz.GetCurrentQuestion();
                    if (question == null)
                        return "❌ Quiz could not start.";
                    AddToLog("Quiz started");
                    return $"🎮 Let's test your cybersecurity knowledge!\n\n{question.Question}\n" +
                           string.Join("\n", question.Options) +
                           "\n\nType the letter (A/B/C/D) or number (0/1/2/3) of your answer.\n" +
                           "You can also type the full answer text.";
                }
                return "❌ Quiz could not start.";
            }

            // Quiz answer (only when quiz is active)
            if (quiz.IsQuizActive())
            {
                int selectedIndex = -1;

                // 1. Letter answers
                if (lower == "a" || lower == "0") selectedIndex = 0;
                else if (lower == "b" || lower == "1") selectedIndex = 1;
                else if (lower == "c" || lower == "2") selectedIndex = 2;
                else if (lower == "d" || lower == "3") selectedIndex = 3;
                // 2. True / False
                else if (lower == "true" || lower == "t") selectedIndex = 0;
                else if (lower == "false" || lower == "f") selectedIndex = 1;
                // 3. Number input
                else if (int.TryParse(lower, out int num) && num >= 0 && num <= 3)
                    selectedIndex = num;
                // 4. ✨ IMPROVED: Full‑sentence answer matching (more flexible)
                else
                {
                    var currentQuestion = quiz.GetCurrentQuestion();
                    if (currentQuestion != null)
                    {
                        for (int i = 0; i < currentQuestion.Options.Count; i++)
                        {
                            string optionText = currentQuestion.Options[i];
                            string cleanOption = optionText;

                            // Remove the letter prefix like "A) " or "A. " to get clean answer text
                            int idx = optionText.IndexOfAny(new char[] { ')', '.' });
                            if (idx >= 0 && idx + 2 <= optionText.Length)
                            {
                                cleanOption = optionText.Substring(idx + 2).Trim();
                            }

                            // Check for exact match or keyword match
                            bool matchFound = false;

                            // 4a. Exact match (case-insensitive)
                            if (lower.Contains(cleanOption.ToLower()))
                                matchFound = true;

                            // 4b. If no exact match, try keyword matching
                            if (!matchFound)
                            {
                                // Split clean option into keywords
                                string[] keywords = cleanOption.Split(new char[] { ' ', ',', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
                                int matchCount = 0;
                                foreach (string keyword in keywords)
                                {
                                    if (keyword.Length > 2 && lower.Contains(keyword.ToLower()))
                                        matchCount++;
                                }
                                // If more than 50% of keywords match, consider it a match
                                if (keywords.Length > 0 && (double)matchCount / keywords.Length >= 0.5)
                                    matchFound = true;
                            }

                            // 4c. Special handling for "12+" vs "12" etc.
                            if (!matchFound && lower.Contains("12") && cleanOption.Contains("12"))
                                matchFound = true;

                            // 4d. Special handling for "uppercase", "lowercase", "numbers", "symbols"
                            if (!matchFound)
                            {
                                string[] keyTerms = { "uppercase", "lowercase", "numbers", "symbols", "special", "characters", "letters" };
                                int termMatchCount = 0;
                                foreach (string term in keyTerms)
                                {
                                    if (lower.Contains(term) && cleanOption.ToLower().Contains(term))
                                        termMatchCount++;
                                }
                                if (termMatchCount >= 2)
                                    matchFound = true;
                            }

                            if (matchFound)
                            {
                                selectedIndex = i;
                                break;
                            }
                        }

                        // 4e. If still no match, try matching first few words of the option
                        if (selectedIndex == -1)
                        {
                            for (int i = 0; i < currentQuestion.Options.Count; i++)
                            {
                                string optionText = currentQuestion.Options[i];
                                string cleanOption = optionText;
                                int idx = optionText.IndexOfAny(new char[] { ')', '.' });
                                if (idx >= 0 && idx + 2 <= optionText.Length)
                                    cleanOption = optionText.Substring(idx + 2).Trim();

                                // Get first 3 words of the clean option
                                string[] words = cleanOption.Split(' ');
                                if (words.Length >= 3)
                                {
                                    string firstFewWords = string.Join(" ", words.Take(3));
                                    if (lower.Contains(firstFewWords.ToLower()))
                                    {
                                        selectedIndex = i;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

                if (selectedIndex >= 0)
                {
                    string response = quiz.SubmitAnswer(selectedIndex);
                    AddToLog($"Quiz answer submitted: {selectedIndex}");
                    if (response.Contains("🏆"))
                        AddToLog($"Quiz completed. Score: {quiz.GetScore()}/{quiz.GetTotalQuestions()}");
                    return response;
                }
                else
                {
                    // Show the options again if user is confused
                    var currentQuestion = quiz.GetCurrentQuestion();
                    if (currentQuestion != null)
                    {
                        return $"I didn't recognise that answer. Please choose from:\n" +
                               string.Join("\n", currentQuestion.Options) +
                               "\n\nType the letter (A/B/C/D) or type the full answer text.";
                    }
                    return "Please answer with A, B, C, D (or 0, 1, 2, 3) or type the full answer text.";
                }
            }

            return null; // Not a quiz command
        }

        // ---- PART 3: Activity Log ----
        private void AddToLog(string action)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            activityLog.Add($"[{timestamp}] {action}");
            if (activityLog.Count > 100)
                activityLog.RemoveAt(0);
        }

        public List<string> GetActivityLog()
        {
            return new List<string>(activityLog);
        }

        public string GetActivityLogSummary()
        {
            var log = GetActivityLog();
            if (log.Count == 0)
                return "📋 No recent activity. Start chatting, add tasks, or take the quiz!";

            int showCount = Math.Min(10, log.Count);
            var recent = log.Skip(log.Count - showCount).ToList();

            string response = "📋 Here's a summary of recent actions:\n\n";
            int count = 1;
            foreach (var entry in recent)
            {
                response += $"{count}. {entry}\n";
                count++;
            }

            if (log.Count > 10)
                response += $"\n(Showing last 10 of {log.Count} actions. Say 'show full log' for all.)";

            return response;
        }

        public string GetFullActivityLog()
        {
            var log = GetActivityLog();
            if (log.Count == 0)
                return "📋 No activity logged yet.";

            string response = "📋 Complete Activity Log:\n\n";
            int count = 1;
            foreach (var entry in log)
            {
                response += $"{count}. {entry}\n";
                count++;
            }
            return response;
        }
    }
}

//Reference list:
// Cybersecurity & Infrastructure Security Agency (CISA). 2024. Phishing Tips.
// Available at: https://www.cisa.gov/phishing (Accessed: 22 June 2026).

//National Cyber Security Centre (NCSC). 2025. Password Guidance.
//Available at: https://www.ncsc.gov.uk/guidance/password-guidance (Accessed: 22 June 2026).

//South African Banking Risk Information Centre (SABRIC). 2025. Cybersecurity Awareness for South Africans.
//Available at: https://www.sabric.co.za (Accessed: 22 June 2026).