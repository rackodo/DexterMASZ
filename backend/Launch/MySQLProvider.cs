using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace Launch;

public class MySqlProvider
{
	public Action<DbContextOptionsBuilder> GetDatabaseBuilder()
	{
		var server = ConsoleCreator.AskAndSet("server host", "MYSQL_HOST");

		var port = ConsoleCreator.AskAndSet("server port", "MYSQL_PORT");

		var database = ConsoleCreator.AskAndSet("database name", "MYSQL_DATABASE");

		var uid = ConsoleCreator.AskAndSet("login username", "MYSQL_USER");

		var pwd = ConsoleCreator.AskAndSet("login password", "MYSQL_PASSWORD");

		ConsoleCreator.AddSubHeading("Successfully created: ", "MySQL Database Provider");

		var connectionString = $"Server={server};Port={port};Database={database};Uid={uid};Pwd={pwd};";

		return x => x.UseMySql(
			connectionString,
			ServerVersion.AutoDetect(connectionString),
			o => o.SchemaBehavior(MySqlSchemaBehavior.Translate, (schema, table) => $"{schema}_{table}")
		);
	}
}