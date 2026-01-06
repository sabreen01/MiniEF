namespace practicing;
using Npgsql;
using System;

public class DatabaseConnection(string connectionString)
{
    
    public async Task<bool> Connect()
    {
        try
        {
            await using var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
    
}