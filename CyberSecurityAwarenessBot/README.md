# CyberSecurityAwarenessBot

## Overview
This project is a console-based chatbot developed in C# to help educate users about basic cybersecurity awareness. The chatbot focuses on common online threats such as phishing, weak passwords, scams, social engineering, and unsafe browsing habits.

The aim of this program is to provide simple and easy-to-understand information that can help users stay safe while using the internet.

## Features
- Voice greeting using a WAV audio file
- Simple ASCII banner on startup
- Personalised user interaction
- Menu with different cybersecurity topics
- Basic input validation
- Colour formatting to improve readability
- GitHub Actions workflow for automatic build checks

## Technologies Used
- C#
- .NET 8 (Windows)
- Visual Studio
- GitHub
- GitHub Actions

## How to Run the Program
1. Open the project in Visual Studio.
2. Make sure the file `welcome.wav` is inside the project folder.
3. Build the solution.
4. Run the program using Ctrl + F5.
5. Follow the instructions in the console.

## Project Files
- `Program.cs` – Starts the application
- `CyberSecurityBot.cs` – Contains the chatbot logic
- `welcome.wav` – Audio greeting file
- `README.md` – Project documentation
- `.github/workflows/dotnet-ci.yml` – CI build workflow

## Purpose of the Project
The purpose of this project is to demonstrate basic programming skills while creating a useful application that promotes cybersecurity awareness. It also shows the use of user input, validation, audio support, and version control using GitHub.

## Notes
- The audio greeting will only play on Windows systems.
- If the audio file is missing, the program will still run.