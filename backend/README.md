# LoopHabitsAPI

This project was generated with [dotnet](https://learn.microsoft.com/en-us/dotnet/core/install/) version 7.0.401.
PostgreSQL version 15.3.

## Development server

Add and configure `appsettings.Development.json` file to core project folder (backend/LoopHabitsAPI):

```
{
  "ConnectionStrings": {
    "PostgresConnection": "Host=localhost;Port=your_port_here;Database=LoopHabits;Username=your_username_here;Password=your_password_here",
    "SqliteConnection": "Data Source=LoopHabitsBackup.db"
  },
  "JwtSettings": {
    "ValidIssuer": "LoopHabitsAPI",
    "ValidAudience": "https://localhost:40443",
    "Expires": 15
  },
  "AllowedCORS":  "https://localhost:40443"
}
```

Configure environment variable `SECRETLOOPHABITS` (used in AuthenticationService).

Run the project.