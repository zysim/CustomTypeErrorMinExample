# CustomTypeErrorMinExample

Minimal example demonstrating how custom data types cause seeding to fail if starting from an empty, non-migrated database.

The example contains a model `WeatherEntry` with a field `Name` that has type `citext`. On attempt to migrate from a fresh install, the migration fails, complaining that `citext` isn't loaded in. The only way around this is to comment the seeding code out first, migrate, then uncomment and migrate again.

## Steps

```bash
dotnet restore
dotnet tool restore
dotnet ef database update
```
- Expected results: a successful migration with seeded data
- Actual results:
    ```
    Failed executing DbCommand (6ms) [Parameters=[@p0='?'], CommandType='Text', CommandTimeout='30']
    INSERT INTO "WeatherEntries" ("Name")
    VALUES (@p0)
    RETURNING "Id";
    fail: Microsoft.EntityFrameworkCore.Update[10000]
          An exception occurred in the database while saving changes for context type 'ApplicationContext'.
          Microsoft.EntityFrameworkCore.DbUpdateException: An error occurred while saving the entity changes. See the inner exception for details.
           ---> System.NotSupportedException: The data type name 'citext', provided as NpgsqlDbType 'Citext', could not be found in the types that were loaded by Npgsql. Your database details or Npgsql type loading configuration may be incorrect. Alternatively your PostgreSQL installation might need to be upgraded, or an extension adding the missing data type might not have been installed.
    ```
