using Npgsql;

namespace practicing;

public class MyDbSet<T> where T : class, new() 
{
    private readonly string _tableName;
    private readonly string _connectionString;

    public MyDbSet(string tableName, string connectionString)
    {
        _tableName = tableName;
        _connectionString = connectionString;
    }

    public QueryBuilder Query() => new QueryBuilder().From(_tableName);
    
    public async Task<int> DeleteAsync(int id, string primaryKeyName = "product_id")
    {
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();
    
        var qb = new QueryBuilder().Delete(_tableName).Where(primaryKeyName, id);
        return await qb.ExecuteNonQueryAsync(conn);
    }
    
    
    public async Task<int> UpdateAsync(int id, Dictionary<string, object> data, string primaryKeyName = "product_id")
    {
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var qb = new QueryBuilder()
            .Update(_tableName, data)
            .Where(primaryKeyName, id);

        return await qb.ExecuteNonQueryAsync(conn);
    }
    
    
    public async Task<int> AddAsync(T entity)
    {
        var data = new Dictionary<string, object>();
    
        var properties = typeof(T).GetProperties();
        foreach (var prop in properties)
        {
          
            if (prop.Name.ToLower() == "id" || prop.Name.ToLower() == "product_id") 
                continue;
        
            var value = prop.GetValue(entity);
            if (value != null)
                data.Add(prop.Name.ToLower(), value); 
        }

        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var qb = new QueryBuilder().Insert(_tableName, data);
        return await qb.ExecuteNonQueryAsync(conn);
    }
}