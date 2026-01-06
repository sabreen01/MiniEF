
using Npgsql;
namespace practicing;

public class ProductRepository
{
    private readonly string _connection;
    public ProductRepository(string connection)
    {
        _connection = connection;
    }
    //GetAllProducts withCatName
    public async Task<List<ProductDto>> GetAllProducts()
    {
        var Products = new List<ProductDto>();
        try
        {
            await using var conn = new NpgsqlConnection(_connection);
            await conn.OpenAsync();

            string sql = @"SELECT 
                    p.product_id, 
                    p.name AS product_name, 
                    p.price, 
                    c.name AS category_name
                  FROM products p
                  JOIN categories c ON p.category_id = c.category_id
                  LIMIT 9";
            await using var cmd = new NpgsqlCommand(sql, conn);
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var product = new ProductDto
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Price = reader.GetDecimal(2),
                    CategoryName = reader.GetString(3)
                };
                Products.Add(product);
            }
        }
        catch

        {
            Console.WriteLine("Error occured");
        }

        return Products;

    }
    
    
    //GetProductById
    public async Task<ProductDto?> GetProductById(int id)
    {
        try
        {
            await using var conn = new NpgsqlConnection(_connection);
            await conn.OpenAsync();
            string sql = @"SELECT p.product_id, p.name, p.price, c.name 
                       FROM products p 
                       JOIN categories c ON p.category_id = c.category_id 
                       WHERE p.product_id = @id ";
            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            await using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new ProductDto
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Price = reader.GetDecimal(2),
                    CategoryName = reader.GetString(3)

                };

            }
        }
        catch
        {
            Console.WriteLine("Error occured");
        }
        return  null;
        
    }
    
    
    //AddNewProduct
    public async Task AddProduct(ProductDto product)
    {
        try
        {
            await using var conn = new NpgsqlConnection(_connection);
            await conn.OpenAsync();

            string sql = @"INSERT INTO products (name, price, category_id) 
                       VALUES (@name, @price, @catId)";

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@name", product.Name);
            cmd.Parameters.AddWithValue("@price", product.Price);
            cmd.Parameters.AddWithValue("@CatId", 1);

            await cmd.ExecuteNonQueryAsync();
            Console.WriteLine("Product added");
        }
        catch
        {
            Console.WriteLine("Error occured");
        }
    }
    
    //Update product 
    public async Task UpdateProduct(int id , Decimal NewPrice)
    {
        try
        {
            await using var conn = new NpgsqlConnection(_connection);
            await conn.OpenAsync();
            string sql = @"UPDATE products SET price = @NewPrice WHERE product_id = @id";
            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@NewPrice", NewPrice);
            await cmd.ExecuteNonQueryAsync();
            Console.WriteLine("Product updated");

        }
        catch
        {
            Console.WriteLine("Error occured");
        }
    }
    
    //delet Product
    public async Task DeleteProduct(int id)
    {
        try
        {
            await using var conn = new NpgsqlConnection(_connection);
            await conn.OpenAsync();
            string sql = @"DELETE FROM products WHERE product_id = @id";
            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            await cmd.ExecuteNonQueryAsync();
            Console.WriteLine("Product deleted");
        }

        catch
        {
            Console.WriteLine("Error occured");
        }
    }
    
    
}