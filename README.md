# AdvancedForms

## Creating Migrations

Make sure ef tool is installed

    dotnet tool install --global dotnet-ef

https://logu.co/efcore-multiple-providers.html

    dotnet ef migrations add InitialCreate --context SqliteFormContext --output-dir Migrations/Sqlite --configuration EF_MIGRATION
    dotnet ef migrations add InitialCreate --context NpgsqlFormContext --output-dir Migrations/Npgsql --configuration EF_MIGRATION

