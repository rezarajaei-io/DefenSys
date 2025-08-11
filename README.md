# DefenSys - A Modular Security Orchestration Platform

## 📜 Vision & Goal

DefenSys is a modern, open-source security tool designed to act as an orchestration platform for various vulnerability scanners. Instead of re-inventing the wheel, DefenSys provides a unified interface to run, manage, and view results from both its own built-in scanning modules and other professional-grade external tools.

This project is being developed as a portfolio piece to showcase modern software architecture and development practices in the .NET and Angular ecosystems.

---

## 🏛️ Architecture

This project is being built from the ground up using the principles of **Clean Architecture**. The goal is to create a system that is highly maintainable, testable, and independent of external frameworks.

The core dependency rule is: **Presentation → Application → Core**

-   **Core:** Contains the core domain entities and business rules.
-   **Application:** Contains the application logic, services, and use cases.
-   **Infrastructure/Presentation:** Handles external concerns like the Web API, database access, and integration with external tools.

---

## 🛠️ Technology Stack

-   **Backend:** ASP.NET Core 9, C#
-   **Frontend:** Angular 18, TypeScript, Tailwind CSS
-   **Architecture:** Clean Architecture
-   **Real-time:** SignalR (planned)
-   **Testing:** Docker (planned for sandboxing)

---

## 🌟 Vision & Future Direction

DefenSys is architected for growth. The current version establishes a strong foundation for what is planned to be a comprehensive security platform.

Future development will focus on expanding the scanning capabilities, introducing intelligent crawling features for automated discovery, and enhancing the engine-agnostic framework to integrate a wider array of industry-standard security tools. The goal is to evolve DefenSys into a truly versatile orchestration tool for developers and security professionals.

---

## 🚀 Getting Started

*(This section will be filled out once the project is in a runnable state after the refactoring.)*
