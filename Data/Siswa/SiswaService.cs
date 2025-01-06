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
