 // Cybersecurity Awareness Chatbot - Part 2 (WPF)
 // Module: PROG6221
 // Student Number: 10491364
 // Description: Main window code-behind – handles GUI events, voice greeting,
 //              chat display, and user input processing.
 

using System;
using System.Collections;
using System.IO;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CyberSecurityChatbot_Part2
{
    public partial class MainWindow : Window
    {
        private ChatbotEngine bot;

        public MainWindow()
        {
            InitializeComponent();
            bot = new ChatbotEngine();
            PlayVoiceGreeting();
            AppendMessage("Assistant", "Hello! What's your name?");
        }

       
        // Plays the welcome.wav audio file asynchronously if it exists.
        
        private void PlayVoiceGreeting()
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "welcome.wav");
                if (File.Exists(path))
                {
                    using (SoundPlayer player = new SoundPlayer(path))
                    {
                        player.Play(); // asynchronous, does not block UI
                    }
                }
            }
            catch { /* silent fail – app still works */ }
        }

        
        // Appends a message to the chat history ListBox and auto-scrolls to the bottom.
        
        private void AppendMessage(string sender, string message)
        {
            ChatHistory.Items.Add($"{sender}: {message}");
            // Auto-scroll to the latest message
            if (ChatHistory.Items.Count > 0)
                ChatHistory.ScrollIntoView(ChatHistory.Items[ChatHistory.Items.Count - 1]);
        }

        
        // Handles the Send button click.
        
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessUserInput();
        }

        
        // Handles the Enter key press in the input text box.
      
        private void UserInputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ProcessUserInput();
                e.Handled = true;
            }
        }

        
        // Processes the user's input, clears the input box, and displays the bot's response.
        
        private void ProcessUserInput()
        {
            string userText = UserInputBox.Text.Trim();
            if (string.IsNullOrEmpty(userText))
                return;

            AppendMessage("You", userText);
            UserInputBox.Clear();

            string response = bot.ProcessInput(userText);
            AppendMessage("Assistant", response);
        }
    }
}

// Stack Overflow (2015).WPF ListBox auto-scroll and key binding examples.
// Available at: https://stackoverflow.com/questions/tagged/wpf (Accessed:13 May 2026)