using System.Data;
using System.Data.SQLite;

namespace TestingSQLITE.Data
{
	internal class Connection
	{
		SQLiteConnection _connection;
		public Connection()
		{
			_connection = new SQLiteConnection("Data Source=database.db;Version=3;");
			Console.WriteLine($"Database Path: {System.IO.Path.GetFullPath("database.db")}");
			_connection.Open();
		}

		public SQLiteConnection Db()
		{
			return _connection;
		}

		~Connection()
		{
			_connection.Close();
		}
	}
	
	static class Db
	{
		private static Connection _conn = new Connection();
		public static bool NonQuery(string query)
		{
			try
			{
				using (var cmd = new SQLiteCommand(query, _conn.Db()))
				{
					cmd.ExecuteNonQuery();
					return true;
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);

				return false;
			}
		}

		public static DataTable GetQuery(string query)
		{
			try
			{
				using (var cmd = new SQLiteCommand(query, _conn.Db()))
				{
					var dt = new DataTable();
					dt.Load(cmd.ExecuteReader());
					return dt;
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return new DataTable();
			}
		}

	}
}