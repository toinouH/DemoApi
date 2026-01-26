```shell
dotnet ef migrations add Initial
dotnet ef database update
```
Ces commande sont les équivalentes de:
```
Add-Migration InitialCreate
Update-Database
```