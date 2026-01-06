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
}