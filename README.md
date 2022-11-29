## About The Project

LearningApp is .NET web api designed for quiz application. Api follows simple onion type architecture. <br>
Frontend part of application is also available here: [click](https://github.com/L3monPL/QuizAppAngular14)


## Built with

-  ASP.NET Core
-  SQL Server
- Entity Framework Core
- Microsoft Identity
- FluentValidation
- Auto Mapper
- XUnit
- Moq
- FluentAssertions
- Azure Storage Blobs


## Main Features
 - Authorization
 - User management
 - Creating questions divided by categories and difficulty levels
 - Tracking user progress in given category and level
 - Creating statistics
 - User scoreboard by user experience points
 - Rewarding users by unlocking user achivements
 - Managing images in storage
 
## Getting Started
In order to setup project locally:

 1.  Clone repository using git bash:
  ```sh
  git clone https://github.com/jacekcielecki/Learning-App.git
  ```
  
 2. Setup MSSQL Server connection string in  `appsettings.Development.json`
  ```sh
    "ConnectionStrings": {
    "azureSqlDb": "Server="SQL_SERVER_CONNECTION_STRING" 
    }
  ```
 
  3. Setup Azure Blob Storage connection string, and container names in `appsettings.json`
  ```sh
"BlobStorage": {
    "ConnectionString": "BLOB_STORAGE_CONNECTION_STRING",
    "ImageContainerName": "CONTAINER_NAME",
    "AvatarContainerName": "CONTAINER_NAME"
  }
  ```
  4.  Run application using dotnet cli (example)
  ```sh
  dotnet run --project C:\Users\username\source\repos\Learning-App\WSBLearn.WebApi\WSBLearn.WebApi.csproj
  ```

## Relationship Diagram
![enter image description here](https://wsblearnstorage.blob.core.windows.net/imagecontainer/drawSQL-export-2022-11-29_13_54-f6334cd5-1acc-4311-9681-9750f54c3a7a.png)

## Endpoints
Swagger Documentation available:
https://wsblearn-api.azurewebsites.net/swagger/index.html
