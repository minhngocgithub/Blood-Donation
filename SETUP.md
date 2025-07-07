## Setting up the DefaultConnection (SQL Server)

To run this project, you need to configure the database connection string so the application can connect to your local SQL Server instance.

### 1. Locate the Connection String

Open either `appsettings.json` or `appsettings.Development.json` in the project root. Find the section:

```
"ConnectionStrings": {
  "DefaultConnection": ""
}
```

### 2. Update the DefaultConnection

Replace the value of `DefaultConnection` with your local SQL Server connection string. For example:

```
"DefaultConnection": "Data Source=YOUR_SERVER_NAME;Initial Catalog=Blood_Donation;Integrated Security=True;Trust Server Certificate=True"
```

- `YOUR_SERVER_NAME` is usually `localhost`, `.` (dot), or `MACHINE_NAME\\SQLEXPRESS` for SQL Server Express.
- `Initial Catalog` should match your database name (e.g., `Blood_Donation`).
- `Integrated Security=True` uses Windows Authentication. If you use SQL Authentication, replace with `User ID=your_user;Password=your_password`.

**Example for SQL Server Express:**
```
"DefaultConnection": "Data Source=localhost\\SQLEXPRESS;Initial Catalog=Blood_Donation;Integrated Security=True;Trust Server Certificate=True"
```

### 3. Save and Run

After updating, save the file and run the project. The application will use the specified connection string to connect to your local database.

If you have any issues connecting, ensure:
- SQL Server is running.
- The database `Blood_Donation` exists.
- Your user has access rights.

## Running in Development Mode

For local development, it is recommended to run the project in **Development** mode. When running in this mode, the application will automatically use the settings from `appsettings.Development.json` (overriding values in `appsettings.json`).

### How to Run in Development Mode

- **Visual Studio:** By default, running or debugging the project will use the `Development` environment.
- **Command Line:** You can set the environment variable before running the project:
  - On Windows (PowerShell):
    ```powershell
    $env:ASPNETCORE_ENVIRONMENT = "Development"
    dotnet run
    ```
  - On Windows (CMD):
    ```cmd
    set ASPNETCORE_ENVIRONMENT=Development
    dotnet run
    ```
  - On Linux/macOS:
    ```bash
    export ASPNETCORE_ENVIRONMENT=Development
    dotnet run
    ```

When running in `Development` mode, the application will use the connection string and other settings from `appsettings.Development.json`. This is the recommended approach for local development and testing.