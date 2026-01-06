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

    public QueryBuilder Take(int count) { _limit = count; return this; }
    public QueryBuilder Skip(int count) { _offset = count; return this; }

    public string Build()
    {
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
}