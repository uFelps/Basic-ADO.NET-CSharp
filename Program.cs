using System.Data;
using Microsoft.Data.SqlClient;

const string connectionString = "Server=localhost,1433;Database=balta;User ID=sa;Password=1q2w3e4r@#$;TrustServerCertificate=True;";

using (var connetion = new SqlConnection(connectionString))
{
    Console.WriteLine("Conectado");
    connetion.Open();

    using (var command = new SqlCommand())
    {
        command.Connection = connetion;
        command.CommandType = CommandType.Text;
        command.CommandText = "SELECT [Id], [Title] FROM [Category]";

        var reader = command.ExecuteReader();

        while (reader.Read())
        {
            Console.WriteLine($"{reader.GetGuid(0)}, {reader.GetString(1)}");
        }
    }
    
    connetion.Close();
    
}