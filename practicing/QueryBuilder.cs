using System.Text;
using Npgsql;

namespace practicing;

public class QueryBuilder
{
    private string _table = "";
    private List<string> _columns = new List<string>();
    private List<string> _whereConditions = new List<string>();
    private List<string> _orderBy = new List<string>();
    private int? _limit;
    private int? _offset;
    private Dictionary<string, object> _parameters = new Dictionary<string, object>();
    private bool _isDelete = false;
    private bool _isUpdate = false;
    private Dictionary<string, object> _updateData = new Dictionary<string, object>();
    
    private bool _isInsert = false;
    private Dictionary<string, object> _insertData = new();
    

    public QueryBuilder From(string table)
    {
        _table = table; return this;
    }

    public QueryBuilder Select(params string[] columns)
    {
        _columns.AddRange(columns); return this;
    }
    
    public QueryBuilder Where(string column, object value, string op = "=")
    {
        string paramName = $"@p{_parameters.Count}";
        _whereConditions.Add($"{column} {op} {paramName}");
        _parameters.Add(paramName, value);
        return this;
    }

    public QueryBuilder OrderBy(string column, bool ascending = true)
    {
        _orderBy.Add($"{column} {(ascending ? "ASC" : "DESC")}");
        return this;
    }

    public QueryBuilder Take(int count)
    {
        _limit = count; return this;
    }

    public QueryBuilder Skip(int count)
    {
        _offset = count; return this;
    }

    
    
   
    public QueryBuilder Delete(string table)
    {
        _table = table;
        _isDelete = true; 
        return this;
    }


    public QueryBuilder Update(string table, Dictionary<string, object> data)
    {
        _table = table;
        _updateData = data; 
        _isUpdate = true;
        return this;
    }
    
    
    public QueryBuilder Insert(string table, Dictionary<string, object> data)
    {
        _table = table;
        _insertData = data;
        _isInsert = true;
        return this;
    }
    
    
    public string Build()
    {
        if (_isInsert)
        {
           
            var columns = string.Join(", ", _insertData.Keys);
            var values = string.Join(", ", _insertData.Keys.Select(k => $"@i_{k}"));
    
            foreach (var entry in _insertData)
                _parameters[$"@i_{entry.Key}"] = entry.Value;

            return $"INSERT INTO {_table} ({columns}) VALUES ({values})";
        }
        
        if (_isDelete)
        {
            return $"DELETE FROM {_table} WHERE " + string.Join(" AND ", _whereConditions);
        }
    
       
        
        if (_isUpdate)
        {
          
            var setClause = 
                string.Join(", ", _updateData.Keys.Select(key => $"{key} = @u_{key}"));
        
            foreach (var entry in _updateData)
            {
                _parameters[$"@u_{entry.Key}"] = entry.Value;
            }

            var sqlUpdate = $"UPDATE {_table} SET {setClause}";
            if (_whereConditions.Count > 0)
                sqlUpdate += " WHERE " + string.Join(" AND ", _whereConditions);
            
            return sqlUpdate;
        }
        
        var sql = new StringBuilder
            ($"SELECT {(_columns.Count > 0 ? string.Join(", ", _columns) : "*")} FROM {_table}");
        if (_whereConditions.Count > 0) sql.Append(" WHERE " + string.Join(" AND ", _whereConditions));
        if (_orderBy.Count > 0) sql.Append(" ORDER BY " + string.Join(", ", _orderBy));
        if (_limit.HasValue) sql.Append($" LIMIT {_limit}");
        if (_offset.HasValue) sql.Append($" OFFSET {_offset}");
        return sql.ToString();
    }

    public async Task<List<T>> ExecuteListAsync<T>(NpgsqlConnection conn, Func<NpgsqlDataReader, T> map)
    {
        await using var cmd = new NpgsqlCommand(Build(), conn);
        foreach (var p in _parameters) cmd.Parameters.AddWithValue(p.Key, p.Value);
        await using var reader = await cmd.ExecuteReaderAsync();
        var results = new List<T>();
        while (await reader.ReadAsync()) results.Add(map(reader));
        return results;
    }
    
    
    public async Task<int> ExecuteNonQueryAsync(NpgsqlConnection conn)
    {
        await using var cmd = new NpgsqlCommand(Build(), conn);
        foreach (var p in _parameters)
            cmd.Parameters.AddWithValue(p.Key, p.Value);
        
        return await cmd.ExecuteNonQueryAsync(); 
    }
}