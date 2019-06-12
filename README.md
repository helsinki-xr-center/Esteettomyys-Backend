# Esteettomyys-Backend

This is the backend server code for the accessibility project.

The technologies used are ASP.NET Core and MongoDB.

## Configuration

`hosting.json` includes any hosting related configuration for the built-in kestrel server.

Before the project can be started, the following configuration parameters must be present:
```json
"mongodb_connection": "mongodb://localhost (or other connection string if database is not local)",
"mongodb_database": "esteettomyys",
"mongodb_collection_users": "users",
"jwt_secret": "HMAC secret key should be placed here. (can be generated with HMACKeyGen included in this project)"
```

While developing, these should be placed in the local [user secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-2.2&tabs=windows) file.

In the server they should ideally be placed in the server environment variables.

**Remember to never commit any files containing sensitive data (like these secrets) to git!**

## Server setup

`TODO`
