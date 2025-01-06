# C# Simple C# SQLite CRUD

Repository ini berisi contoh bagaimana cara melakukan CRUD menggunakan SQLite di C#.  
**Link Repository:** [C-Simple-Sqlite-CRUD](https://github.com/MasFana/C-Simple-Sqlite-CRUD)

### **NuGet Package yang Digunakan**:
- `System.Data.SQLite`

---

### **Daftar Isi**
1. [Koneksi ke Database](#koneksi-ke-database)
2. [Interface Siswa dan SiswaService](#interface-siswa-dan-siswaservice)
3. [Class Siswa dan SiswaService](#class-siswa-dan-siswaservice)

---

## **Koneksi ke Database**

### **1. Class Connection**
Class ini digunakan untuk menyimpan konfigurasi database, inisialisasi koneksi SQLite, dan memastikan koneksi ditutup saat program selesai.

<details>
<summary>Kode Class Connection</summary>

```csharp
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
}
```

</details>

---

### **2. Static Class Db**
Static class ini berfungsi menyimpan instance dari `Connection` sehingga dapat digunakan di seluruh proyek tanpa perlu membuat instance baru.  
Class ini memiliki dua metode:
- **`NonQuery`**: Untuk perintah non-`SELECT` (seperti `INSERT`, `UPDATE`, `DELETE`).
- **`GetQuery`**: Untuk perintah `SELECT` yang mengembalikan data.

<details>
<summary>Kode Static Class Db</summary>

```csharp
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
```

</details>

---

## **Interface Siswa dan SiswaService**

### **1. Definisi Interface**
Interface ini digunakan untuk mendefinisikan metode yang harus diimplementasikan pada setiap class.  
- `ISiswa`: Mendefinisikan operasi yang dapat dilakukan pada entitas siswa, seperti `UpdateSiswa` dan `DeleteSiswa`.
- `ISiswaService`: Mendefinisikan operasi untuk mengelola data siswa, seperti `GetAllSiswa` dan `InsertSiswa`.

<details>
<summary>Kode Interface</summary>

```csharp
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

---

## **Class Siswa dan SiswaService**

### **1. Class Siswa**
Class ini mengimplementasikan interface `ISiswa` untuk mendukung operasi pembaruan (`Update`) dan penghapusan (`Delete`) data siswa.

<details>
<summary>Kode Class Siswa</summary>

```csharp
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

---

### **2. Class SiswaService**
Class ini bertugas mengelola daftar siswa dan mengimplementasikan interface `ISiswaService`.

<details>
<summary>Kode Class SiswaService</summary>

```csharp
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
