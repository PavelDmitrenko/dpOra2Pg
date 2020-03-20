# dpOra2Pg
### Oracle to a PostgreSQL converter

* Direct data import into PostgreSQL using `BINARY COPY` without intermediate steps and temporary files;
* No Oracle client required;
* No Postgres installation required;
* No Oracle DBA permissions required;

## Dependencies
+ .NET Core 3.1

## Usage
`dotnet dpOra2Db.dll SampleConfig.json`

## Sample config
```json
{
	"Oracle": {
		"Host": "127.0.0.1",
		"Port": 1521,
		"Service": "service",
		"Schema": "schema",
		"UserID": "userid",
		"Password": "password"
	},
	"Postgres": {
		"Host": "127.0.0.1",
		"Port": 5432,
		"Database": "db",
		"UserID": "userid",
		"Password": "password",
		"Schema": "public"
	}
}```
