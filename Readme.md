<?xml version="1.0" encoding="utf-8"?>
<ClassDiagram /> 

# C# 0. Simple C# SQLite CRUD

### Repository ini berisi contoh bagaimana cara melakukan CRUD menggunakan SQLite di C#

Link Repo : https://github.com/MasFana/C-Simple-Sqlite-CRUD


NuGet Package yang digunakan : 

>System.Data.SQLite

- [Koneksi ke Database  ](#koneksi-ke-database)
- [Interface Siswa dan SiswaService](#interface-siswa-dan-siswaservice)
- [Class Siswa dan SiswaService](#class-siswa-dan-siswaservice)

---


### Koneksi ke Database  

1. **Class Connection**   
  untuk menyimpan konfigurasi database dan inisiasi koneksi ke database SQLite dan juga menutup koneksi saat program ditutup.

<details>
<summary> Connection </summary>

```cs
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
	
```
</details>

2. **Static Class Db**   
  meyimpan object inisiasi dari **Class Connection** supaya dapat digunakan di semua project, disini menggunakan static class agar tidak perlu untuk membuat object baru jika ingin melakukan Query ke database.  
> Disini terdapat 2 Method **NonQuery** dan **GetQuery** perbedaanya sendiri yaitu **GetQuery** terdapat reader untuk perintah **SELECT**.

<details>
<summary> Db </summary>

```cs
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
```
</details>

---

### Interface Siswa dan SiswaService
Disini terdapat **Class Siswa** dan **Class SiswaService**. yang mengimplement method yang didefinisikan di **Interface** mereka Masing masing.

1. Interface **Siswa** dan **SiswaService** 
  Disini kita membuat **Interface**, untuk mengspesifikasikan **Method** apa yang harus diimplementasikan di **Setiap Classnya**.

<details>
<summary> Interface Siswa dan SiswaService </summary>

```cs
	internal interface ISiswa
	{
		bool UpdateSiswa(string nama, string alamat, string kelas);
		bool DeleteSiswa();
	}
	internal interface ISiswaService
	{
		List<Siswa> GetAllSiswa();
		bool InsertSiswa(string nama, string alamat, string kelas);
	}
```

</details>

- Dapat dilihat bahwa **SiswaService** memiliki **Method** untuk **GetAllSiswa()** yang mana me-return **List object Siswa** dan setiap **Siswa** terdapat method untuk **UpdateSiswa()** dan **DeleteSiswa()**.
- Dan juga untuk menambahkan siswa kita menyimpan **Methodnya** di **SiswaService**.

### Class Siswa dan SiswaService
Disini kita mengimplementasikan Interface yang tadi kita definisikan.

1. **Class Siswa**

<details>
<summary> Siswa </summary>

```cs
namespace TestingSQLITE.Data.Siswa
{
	internal class Siswa : ISiswa
	{
		public int Id { get; set; }
		public string Nama { get; set; }
		public string Alamat { get; set; }
		public string Kelas { get; set; }



		public bool UpdateSiswa(string nama, string alamat, string kelas)
		{
			nama = string.IsNullOrEmpty(nama) ? Nama : nama;
			alamat = string.IsNullOrEmpty(alamat) ? Alamat : alamat;
			kelas = string.IsNullOrEmpty(kelas) ? Kelas : kelas;
			var query = $"UPDATE siswa SET nama = '{nama}', alamat = '{alamat}', kelas = '{kelas}' WHERE id = {Id}";
			return Db.NonQuery(query);
		}
		public bool DeleteSiswa()
		{
			var query = $"DELETE FROM siswa WHERE id = {Id}";
			return Db.NonQuery(query);
		}
	}
}
	
```

</details>

2. **Class SiswaService**
<details>
<summary> SiswaService </summary>

```cs
using System.Data;

namespace TestingSQLITE.Data.Siswa
{
	internal class SiswaService : ISiswaService
	{

		private string CreateTable = "CREATE TABLE IF NOT EXISTS siswa (id INTEGER PRIMARY KEY AUTOINCREMENT, nama TEXT, alamat TEXT, kelas TEXT)";

		public SiswaService()
		{
			Db.NonQuery(CreateTable);
		}

		public List<Siswa> GetAllSiswa()
		{
			List<Siswa> listSiswa = new List<Siswa>();
			var siswa = Db.GetQuery("SELECT * FROM siswa");
			if (siswa == null) return listSiswa;
			foreach (DataRow row in siswa.Rows)
			{
				listSiswa.Add(new Siswa
				{
					Id = Convert.ToInt32(row["id"]),
					Nama = row["nama"]?.ToString() ?? string.Empty,
					Alamat = row["alamat"]?.ToString() ?? string.Empty,
					Kelas = row["kelas"]?.ToString() ?? string.Empty
				});
			}
			return listSiswa;
		}

		public bool InsertSiswa(string nama, string alamat, string kelas)
		{
			var query = $"INSERT INTO siswa (nama, alamat, kelas) VALUES ('{nama}', '{alamat}', '{kelas}')";
			return Db.NonQuery(query);
		}
	}
}

```

</details>
