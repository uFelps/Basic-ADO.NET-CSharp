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
    //ExecuteProcedure(connection);
    //ExecuteReadProcedure(connection);
    //OneToOne(connection);
    OneToMany(connection);
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

static void ExecuteProcedure(SqlConnection connection)
{
    var procedure = "[spDeleteStudent]";

    var pars = new { StudentId = "505E6C7E-8141-495F-B7CE-2D61D89D4DC4" };

    var rolls = connection.Execute(procedure, pars, commandType: CommandType.StoredProcedure);

    Console.WriteLine($"{rolls} Afetadas");
}

static void ExecuteReadProcedure(SqlConnection connection)
{
    var procedure = "[spGetCoursesByCategory]";

    var pars = new
    {
        CategoryId = "09CE0B7B-CFCA-497B-92C0-3290AD9D5142"
    };

    var courses = connection.Query(procedure, pars, commandType: CommandType.StoredProcedure);

    foreach (var item in courses)
    {
        Console.WriteLine(item);
    }
}

static void OneToOne(SqlConnection connection)
{
    var sql = "SELECT * FROM [CareerItem] INNER JOIN [Course] ON [CareerItem].[CourseId] = [Course].[Id]";

    var itens = connection.Query<CareerItem, Course, CareerItem>(
        sql,
        (careerItem, course) =>
        {
            careerItem.Course = course;
            return careerItem;
        }, splitOn: "Id");


    foreach (var item in itens)
    {
        Console.WriteLine($"{item.Title} - Curso: {item.Course.Title}" );
    }

    {
    }
}

static void OneToMany(SqlConnection connection)
{
    var sql = "SELECT * FROM [Career] INNER JOIN [CareerItem] ON [Career].[Id] = [CareerItem].[CareerId]";

    var carreiras = new List<Career>();

    var list = connection.Query<Career, CareerItem, Career>(sql,
        (career, item) =>
        {
            var find = carreiras.Find(x => x.Id == career.Id);

            if (find == null)
            {
                find = career;
                find.Items.Add(item);
                carreiras.Add(find);
                
            }
            else
            {
                find.Items.Add(item);
            }
            
            

            return career;
        }, 
        splitOn: "CareerId");

    foreach (var career in carreiras)
    {
        Console.WriteLine($"{career.Title}");
        foreach (var itens in career.Items)
        {
            Console.WriteLine($" - {itens.Title}");
        }
    }
    {
        
    }
}