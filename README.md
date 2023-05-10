# AdvancedForms

## Run using docker

```sh
docker run -d \
  -p 8080:80 \
  -v ~/advancedforms.db:/app/AdvancedForms.db \
  -e Jwt__Secret=your-secret-key
  --name advancedforms_api \
  --restart=unless-stopped \
  ghcr.io/advancedforms-net/api:master
```

Generate `Jwt__Secret` using `openssl rand -hex 32`.

### Configuration

See configuration option examples in [appsettings.json](https://github.com/advancedforms-net/api/blob/master/AdvancedForms/appsettings.json).

To set these in the environment variables use `__` to seperate the different levels (e.g. Jwt__Secret).

### Health check

Health check is provided in the docker and can be done on the `/healthz` endpoint for external monitoring.

## Development

### Creating Migrations

Make sure ef tool is installed

    dotnet tool install --global dotnet-ef

https://logu.co/efcore-multiple-providers.html

```sh
dotnet ef migrations add InitialCreate --context SqliteFormContext --output-dir Migrations/Sqlite --configuration EF_MIGRATION
dotnet ef migrations add InitialCreate --context NpgsqlFormContext --output-dir Migrations/Npgsql --configuration EF_MIGRATION
```
