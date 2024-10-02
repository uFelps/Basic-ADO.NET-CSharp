using System.Data;
using Dapper;
using dataAccess.Model;
using Microsoft.Data.SqlClient;

const string connectionString =
    "Server=localhost,1433;Database=balta;User ID=sa;Password=1q2w3e4r@#$;TrustServerCertificate=True;";


using (var connection = new SqlConnection(connectionString))
{
    //ListCategories(connection);
    //CreateCategory(connection);
    //UpdateCategory(connection);
    //DeleteCategory(connection);
}


static void ListCategories(SqlConnection connection)
{
    var categories = connection.Query<Category>("SELECT [Id], [Title] FROM [Category]");

    foreach (var category in categories)
    {
        Console.WriteLine($"{category.Id}, {category.Title}");
    }
}

static void CreateCategory(SqlConnection connection)
{
    var newcategory = new Category(new Guid(),
        "Amazon AWS", "amazon.com", "AWS Cloud", 8,
        "Categoria destinada aos serviços aws", false);

    var insertSql = "INSERT INTO [Category] VALUES(@Id, @Title, @Url, @Summary, @Order, @Description, @Featured)";

    var rolls = connection.Execute(insertSql, new
    {
        newcategory.Id, newcategory.Title, newcategory.Url, newcategory.Summary,
        newcategory.Order, newcategory.Description, newcategory.Featured
    });

    Console.WriteLine($"{rolls} Linhas Inseridas");
}

static void UpdateCategory(SqlConnection connection)
{
    var updateSql = "UPDATE [Category] SET [Title] = @title WHERE [Id] = @id";

    var rolls = connection.Execute(updateSql, new
    {
        id = "00000000-0000-0000-0000-000000000000",
        title = "Azure"
    });

    Console.WriteLine($"{rolls} Linhas Atualizadas");
}

static void DeleteCategory(SqlConnection connection)
{
    var deleteSql = "DELETE [Category] WHERE [Title] LIKE @title";

    var rolls = connection.Execute(deleteSql, new
    {
        title = "Azure"
    });

    Console.WriteLine($"{rolls} Linhas deletadas");
}

