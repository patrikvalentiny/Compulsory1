using Dapper;
using Newtonsoft.Json;
using Npgsql;

namespace TestAPI;

public static class Helper
{
    public static readonly Uri Uri;
    public static readonly string ProperlyFormattedConnectionString;
    public static readonly NpgsqlDataSource DataSource;

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
            DataSource =
                new NpgsqlDataSourceBuilder(ProperlyFormattedConnectionString).Build();
            DataSource.OpenConnection().Close();
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
    
    public static void TriggerRebuildWithData(int rows = 10)
    {
        using (var conn = DataSource.OpenConnection())
        {
            try
            {
                 const string sqldata = "insert into box_factory.box_inventory (description, width, height, depth, location, guid) values ('Sed ante. Vivamus tortor. Duis mattis egestas metus.', 527.03, 176.65, 660.33, '5706 Di Loreto Center', '40e01a0b-d15f-49a0-ab4d-97925b4e3ebe');\ninsert into box_factory.box_inventory (description, width, height, depth, location, guid) values ('Nullam porttitor lacus at turpis. Donec posuere metus vitae ipsum. Aliquam non mauris.\n\nMorbi non lectus. Aliquam sit amet diam in magna bibendum imperdiet. Nullam orci pede, venenatis non, sodales sed, tincidunt eu, felis.\n\nFusce posuere felis sed lacus. Morbi sem mauris, laoreet ut, rhoncus aliquet, pulvinar sed, nisl. Nunc rhoncus dui vel sem.', 746.05, 591.33, 528.64, '09142 David Pass', 'a432ea94-e136-4df8-a6a4-4210d3573e16');\ninsert into box_factory.box_inventory (description, width, height, depth, location, guid) values ('Nam ultrices, libero non mattis pulvinar, nulla pede ullamcorper augue, a suscipit nulla elit ac nulla. Sed vel enim sit amet nunc viverra dapibus. Nulla suscipit ligula in lacus.\n\nCurabitur at ipsum ac tellus semper interdum. Mauris ullamcorper purus sit amet nulla. Quisque arcu libero, rutrum ac, lobortis vel, dapibus at, diam.', 660.59, 630.87, 963.78, '3 Farmco Parkway', '823524e8-1bbc-47b4-9ee8-c37e9f59715d');\ninsert into box_factory.box_inventory (description, width, height, depth, location, guid) values ('Aliquam quis turpis eget elit sodales scelerisque. Mauris sit amet eros. Suspendisse accumsan tortor quis turpis.\n\nSed ante. Vivamus tortor. Duis mattis egestas metus.', 762.38, 789.12, 141.31, '5313 Summit Alley', 'c639e063-1128-46b9-8944-cfe85aacbb1c');\ninsert into box_factory.box_inventory (description, width, height, depth, location, guid) values ('Sed sagittis. Nam congue, risus semper porta volutpat, quam pede lobortis ligula, sit amet eleifend pede libero quis orci. Nullam molestie nibh in lectus.', 226.78, 175.15, 533.07, '4 Mariners Cove Junction', '3cd3f6b9-8814-4a64-9fd9-f9a8f3e22b8d');\ninsert into box_factory.box_inventory (description, width, height, depth, location, guid) values ('Curabitur gravida nisi at nibh. In hac habitasse platea dictumst. Aliquam augue quam, sollicitudin vitae, consectetuer eget, rutrum at, lorem.\n\nInteger tincidunt ante vel ipsum. Praesent blandit lacinia erat. Vestibulum sed magna at nunc commodo placerat.', 657.54, 86.4, 339.5, '090 Bobwhite Way', 'eb6fa7b9-6059-441f-8601-6c51d6ebeb23');\ninsert into box_factory.box_inventory (description, width, height, depth, location, guid) values ('Mauris enim leo, rhoncus sed, vestibulum sit amet, cursus id, turpis. Integer aliquet, massa id lobortis convallis, tortor risus dapibus augue, vel accumsan tellus nisi eu orci. Mauris lacinia sapien quis libero.\n\nNullam sit amet turpis elementum ligula vehicula consequat. Morbi a ipsum. Integer a nibh.', 357.98, 143.79, 364.32, '2519 Division Plaza', '1917f270-e80b-4cd9-b052-dd741b5ebd91');\ninsert into box_factory.box_inventory (description, width, height, depth, location, guid) values ('Proin eu mi. Nulla ac enim. In tempor, turpis nec euismod scelerisque, quam turpis adipiscing lorem, vitae mattis nibh ligula nec sem.', 162.57, 751.08, 730.2, '1745 Stephen Park', 'f8c5cbb3-7339-4c5d-a750-c6a72d757eb6');\ninsert into box_factory.box_inventory (description, width, height, depth, location, guid) values ('Integer tincidunt ante vel ipsum. Praesent blandit lacinia erat. Vestibulum sed magna at nunc commodo placerat.', 921.93, 940.52, 789.54, '4 Kenwood Road', '996855dd-fc36-4af2-aa6c-a1feeafcf184');\ninsert into box_factory.box_inventory (description, width, height, depth, location, guid) values ('Integer ac leo. Pellentesque ultrices mattis odio. Donec vitae nisi.\n\nNam ultrices, libero non mattis pulvinar, nulla pede ullamcorper augue, a suscipit nulla elit ac nulla. Sed vel enim sit amet nunc viverra dapibus. Nulla suscipit ligula in lacus.\n\nCurabitur at ipsum ac tellus semper interdum. Mauris ullamcorper purus sit amet nulla. Quisque arcu libero, rutrum ac, lobortis vel, dapibus at, diam.', 393.25, 514.4, 607.32, '04 Ridge Oak Crossing', '6329dac6-ff84-4890-b1c3-98bc3e2dbd1f');";

                conn.Execute(RebuildScript);
                var sqlQueries = sqldata.Split(";\n", StringSplitOptions.RemoveEmptyEntries);
                Console.Out.WriteLine(sqlQueries[1]);
                foreach (var query in sqlQueries)
                {
                    conn.Execute(query);
                }
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

   }
