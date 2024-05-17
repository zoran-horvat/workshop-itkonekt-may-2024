# workshop-design-patterns
Materials for the workshop on design patterns in .NET

The demo uses EF Core. Verify the connection string in Bookstore\AppSettings.Development.json and update it to connect to the server you have available.

Before running the demo, create/update the database: `dotnet ef database update --project ./Bookstore`

The application seeds the database on first run, so the data will be available in the new database.
