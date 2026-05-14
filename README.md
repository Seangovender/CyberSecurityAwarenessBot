Cybersecurity Awareness Chatbot – Part 2 (WPF)

Student Information
- Name: Sean Govender
- Student Number: ST10491364
- Module: PROG6221 – Programming 2A

Overview
This project educates South African citizens about cybersecurity threats (phishing, weak passwords, scams, social engineering, unsafe browsing, privacy).  
It consists of two parts:
- Part 1 – Console application (basic chatbot with menu).
- Part 2 – WPF GUI application with advanced features.

Features (Part 2 – WPF)
- Graphical User Interface – Dark theme, colours, proper spacing, ASCII art, voice greeting (`welcome.wav`)
- Keyword recognition – Responds to: `phishing`, `password`, `safe browsing`, `social engineering`, `scam`, `privacy`
- Random responses – Each topic has a list of tips; a random tip is selected each time
- Conversation flow – Handles follow‑up commands like `"tell me more"`, `"another tip"`, `"explain more"`
- Memory – Remembers user's name and favourite topic
- Sentiment detection – Empathetic responses to `"worried"`, `"frustrated"`, `"curious"`
- Error handling – Default message for unrecognised input (no crashes)
- Code optimisation – Uses `Dictionary`, `List`, OOP, and XML comments

How to Run (Part 2 – WPF)
1. Open the solution `CyberSecurityAwarenessBot.sln` in Visual Studio.
2. Ensure `welcome.wav` is in the `CyberSecurityChatbot_Part2` project root and set Copy to Output Directory = `Copy if newer`.
3. Set `CyberSecurityChatbot_Part2` as the startup project.
4. Press F5 to build and run.
5. Type your name, then ask about any cybersecurity topic (e.g., `"tell me about passwords"`).

Example Conversation
Assistant: Hello! What's your name?
You: Sean
Assistant: Nice to meet you, Sean! I can help you with topics like...
You: tell me about passwords
Assistant: Use a different password for every account – don't reuse!
You: tell me more
Assistant: Here's another tip about password: A strong password has 12+ characters...
You: I'm worried about scams
Assistant: It's completely normal to feel worried, Sean... Always check the sender's email...