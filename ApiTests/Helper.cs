using System.Runtime.CompilerServices;
using Dapper;
using Newtonsoft.Json;
using Npgsql;

namespace TestAPI;

public static class Helper
{
    public static Uri Uri;
    public static string ProperlyFormattedConnectionString;
    public static NpgsqlDataSource DataSource;

    public static string RebuildScript = @"
DROP SCHEMA IF EXISTS box_factory CASCADE;
CREATE SCHEMA box_factory;
CREATE EXTENSION IF NOT EXISTS ""uuid-ossp"";
create table if not exists box_factory.box_inventory
(
    guid             uuid                     default uuid_generate_v4() not null,
    width            numeric,
    height           numeric,
    depth            numeric,
    location         varchar(256),
    description      text,
    datetime_created timestamp with time zone default CURRENT_TIMESTAMP
);
 ";

    public static string NoResponseMessage = @"
🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨
It looks like you failed to get a response from the API.
Are you 100% sure the API is already running on localhost port 5000?
Below is the inner exception.
Best regards, Alex
🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨
";

    static Helper()
    {
        string rawConnectionString;
        var envVarKeyName = "pgconn";

        rawConnectionString = Environment.GetEnvironmentVariable(envVarKeyName)!;
        if (rawConnectionString == null)
            throw new Exception($@"
🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨
YOUR CONN STRING PGCONN IS EMPTY.
Solution: Go to Settings, search for Test Runner, and add the environment variable in there.
Currently, your program looks for an environment variable is called {envVarKeyName}.

Best regards, Alex
🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨
");

        try
        {
            Uri = new Uri(rawConnectionString);
            ProperlyFormattedConnectionString = string.Format(
                "Server={0};Database={1};User Id={2};Password={3};Port={4};Pooling=true;MaxPoolSize=3",
                Uri.Host,
                Uri.AbsolutePath.Trim('/'),
                Uri.UserInfo.Split(':')[0],
                Uri.UserInfo.Split(':')[1],
                Uri.Port > 0 ? Uri.Port : 5432);
            DataSource = CreateDataSource();
        }
        catch (Exception e)
        {
            throw new Exception(@"
🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨
Your connection string is found, but could not be used. Are you sure you correctly inserted
the connection-string to Postgres?

Best regards, Alex
(Below is the inner exception)
🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨", e);
        }
    }

    public static async Task<bool> IsCorsFullyEnabledAsync(string path)
    {
        using var client = new HttpClient();
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Options, new Uri(path));
            // Add Origin header to simulate CORS request
            request.Headers.Add("Origin", "http://localhost:5000");
            request.Headers.Add("Access-Control-Request-Method", "GET");
            request.Headers.Add("Access-Control-Request-Headers", "X-Requested-With");

            var response = await client.SendAsync(request);

            var corsEnabled = false;

            if (response.Headers.Contains("Access-Control-Allow-Origin"))
            {
                var accessControlAllowOrigin =
                    response.Headers.GetValues("Access-Control-Allow-Origin").FirstOrDefault();
                corsEnabled = accessControlAllowOrigin == "*" ||
                              accessControlAllowOrigin == "http://localhost:5000";
            }

            var accessControlAllowMethods = response.Headers.GetValues("Access-Control-Allow-Methods").FirstOrDefault();
            var accessControlAllowHeaders = response.Headers.GetValues("Access-Control-Allow-Headers").FirstOrDefault();

            if (corsEnabled && accessControlAllowMethods != null && accessControlAllowMethods.Contains("GET") &&
                accessControlAllowHeaders != null && accessControlAllowHeaders.Contains("X-Requested-With"))
                return true;
        }
        catch (Exception)
        {
            throw new Exception(
                "\nCORS IS NOT ENABLED. PLEASE ENABLE CORS.\n(check last part of the project description)\n");
        }


        return false;
    }

    public static string BadResponseBody(string content)
    {
        return $@"
🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨
Hey buddy, I've tried to take the response body from the API and turn into a class object,
but that failed. Below is what you sent me + the inner exception.

Best regards, Alex
🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨
RESPONSE BODY: {content}

EXCEPTION:
";
    }

    public static void TriggerRebuild()
    {
        using (var conn = DataSource.OpenConnection())
        {
            try
            {
                conn.Execute(RebuildScript);
            }
            catch (Exception e)
            {
                throw new Exception(@"
🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨
THERE WAS AN ERROR REBUILDING THE DATABASE.

Check the following: Are you running the postgres DB at Amazon Web Services in Stockholm?

Best regards, Alex.
(Below is the inner exception)
🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨", e);
            }
        }
    }

    public static string MyBecause(object actual, object expected)
    {
        var expectedJson = JsonConvert.SerializeObject(expected, Formatting.Indented);
        var actualJson = JsonConvert.SerializeObject(actual, Formatting.Indented);

        return $"because we want these objects to be equivalent:\nExpected:\n{expectedJson}\nActual:\n{actualJson}";
    }

    public static NpgsqlDataSource CreateDataSource()
    {
        try
        {
            NpgsqlDataSource dataSource =
                new NpgsqlDataSourceBuilder(ProperlyFormattedConnectionString).Build();
            dataSource.OpenConnection().Close();
            return dataSource;
        }
        catch (Exception e)
        {
            throw new Exception(@"
🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨
Your connection string is found, but could not be used. Are you sure you correctly inserted
the connection-string to Postgres?

Best regards, Alex
(Below is the inner exception)
🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨🧨", e);
        }
    }
}