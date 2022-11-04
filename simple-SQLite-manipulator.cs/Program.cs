// テーブルの初期化
DBClient.Init("test.db");

// テーブルの作成
{
  DBClient client = new();
  client.Add("CREATE TABLE IF NOT EXISTS helloworld(");
  client.Add("  number INTEGER PRIMARY KEY AUTOINCREMENT,");
  client.Add("  comment VARCHAR(100) NULL");
  client.Add(");");
  try
  {
    client.Execute();
  } catch
  {
    // スルー
  }
}

// 新規コメントの追加
{
  Console.Write("comment -> ");
  var new_comment = Console.ReadLine()?.Trim() ?? "";

  DBClient client = new();
  client.Add("INSERT INTO helloworld(comment)");
  client.Add("VALUES(@comment);");
  client.AddParam("@comment", new_comment);
  client.Execute();

}

// コメントの取得
{
  DBClient client = new();
  client.Add("SELECT number, comment");
  client.Add("FROM helloworld");
  var result = client.SelectAll();
  foreach (var record in result)
  {
    foreach (var column in record)
    {
      Console.Write($"{column.Key} -> {column.Value}   |   ");
    }
    Console.WriteLine();
  }
}
