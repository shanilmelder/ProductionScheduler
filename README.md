# Production Scheduler Setup Guide

## Installation Steps

1. **Configure Database Connection**
   - Open the `appsettings.Development.json` file
   - Change the connection string to your SQL Server

2. **Create Database**
   - Open a Terminal in the root of the project folder
   - Run the command:  
     ```sh
     dotnet ef database update --project .\ProductionScheduler.Services\ProductionScheduler.Services.csproj --startup-project .\ProductionScheduler.API\ProductionScheduler.API.csproj
     ```
     (This will create the database using migrations)

3. **Run the Project**
   - Open the project using the `ProductionScheduler.sln` file
   - Run the project

4. **Access the Application**
   - Open your browser and navigate to:  
     [https://localhost:7253/swagger](https://localhost:7253/swagger)

5. **Using the Application**
   - You will be able to use all the application's functionalities through the Swagger UI

6. **Important Note**
   - Please use `dd-MM-yyyy hh:mm:ss` format for all date/time inputs throughout the application
