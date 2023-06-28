# Relationship

# Install with docker and rancher
```bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=MyPass@word" -e "MSSQL_PID=Express" -p 1433:1433 -d --name=sql mcr.microsoft.com/mssql/server:latest
```

# Appsettings
```bash
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=gameDb;Trusted_Connection=False;User Id=sa;Password=MyPass@word;"
}
```
