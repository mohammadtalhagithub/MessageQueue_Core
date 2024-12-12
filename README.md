# MSMQ Messaging System

This repository contains a messaging system built with **Microsoft Message Queuing (MSMQ)**. The solution includes three main components:

1. **Server Application**: Built with ASP.NET Core, acts as the central message hub.
2. **Client Application**: A Windows Forms app for sending and receiving messages.
3. **Core Library**: A separate DLL for handling MSMQ messaging functionality, reusable across the system.

---

## Features

- **Server (ASP.NET Core)**:
  - Processes incoming messages from clients.
  - Distributes messages to appropriate queues.
  - Provides API endpoints for client communication.

- **Client (WinForms)**:
  - User-friendly interface for interacting with the message queue.
  - Allows sending and retrieving messages via MSMQ.

- **Core Library (DLL)**:
  - Encapsulates MSMQ operations (e.g., creating queues, sending, and receiving messages).
  - Ensures a modular and reusable design.

---

## Installation

### Prerequisites
- .NET 6 SDK or later
- MSMQ installed and configured on your system
- Visual Studio or any compatible IDE

### Steps to Set Up
1. Clone the repository:
   ```bash
   git clone https://github.com/your-username/msmq-messaging-system.git
   cd msmq-messaging-system
