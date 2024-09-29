# TaurBot Project 🤖

## Description 📋

This repository contains two projects aimed at creating and managing an investment portfolio, as well as interacting with users via a Telegram bot.

## Projects:

- **taur-bot-api**: This is an ASP.NET Core API designed for managing an investment portfolio. It uses the MVC architectural pattern, dependency injection (DI), singletons, and the repository pattern to handle portfolio data.

- **taur-bot**: This is a bot that communicates with the InvestmentAPI to interact with users. The bot is built using the Telegram.Bot library to provide a smooth user experience.

- **ethapi**: This API is specifically designed for cryptocurrency operations.

All projects run in containers and are orchestrated via Docker Compose.

## System Requirements ⚙️

- Docker
- Git

## Getting Started 🚀

Clone the repository:
```bash
git clone https://github.com/Kreuz41/TaurBot.git
```

## Setting up the Environment 🔧

Navigate to the root directory of **taur-bot-api** and create a `.env` file, defining the following environment variables:
```env-file
ASPNETCORE_ENVIRONMENT=Development
POSTGRES_USER=postgres
POSTGRES_PASSWORD=password
ConnectionStrings__string="your_connection_string"
```

Also, don’t forget to set up `appsettings.json` in the bot project!

## Running the Containers with Docker Compose 🐳

```bash
docker compose up --build
```

## Verifying the Setup ✅

After the containers are successfully started, check the availability of the API and the bot by visiting the following addresses:
- **API**: [http://localhost:5000/swagger/index.html](http://localhost:5000/swagger/index.html)
- **Bot**: Open your Telegram app and search for the bot by its name or ID.

## Maintenance and Management 🔄

- **API**: Any changes to the API can be made in the corresponding controllers and services in the `taur-bot-api` project. After making changes, restart the API container.
  
- **Bot**: Any logic updates for the bot can be made in the relevant files of the `taur-bot` project. After making changes, restart the bot container.

To restart the containers:
```bash
docker compose down
docker compose up --build
```

## Additional Information ℹ️

- Detailed API documentation for the **InvestmentAPI** is available on the Swagger page: [http://localhost:5000/swagger/index.html](http://localhost:5000/swagger/index.html).
- For configuring and modifying the bot behavior, refer to the documentation for the **Telegram.Bot** library.
- The cryptocurrency operations are powered by a custom-built **ethapi**.
