using System;
using System.Collections.Generic;

namespace CyberSecurityChatbot_Part2
{
    public class QuizHelper
    {
        private List<QuizQuestion> questions = new List<QuizQuestion>(); 
        private int currentIndex = 0;
        private int score = 0;
        private bool active = false;

        public QuizHelper()
        {
            LoadQuestions();
        }

        private void LoadQuestions()
        {
            questions = new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Question = "What should you do if you receive an email asking for your password?",
                    Options = new List<string> { "A) Reply with your password", "B) Delete the email", "C) Report the email as phishing", "D) Ignore it" },
                    CorrectAnswer = 2,
                    Explanation = "Reporting phishing emails helps prevent scams. Never share your password via email!"
                },
                new QuizQuestion
                {
                    Question = "True or False: Using the same password for all accounts is safe.",
                    Options = new List<string> { "A) True", "B) False" },
                    CorrectAnswer = 1,
                    Explanation = "Using the same password everywhere means one breach compromises all your accounts. Always use unique passwords."
                },
                new QuizQuestion
                {
                    Question = "What is a strong password?",
                    Options = new List<string> { "A) Your birthdate", "B) 'Password123'", "C) A mix of uppercase, lowercase, numbers, and symbols (12+ chars)", "D) Your pet's name" },
                    CorrectAnswer = 2,
                    Explanation = "Strong passwords are long, complex, and unique. Use a password manager to generate them!"
                },
                new QuizQuestion
                {
                    Question = "True or False: HTTPS websites are always safe.",
                    Options = new List<string> { "A) True", "B) False" },
                    CorrectAnswer = 1,
                    Explanation = "HTTPS encrypts your data, but scammers can still have HTTPS certificates. Always verify the full URL!"
                },
                new QuizQuestion
                {
                    Question = "What is social engineering?",
                    Options = new List<string> { "A) A type of software", "B) Manipulating people to reveal confidential information", "C) A programming language", "D) A social media app" },
                    CorrectAnswer = 1,
                    Explanation = "Social engineering is psychological manipulation. Attackers trick you into sharing sensitive information."
                },
                new QuizQuestion
                {
                    Question = "True or False: You should click on links in unexpected emails to check if they're safe.",
                    Options = new List<string> { "A) True", "B) False" },
                    CorrectAnswer = 1,
                    Explanation = "Never click links in unexpected emails. Hover to see the URL, or type the website address yourself."
                },
                new QuizQuestion
                {
                    Question = "What is two-factor authentication (2FA)?",
                    Options = new List<string> { "A) A second password", "B) A security question", "C) An extra layer of security requiring a second verification step", "D) A fingerprint scanner" },
                    CorrectAnswer = 2,
                    Explanation = "2FA adds a second verification step, like a code sent to your phone, making accounts much harder to hack."
                },
                new QuizQuestion
                {
                    Question = "True or False: Public Wi-Fi is completely safe for banking.",
                    Options = new List<string> { "A) True", "B) False" },
                    CorrectAnswer = 1,
                    Explanation = "Public Wi-Fi is unsecured. Use a VPN or your mobile data for sensitive transactions."
                },
                new QuizQuestion
                {
                    Question = "What should you do if you win a prize you never entered?",
                    Options = new List<string> { "A) Accept it", "B) Ignore it", "C) Share your details to claim it", "D) Report it as a scam" },
                    CorrectAnswer = 3,
                    Explanation = "Unexpected prizes are almost always scams. Report and ignore them."
                },
                new QuizQuestion
                {
                    Question = "True or False: You should use the same PIN for your phone and bank card.",
                    Options = new List<string> { "A) True", "B) False" },
                    CorrectAnswer = 1,
                    Explanation = "Using the same PIN for everything means if someone sees it once, they have access to all your accounts. Always use different PINs."
                },
                new QuizQuestion
                {
                    Question = "What is phishing?",
                    Options = new List<string> { "A) A type of fish", "B) A scam that tricks you into giving away personal information", "C) A password manager", "D) An antivirus software" },
                    CorrectAnswer = 1,
                    Explanation = "Phishing is a common cyberattack where scammers pretend to be legitimate organisations to steal your data."
                },
                new QuizQuestion
                {
                    Question = "True or False: You should share your OTP (one-time pin) if someone from the bank calls you.",
                    Options = new List<string> { "A) True", "B) False" },
                    CorrectAnswer = 1,
                    Explanation = "Banks never ask for your OTP. If someone asks, it's a scam – hang up immediately."
                }
            };
        }

        public bool StartQuiz()
        {
            if (questions == null || questions.Count == 0)
                return false;

            currentIndex = 0;
            score = 0;
            active = true;
            return true;
        }

        public QuizQuestion? GetCurrentQuestion()   // ✅ Nullable return
        {
            if (!active || currentIndex >= questions.Count)
                return null;
            return questions[currentIndex];
        }

        public string SubmitAnswer(int selectedOption)
        {
            if (!active || currentIndex >= questions.Count)
                return "The quiz has ended. Say 'start quiz' to play again.";

            var question = questions[currentIndex];
            bool isCorrect = selectedOption == question.CorrectAnswer;

            if (isCorrect)
                score++;

            string result = isCorrect ? "✅ Correct!" : "❌ Incorrect.";
            string explanation = question.Explanation;

            currentIndex++;

            if (currentIndex >= questions.Count)
            {
                active = false;
                string finalMessage = GetFinalFeedback(score, questions.Count);
                return $"{result}\n{explanation}\n\n🏆 Quiz complete!\nFinal score: {score}/{questions.Count}\n{finalMessage}";
            }

            return $"{result}\n{explanation}\n\nNext question: {questions[currentIndex].Question}";
        }

        private string GetFinalFeedback(int score, int total)
        {
            double percentage = (double)score / total * 100;
            if (percentage >= 90)
                return "🌟 Excellent! You're a cybersecurity pro! Keep up the great work!";
            else if (percentage >= 70)
                return "👍 Great job! You have solid cybersecurity knowledge. Keep learning!";
            else if (percentage >= 50)
                return "📚 Good effort! Review the topics you missed and try again!";
            else
                return "💪 Keep learning to stay safe online! Try the quiz again after reading some tips.";
        }

        public int GetScore() => score;
        public int GetTotalQuestions() => questions.Count;
        public bool IsQuizActive() => active;
        public int GetProgress() => currentIndex;
    }

    public class QuizQuestion
    {
        public string Question { get; set; } = string.Empty;
        public List<string> Options { get; set; } = new List<string>();
        public int CorrectAnswer { get; set; } // 0-based index
        public string Explanation { get; set; } = string.Empty;
    }
}