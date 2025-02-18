using Microsoft.Data.SqlClient;
using System.Text;

string id = "42";
if (args.Length == 1)
{
    id = args[0];
}

SqlSample(id);
Console.WriteLine("Hello, World!");

static string SqlSample(string id)
{
    string connectionString = "";
    using SqlConnection sqlConnection = new(connectionString);
    SqlCommand command = sqlConnection.CreateCommand();

    // don't do this - string concatenation for SQL commands!
    command.CommandText = "SELECT * FROM Customers WHERE City = " + id;

    // do this instead:
    //SqlParameter parameter = new("@City", System.Data.SqlDbType.NVarChar);
    //parameter.Value = id;
    //command.CommandText = "SELECT * FROM Customers WHERE City = @id";
    //command.Parameters.Add(parameter);
    sqlConnection.Open();
    using SqlDataReader reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

    StringBuilder sb = new();
    while (reader.Read())
    {
        for (int i = 0; i < reader.FieldCount; i++)
        {
            sb.Append(reader[i]);
        }
        sb.AppendLine();
    }
    return sb.ToString();
}

