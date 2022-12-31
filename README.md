# DBClient.sqlite.cs

C#でSQLiteを操作するクライアントクラス。  

## 使い方

DBClientインスタンスの接続文字列を設定。  

```vb
DBClient.Init("データベースファイルパス")
```

接続文字列はstaticメンバであるため、各インスタンス間でその値を共有する。  

---

インスタンスを生成。  

```vb
DBClient client = new();
```

---

SQLを追加。  

```vb
client.Add("SELECT number, name");
client.Add("FROM pokemon");
```

---

SQLを実行。  
実行方法は以下の3つ。  

| 実行方法 | 説明 | 戻り値の型 |
| ---- | ---- | ---- |
| Select | 単一のレコードを取得。 | Dictionary<string, object> |
| SelectAll | 複数のレコードを取得。 | List<Dictionary<string, object>> |
| Execute | 戻り値のないSQLの実行。 | void |

---

バインド機構(クエリ化文字列)を使用する場合には`AddParam`メソッドを使用する。  
引数は以下の通り。  

```vb
client.AddParam("@パラメタ名", "値");
```
