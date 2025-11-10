# User Management Technical Exercise

## Introduction
This is the result of my work on the this technical test. The exercise was relatively open-ended in that it had many ways to be approached.
Since this is an already started project, my understanding is that there is importance on showing an ability to work on an already existing codebase.
Ultimately, after the first couple of standard tasks, I wanted to focus on some of the more advanced tasks - which ended up with a lot of code being refactored.

The choice between addressing or creating tech debt can be tricky to make at times. If time was more in my favour, I would have like to make a point in writing in "planned" tech debt and then overhauled from there.

## Setup
### Configuration the project
You will need to create a `appsettings.json` file in the `UserManagement.API` project with the following content:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "<Your SQL Server Connection String>"
  }
}
```
> I ran my SQL Server instance using Docker.

### Running the project
You should be able to run the projects directly from Visual Studio or using the `dotnet` CLI (`dotnet run`). There is no API mocks for the front-end currently, so I recommend running `UserManagement.Api` first, and then running `UserManagement.UI`.

### Testing the project
You can run the `UserManagement.Data.Tests` without further need for configuration.

To run the `UserManagement.Api.IntergrationTests`, you will need to have Docker running on your machine as it uses TestContainers to automatically spin up a SQL Server instance for testing.

## My Changes
I started by completing the filtering functionality and then updating the User model to include DateOfBirth - including displaying it in the UI. From there, I started work on refactoring the application.

### UserManagement.API
I started here, creating an API for the front-end to consume. Something I that was part of workshopping at my previous work place was using the `TestContainers` library to do integration testing.
The implementation here is somewhat simple, but I do believe that testing a running application, against real services (SQL Server only in this case) is invaluable.

Better error handling and logging would be needed to make this API production ready. Typically, I would want to have a telemetry stack running that I could throw logs and analytics to (Prometheus/OpenTelemetry/Loki and Grafana).

### UserManagement.UI
Most of my "modern" front-end experience has been working with React (or JSX style frameworks). However, I wanted to try giving Blazor a go for this project as it was your preferred framework.

There are many things to be desired in this project, such as UI/UX improvements that would make the website look and feel nicer, along with accessibility and responsiveness ideally being at the core of its design. For the most part, it is derivative of the original `UserManagement.Web` project.

### UserManagement.Data
This project was refactored to use a real database, features migrations, and I implemented an async EntityRepository pattern.
This can be extended further to include a UnitOfWork pattern. About one of the tasks I didn't manage to get to - to add logging for user actions, this would be a great way to log actions that are intended to create changes within the database.

### TDLR (might have missed some changes);
- New API service for managing users.
- Integration tests using TestContainers for the API service.
- New UI project that uses Blazor.
- Changed UserManagement.Data to use a real database.
- Added migrations.
- Added helper scripts for setting up the database.
- Changed to async repository pattern.
- Added DateOfBirth to User model.
- Implemented filtering functionality.
- Implemented Add, View, Edit, Delete functionality in the UI.
- Refactored code to be more maintainable and extensible.

## Final Words
There is much more I would have wanted to do, such as front-end tests, infrastructural as code, CI/CD pipelines, logging, error handling, prettier/better UI code, etc.

Thank you for taking the time to review my project!
