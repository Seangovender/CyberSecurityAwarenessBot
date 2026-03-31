# CyberSecurityAwarenessBot

## Overview
This project is a simple console-based chatbot built in C# to help users understand basic cybersecurity concepts. The chatbot focuses on common online threats such as phishing, weak passwords, scams, social engineering, and unsafe browsing.

The goal of this application is to give users simple and practical information so they can stay safer when using the internet.

## Features
- Voice greeting using a WAV audio file
- ASCII banner displayed when the program starts
- Personalised interaction (asks for the user's name)
- Menu with different cybersecurity topics
- Input validation to handle incorrect or empty input
- Colour formatting for better readability
- GitHub Actions workflow to automatically build the project

## Technologies Used
- C#
- .NET 8 (Windows)
- Visual Studio
- GitHub
- GitHub Actions

## How to Run the Program
1. Open the project in Visual Studio.
2. Make sure `welcome.wav` is inside the project folder.
3. Build the solution.
4. Run the program using **Ctrl + F5**.
5. Follow the menu options shown in the console.

## Project Structure
- `Program.cs` – Entry point of the application
- `CyberSecurityBot.cs` – Contains all chatbot logic
- `welcome.wav` – Audio greeting file
- `README.md` – Project documentation
- `.github/workflows/dotnet-ci.yml` – CI workflow file

## Purpose of the Project
The purpose of this project is to demonstrate basic C# programming concepts such as methods, user input, validation, and file handling, while also creating something useful that promotes cybersecurity awareness.

## Notes
- The audio greeting works only on Windows systems.
- If the audio file is missing, the program will still run without crashing.

## Future Improvements
- Add more quiz questions
- Improve the user interface
- Store user responses
- Convert the chatbot into a GUI application