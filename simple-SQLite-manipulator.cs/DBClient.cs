using System.Data;
using System.Data.Common;
using System.Data.SQLite;

internal class DBClient
{
  private static string? db_name;
  private string sql = "";
  private readonly List<Tuple<string, object>> parameters = new();

  internal static void Init(string dbname)
  {
    db_name = dbname;
  }

  internal void Add(string _sql)
  {
    sql += $" {_sql} ";
  }

  internal void AddParam(string key, object data)
  {
    parameters.Add(new Tuple<string, object>(key, data));
  }

  internal Dictionary<string, object> Select()
  {
    if (db_name == null)
    {
      throw new Exception("使用するデータベースを指定してください。");
    }

    SQLiteConnectionStringBuilder sqlConnectionSb = new() { DataSource = db_name };

    using var connection = new SQLiteConnection(sqlConnectionSb.ToString());
    connection.Open();

    using var cmd = new SQLiteCommand(connection);
    cmd.CommandText = sql;

    foreach (var parameter in parameters)
    {
      cmd.Parameters.Add(new SQLiteParameter(parameter.Item1, parameter.Item2));
    }

    Dictionary<string, object> result = new();

    using var reader = cmd.ExecuteReader();

    for (int i = 0; i < reader.FieldCount; i++)
    {
      result[reader.GetFieldValue<string>(i)] = reader.GetValue(i);
    }
    Reset();
    return result;
  }


  internal List<Dictionary<string, object>> SelectAll()
  {
    if (db_name == null)
    {
      throw new Exception("使用するデータベースを指定してください。");
    }

    SQLiteConnectionStringBuilder sqlConnectionSb = new() { DataSource = db_name };

    using var connection = new SQLiteConnection(sqlConnectionSb.ToString());
    connection.Open();

    using var cmd = new SQLiteCommand(connection);
    cmd.CommandText = sql;

    foreach (var parameter in parameters)
    {
      cmd.Parameters.Add(new SQLiteParameter(parameter.Item1, parameter.Item2));
    }

    List<Dictionary<string, object>> result = new();

    using var reader = cmd.ExecuteReader();
    while (reader.Read())
    {
      Dictionary<string, object> tmp_result = new();
      for (int i = 0; i < reader.FieldCount; i++)
      {
        tmp_result[reader.GetName(i)] = reader.GetValue(i);
      }
      result.Add(tmp_result);
    }
    Reset();
    return result;
  }


  internal void Execute()
  {
    if (db_name == null)
    {
      throw new Exception("使用するデータベースを指定してください。");
    }

    SQLiteConnectionStringBuilder sqlConnectionSb = new() { DataSource = db_name };

    using var connection = new SQLiteConnection(sqlConnectionSb.ToString());
    connection.Open();

    using var cmd = new SQLiteCommand(connection);
    cmd.CommandText = sql;

    foreach (var parameter in parameters)
    {
      cmd.Parameters.Add(new SQLiteParameter(parameter.Item1, parameter.Item2));
    }

    if (cmd.ExecuteNonQuery() == 0)
    {
      throw new Exception("having no record to execute the query specified.");
    }
    Reset();
    return;
  }

  private void Reset()
  {
    sql = "";
    parameters.Clear();
  }

}
