## About The Project

LearningApp is .NET web api tailored for a online quiz application. Project architecture designed following Clean Architecture by Jayson Taylor. <br>
[React UI](https://github.com/jacekcielecki/LearningAppWeb) for application available, as well as it's [alternative](https://github.com/L3monPL/QuizAppAngular14) counterpart.

## Built with

- ASP.NET Core
- SQL Server
- Entity Framework Core
- Microsoft Identity
- FluentValidation
- AutoMapper
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
![enter image description here](https://wsblearnstorage.blob.core.windows.net/imagecontainer/drawSQL-learningapp-export-2023-06-12-488482a8-af98-4e61-ad44-dee25c6f52d5.png)

## Endpoints
Swagger Documentation available:
https://wsblearn-api.azurewebsites.net/swagger/index.html
