# BBL.ModernApp.AuditAggregate
## Containerize
### Database
> ```docker run -p 1433:1433 -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Password123" --name mssql-local -d mcr.microsoft.com/mssql/server```

### Sql Command (CLI)
> ```docker exec -it mssql-local /opt/mssql-tools/bin/sqlcmd -U sa -P Password123 -S localhost```

### Copy sql script files to container.
1. Create _bbl_ directory in _mssql_local_ container.
   >```docker exec -it mssql-local mkdir /bbl```
2. Copy create table sql files to bbl directory in _mssql_local_ container.
   >```docker cp BBL.ModernApp.AuditAggregate.Client/sql/table.sql mssql-local:/bbl```
3. Copy create storeprocedure sql files to bbl directory in _mssql_local_ container.
   >```docker cp BBL.ModernApp.AuditAggregate.Client/sql/procedure.sql mssql-local:/bbl```
3. Check file is existed in _bbl_ directory.
   >```docker exec -it mssql-local ls -lah /bbl```

### Install sql script files to Sql server container
1. Create table.
   >```docker exec -it mssql-local /opt/mssql-tools/bin/sqlcmd -U sa -P Password123 -S localhost -i /bbl/table.sql```
2. Create procedure
   >```docker exec -it mssql-local /opt/mssql-tools/bin/sqlcmd -U sa -P Password123 -S localhost -i /bbl/procedure.sql```

### Build The Client
**OSX**
```dotnet build -c Release -o app -r osx.12-x64 --no-self-contained```