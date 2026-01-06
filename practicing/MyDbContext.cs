using Npgsql;

namespace practicing;

public class MyDbContext
{
    private readonly string _connectionString;
 
    public MyDbSet<ProductDto> Products { get; } 

    public MyDbContext(string connectionString)
    {
        _connectionString = connectionString;
        Products = new MyDbSet<ProductDto>("products", _connectionString);
    }

    public NpgsqlConnection CreateConnection() => new NpgsqlConnection(_connectionString);
}