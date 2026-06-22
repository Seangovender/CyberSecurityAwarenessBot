/* Cybersecurity Quiz for Part 3 (POE)
 * Module: PROG6221
 * Student Name: Sean Govender
 * Student Number: ST10491364
 * Date: 22 June 2026
 * Description: Manages the cybersecurity quiz with 12 questions covering
 *              phishing, passwords, social engineering, 2FA, and more.
 *              Tracks score, provides feedback, and displays final results.
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace CyberSecurityChatbot_Part2
{
  
    // Manages the cybersecurity quiz functionality.
    
    public class QuizHelper
    {
        // Private Fields
        private List<QuizQuestion> questions = new List<QuizQuestion>();
        private int currentIndex = 0;
        private int score = 0;
        private bool active = false;

        
        // Initialises the QuizHelper and loads all quiz questions.
        
        public QuizHelper()
        {
            LoadQuestions();
        }

        // Private Methods
        
        // Loads all 12 cybersecurity quiz questions.
        
        private void LoadQuestions()
        {
            questions = new List<QuizQuestion>
            {
                // Question 1: Phishing
                new QuizQuestion
                {
                    Question = "What should you do if you receive an email asking for your password?",
                    Options = new List<string> { "A) Reply with your password", "B) Delete the email", "C) Report the email as phishing", "D) Ignore it" },
                    CorrectAnswer = 2,
                    Explanation = "Reporting phishing emails helps prevent scams. Never share your password via email!"
                },
                // Question 2: Password safety (True/False)
                new QuizQuestion
                {
                    Question = "True or False: Using the same password for all accounts is safe.",
                    Options = new List<string> { "A) True", "B) False" },
                    CorrectAnswer = 1,
                    Explanation = "Using the same password everywhere means one breach compromises all your accounts. Always use unique passwords."
                },
                // Question 3: Strong passwords
                new QuizQuestion
                {
                    Question = "What is a strong password?",
                    Options = new List<string> { "A) Your birthdate", "B) 'Password123'", "C) A mix of uppercase, lowercase, numbers, and symbols (12+ chars)", "D) Your pet's name" },
                    CorrectAnswer = 2,
                    Explanation = "Strong passwords are long, complex, and unique. Use a password manager to generate them!"
                },
                // Question 4: HTTPS safety (True/False)
                new QuizQuestion
                {
                    Question = "True or False: HTTPS websites are always safe.",
                    Options = new List<string> { "A) True", "B) False" },
                    CorrectAnswer = 1,
                    Explanation = "HTTPS encrypts your data, but scammers can still have HTTPS certificates. Always verify the full URL!"
                },
                // Question 5: Social engineering
                new QuizQuestion
                {
                    Question = "What is social engineering?",
                    Options = new List<string> { "A) A type of software", "B) Manipulating people to reveal confidential information", "C) A programming language", "D) A social media app" },
                    CorrectAnswer = 1,
                    Explanation = "Social engineering is psychological manipulation. Attackers trick you into sharing sensitive information."
                },
                // Question 6: Clicking links (True/False)
                new QuizQuestion
                {
                    Question = "True or False: You should click on links in unexpected emails to check if they're safe.",
                    Options = new List<string> { "A) True", "B) False" },
                    CorrectAnswer = 1,
                    Explanation = "Never click links in unexpected emails. Hover to see the URL, or type the website address yourself."
                },
                // Question 7: Two-Factor Authentication
                new QuizQuestion
                {
                    Question = "What is two-factor authentication (2FA)?",
                    Options = new List<string> { "A) A second password", "B) A security question", "C) An extra layer of security requiring a second verification step", "D) A fingerprint scanner" },
                    CorrectAnswer = 2,
                    Explanation = "2FA adds a second verification step, like a code sent to your phone, making accounts much harder to hack."
                },
                // Question 8: Public Wi-Fi (True/False)
                new QuizQuestion
                {
                    Question = "True or False: Public Wi-Fi is completely safe for banking.",
                    Options = new List<string> { "A) True", "B) False" },
                    CorrectAnswer = 1,
                    Explanation = "Public Wi-Fi is unsecured. Use a VPN or your mobile data for sensitive transactions."
                },
                // Question 9: Unexpected prizes
                new QuizQuestion
                {
                    Question = "What should you do if you win a prize you never entered?",
                    Options = new List<string> { "A) Accept it", "B) Ignore it", "C) Share your details to claim it", "D) Report it as a scam" },
                    CorrectAnswer = 3,
                    Explanation = "Unexpected prizes are almost always scams. Report and ignore them."
                },
                // Question 10: Same PIN (True/False)
                new QuizQuestion
                {
                    Question = "True or False: You should use the same PIN for your phone and bank card.",
                    Options = new List<string> { "A) True", "B) False" },
                    CorrectAnswer = 1,
                    Explanation = "Using the same PIN for everything means if someone sees it once, they have access to all your accounts. Always use different PINs."
                },
                // Question 11: Phishing definition
                new QuizQuestion
                {
                    Question = "What is phishing?",
                    Options = new List<string> { "A) A type of fish", "B) A scam that tricks you into giving away personal information", "C) A password manager", "D) An antivirus software" },
                    CorrectAnswer = 1,
                    Explanation = "Phishing is a common cyberattack where scammers pretend to be legitimate organisations to steal your data."
                },
                // Question 12: OTP sharing (True/False)
                new QuizQuestion
                {
                    Question = "True or False: You should share your OTP (one-time pin) if someone from the bank calls you.",
                    Options = new List<string> { "A) True", "B) False" },
                    CorrectAnswer = 1,
                    Explanation = "Banks never ask for your OTP. If someone asks, it's a scam – hang up immediately."
                }
            };
        }

        // Public Methods 

        
        public bool StartQuiz()
        {
            if (questions == null || questions.Count == 0)
                return false;

            currentIndex = 0;
            score = 0;
            active = true;
            return true;
        }

        
        // It gets the current question being displayed.
        
        public QuizQuestion? GetCurrentQuestion()
        {
            if (!active || currentIndex >= questions.Count)
                return null;
            return questions[currentIndex];
        }

       
        // this checks correctness, updates score, and provides feedback.
        
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

        
        // It Generates encouraging feedback based on the user's final score.
        
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

        // Properties 

        
        // Gets the current score (number of correct answers).
        
        public int GetScore() => score;

       
        public int GetTotalQuestions() => questions.Count;

       
        // Checks if the quiz is currently active.
        
        public bool IsQuizActive() => active;

        
        public int GetProgress() => currentIndex;
    }

    
    // Represents a single quiz question.
    // Contains the question text, a list of options,
    
    public class QuizQuestion
    {
        public string Question { get; set; } = string.Empty;
        public List<string> Options { get; set; } = new List<string>();
        public int CorrectAnswer { get; set; } // 0-based index
        public string Explanation { get; set; } = string.Empty;
    }
}

// reference list 

// Stack Overflow. (2011). Random element from List<string> in C#.
//Available at: https://stackoverflow.com/questions/2019417/ (Accessed: 22 June 2026)

//GeeksforGeeks. (2022).List < T > Class in C#.
//Available at: https://www.geeksforgeeks.org/c-sharp-list-class/ (Accessed: 22 June 2026)