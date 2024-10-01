# BudgetWebApp

This is a web application that allows users to manage their personal finances by recording transactions and categorizing them. The application uses SQL Server to store and retrieve data. Users can view, create, edit, and delete transactions and categories without being redirected to a new page, providing a seamless single-page application (SPA) experience.
<https://www.thecsharpacademy.com/project/27/budget>

## Features

- Contains models for transactions and categories, and a context to manage the data.
- Uses Entity Framework Core to interact with the SQL Server database and create the necessary schema.
- Implements a minimal API to connect the front-end and the database.
- Uses Razor Pages for the front-end to call the minimal API in the backend.
- Displays a list of transactions with options to add, edit, and delete entries.
- Provides a user-friendly interface for managing transactions and categories.
- Presents confirmation messages for delete operations and success messages for updates.
- Includes search functionality to find transactions by name.
- Includes filter functionality to show transactions per category and date.
- Uses modals to insert, delete, and update transactions and categories.

## Getting Started

To run the application, follow these steps:

1. Clone the repository to your local machine.
2. Open the solution in Visual Studio.
3. Build the solution to restore NuGet packages and compile the code.
4. Run the `BudgetWebApp` project to start the web application.

## Dependencies

- Microsoft.EntityFrameworkCore: The application uses this package to manage the SQL Server database context and entity relationships.
- Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore: The application uses this package for error handling related to the database.
- Swashbuckle.AspNetCore: The application uses this package to generate Swagger documentation for the API.

## Usage

1. The application will display a list of transactions with options to add, edit, and delete entries.
2. Use the input box to add new transactions.
3. Click on the edit button to modify existing transactions.
4. Click on the delete button to remove transactions from the list. A confirmation message will be presented before deletion.
5. Use the search box to find transactions by name.
6. Use the filter options to show transactions per category and date.

## License

This project is licensed under the MIT License.

## Resources Used

- [The C# Academy](https://www.thecsharpacademy.com/)
- [Microsoft Docs: Minimal APIs](https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-8.0&tabs=visual-studio)
- [Microsoft Docs: Calling a Web API in JavaScript](https://learn.microsoft.com/en-us/aspnet/core/tutorials/web-api-javascript?view=aspnetcore-8.0)
- GitHub Copilot to generate code snippets.
